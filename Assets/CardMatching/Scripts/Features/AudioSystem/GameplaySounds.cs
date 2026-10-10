using System;
using CardMatching.Core.Enums;
using CardMatching.Core.Events.Signals;
using CardMatching.Core.Interfaces;
using CardMatching.Core.Settings;
using CardMatching.Features.MatchMechanic.Signals;
using MessagePipe;
using VContainer.Unity;


namespace CardMatching.Feature.AudioSystem
{
    public class GameplaySounds : IInitializable, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly ISubscriber<CardSelectedSignal> _cardSelectedSub;
        private readonly ISubscriber<MatchingCardSignal> _matchingCardSub;
        private readonly ISubscriber<MismatchingCardSignal> _mismatchingCardSignalSub;
        private readonly ISubscriber<GameOverSignal> _gameOverSub;

        private IDisposable _disposables;

        
        public GameplaySounds(IAudioService audioService, 
        ISubscriber<CardSelectedSignal> cardSelectedSub,
        ISubscriber<MatchingCardSignal> matchingCardSub,
        ISubscriber<MismatchingCardSignal> mismatchingCardSignalSub,
        ISubscriber<GameOverSignal> gameOverSub)
        {
            _audioService = audioService;

            _cardSelectedSub = cardSelectedSub;
            _matchingCardSub = matchingCardSub;
            _mismatchingCardSignalSub = mismatchingCardSignalSub;
            _gameOverSub = gameOverSub;
        }
        
        
        public void Initialize()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder(4);

            _cardSelectedSub.Subscribe(OnCardSelected).AddTo(bag);
            _matchingCardSub.Subscribe(OnMatchingCard).AddTo(bag);
            _mismatchingCardSignalSub.Subscribe(OnMismatchingCard).AddTo(bag);
            _gameOverSub.Subscribe(OnGameOver).AddTo(bag);

            _disposables = bag.Build();
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }


        #region Events

        // Play the flipping card sound effect
        private void OnCardSelected(CardSelectedSignal signal)
        {
            _audioService.PlaySFX(AudioID.FlippingCard);
        }

        // Play the matching card sound effect
        private void OnMatchingCard(MatchingCardSignal signal)
        {
            _audioService.PlaySFX(AudioID.MatchingCard);
        }

        // Play the mismatching card sound effect
        private void OnMismatchingCard(MismatchingCardSignal signal)
        {
            _audioService.PlaySFX(AudioID.MismatchingCard);
        }

        // Play the game over sound effect
        private void OnGameOver(GameOverSignal signal)
        {
            _audioService.PlaySFX(AudioID.GameOver);
        }

        #endregion
    }
}
