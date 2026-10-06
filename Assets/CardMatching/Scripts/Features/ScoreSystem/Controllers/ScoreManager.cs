using System;
using CardMatching.Core.Events.Signals;
using CardMatching.Core.Settings;
using CardMatching.Features.MatchMechanic.Signals;
using CardMatching.Features.ScoreSystem.Signals;
using MessagePipe;
using VContainer.Unity;


namespace CardMatching.Features.ScoreSystem.Controllers
{
    public class ScoreManager : IInitializable, IDisposable
    {
        private const float _comboPoint = 10;
        private const float _correctAnswerPoint = 5;

        //This is the minimum correct answers in a row to get Combo point
        private readonly int _minimumStreak = 2;

        private int _currentStreak;
        private readonly CurrentGameDataSO _currentGameData;

        private readonly ISubscriber<NewGameStartingSignal> _newGameStartingSub;
        private readonly ISubscriber<MatchingCardSignal> _matchingCardSub;
        private readonly ISubscriber<MismatchingCardSignal> _mismatchingCardSub;
        private readonly IPublisher<EarnedPointSignal> _earnedPointPub;
        private readonly IPublisher<EarnedComboPointSignal> _earnedComboPointPub;

        IDisposable _disposables;


        public ScoreManager(int minimumStreak, CurrentGameDataSO currentGameData,
        ISubscriber<NewGameStartingSignal> newGameStartingSub,
        ISubscriber<MatchingCardSignal> matchingCardSub,
        ISubscriber<MismatchingCardSignal> mismatchingCardSub,
        IPublisher<EarnedPointSignal> earnedPointPub,
        IPublisher<EarnedComboPointSignal> earnedComboPointPub)
        {
            _minimumStreak = minimumStreak;
            _currentGameData = currentGameData;

            _newGameStartingSub = newGameStartingSub;
            _matchingCardSub = matchingCardSub;
            _mismatchingCardSub = mismatchingCardSub;

            _earnedPointPub = earnedPointPub;
            _earnedComboPointPub = earnedComboPointPub;
        }

        public void Initialize()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder(3);
            _newGameStartingSub.Subscribe(OnGameStarting).AddTo(bag);
            _matchingCardSub.Subscribe(OnMatchingCard).AddTo(bag);
            _mismatchingCardSub.Subscribe(OnMismatchingCard).AddTo(bag);
            _disposables = bag.Build();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }


        private void OnGameStarting(NewGameStartingSignal signal)
        {
            _currentStreak = 0;
        }

        private void OnMatchingCard(MatchingCardSignal signal)
        {
            if (_currentGameData.IsGameCompleted)
                return;

            _currentStreak++;
            _currentGameData.Score += _correctAnswerPoint;
            _earnedPointPub.Publish(new EarnedPointSignal(){Point = _correctAnswerPoint} );

            if (_currentStreak > _minimumStreak)
            {
                _currentGameData.Score += _correctAnswerPoint;
                _currentGameData.IsComboActive = true;
                _earnedComboPointPub.Publish(new EarnedComboPointSignal(){Point = _comboPoint});
            }
        }

        private void OnMismatchingCard(MismatchingCardSignal signal)
        {
            _currentStreak = 0;
            _currentGameData.IsComboActive = false;
        }
    }
}