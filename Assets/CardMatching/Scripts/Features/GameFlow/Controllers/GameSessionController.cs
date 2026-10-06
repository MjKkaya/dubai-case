using System;
using CardMatching.Core.Settings;
using CardMatching.Core.Utils;
using VContainer.Unity;
using UnityEngine;
using MessagePipe;
using CardMatching.Features.GameFlow.Signals;
using CardMatching.Features.MatchMechanic.Signals;
using CardMatching.Core.Events.Signals;


namespace CardMatching.Features.GameFlow.Controllers
{
    public class GameSessionController : IInitializable, IDisposable
    {
        private readonly CurrentGameDataSO _currentGameData;

        // 1. Publishers
        private readonly IPublisher<NewGameStartingSignal> _newGameStartingPub;
        private readonly IPublisher<GameOverSignal> _gameOverPub;

        // Subscribers
        private readonly ISubscriber<NewGameRequestedSignal> _newGameRequestedSub;
        private readonly ISubscriber<GameStartedSignal> _gameStartedSub;
        private readonly ISubscriber<MatchingCardSignal> _matchingCardSub;
        private readonly ISubscriber<MismatchingCardSignal> _mismatchingCardSub;

        private IDisposable _disposables;


        public GameSessionController(CurrentGameDataSO currentGameData,
            IPublisher<NewGameStartingSignal> newGameStartingPub,
            IPublisher<GameOverSignal> gameOverPub,
            ISubscriber<NewGameRequestedSignal> newGameRequestedSub,
            ISubscriber<GameStartedSignal> gameStartedSub,
            ISubscriber<MatchingCardSignal> matchingCardSub,
            ISubscriber<MismatchingCardSignal> mismatchingCardSub)
        {
            _currentGameData = currentGameData;
            _newGameStartingPub = newGameStartingPub;
            _gameOverPub = gameOverPub;
            _newGameRequestedSub = newGameRequestedSub;
            _gameStartedSub = gameStartedSub;
            _matchingCardSub = matchingCardSub;
            _mismatchingCardSub = mismatchingCardSub;
        }

        public void Initialize()
        {
            // MessagePipe'ın "DisposableBag" yapısı: Tüm dinleyicileri bir poşete doldururuz.
            var bag = DisposableBag.CreateBuilder();

            // Olayları dinle (Subscribe) ve poşete ekle (AddTo)
            _newGameRequestedSub.Subscribe(OnNewGameRequested).AddTo(bag);
            _gameStartedSub.Subscribe(OnGameStarted).AddTo(bag);
            _matchingCardSub.Subscribe(OnMatchingCard).AddTo(bag);
            _mismatchingCardSub.Subscribe(OnMismatchingCard).AddTo(bag);

            // Poşetin ağzını bağla
            _disposables = bag.Build();
        }

        public void Dispose()
        {
            // Sınıf yok olduğunda poşeti çöpe at (Memory Leak önlendi!)
            _disposables?.Dispose();
        }

        private void CheckGameOver()
        {
            if (_currentGameData.MatchesCount == _currentGameData.PairCount)
            {
                _currentGameData.IsGameCompleted = true;
                _currentGameData.GridBoxCardItems = null;
                // Oyun bitti sinyali fırlat:
                _gameOverPub.Publish(new GameOverSignal());
            }
        }


        #region Events

        private void OnNewGameRequested(NewGameRequestedSignal signal)
        {
            Debug.Log($"GameSessionController-OnNewGameRequested");
            _currentGameData.ResetData();
            _newGameStartingPub.Publish(new NewGameStartingSignal());
        }

        private void OnGameStarted(GameStartedSignal signal)
        {
            Debug.Log($"GameSessionController-OnGameStarted");
            _currentGameData.GridBoxCardItems = signal.GridItems;
            _currentGameData.IconIndexArray = Tools.ConvertGridBoxCardItemsToOneDimension(signal.GridItems);
            _currentGameData.PairCount = signal.Dimension.X * signal.Dimension.Y / 2;
            _currentGameData.GridAreaDimensionX = signal.Dimension.X;
            _currentGameData.GridAreaDimensionY = signal.Dimension.Y;
            _currentGameData.IsGameCompleted = false;
        }

        private void OnMatchingCard(MatchingCardSignal signal)
        {
            _currentGameData.MatchesCount++;
            _currentGameData.TurnCount++;
            CheckGameOver();
        }

        private void OnMismatchingCard(MismatchingCardSignal signal)
        {
            _currentGameData.TurnCount++;
        }

        #endregion
    }
}
