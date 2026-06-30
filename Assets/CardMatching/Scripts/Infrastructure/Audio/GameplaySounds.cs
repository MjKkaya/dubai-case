using System;
using CardMatching.Core.Events;
using CardMatching.Core.Interfaces;
using CardMatching.Core.ScriptableObjects;
using VContainer.Unity;


namespace CardMatching.Infrastructure.Audio
{
    public class GameplaySounds : IInitializable, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly AudioSettingsSO _audioSettings;
        private readonly GameEvents _gameEvents;

        
        public GameplaySounds(IAudioService audioService, AudioSettingsSO audioSettings, GameEvents gameEvents)
        {
            _audioService = audioService;
            _audioSettings = audioSettings;
            _gameEvents = gameEvents;
        }
        
        
        public void Initialize()
        {
            _gameEvents.CardSelected += GameEvents_CardSelected;
            _gameEvents.MatchingCard += GameEvents_MatchingCard;
            _gameEvents.MismatchingCard += GameEvents_MismatchingCard;
            _gameEvents.GameOver += GameEvents_GameOver;
        }

        public void Dispose()
        {
            _gameEvents.CardSelected -= GameEvents_CardSelected;
            _gameEvents.MatchingCard -= GameEvents_MatchingCard;
            _gameEvents.MismatchingCard -= GameEvents_MismatchingCard;
            _gameEvents.GameOver -= GameEvents_GameOver;
        }


        // Play the flipping card sound effect
        private void GameEvents_CardSelected()
        {
            _audioService.PlaySFX(_audioSettings.FlippingCardSound);
        }

        // Play the matching card sound effect
        private void GameEvents_MatchingCard(IGridBoxCardItem firstSelectedCardOne, IGridBoxCardItem secondSelectedCard)
        {
            _audioService.PlaySFX(_audioSettings.MatchingCardSound);
        }

        // Play the mismatching card sound effect
        private void GameEvents_MismatchingCard()
        {
            _audioService.PlaySFX(_audioSettings.MismatchingCardSound);
        }

        // Play the game over sound effect
        private void GameEvents_GameOver()
        {
            _audioService.PlaySFX(_audioSettings.GameOverSound);
        }

        
    }
}
