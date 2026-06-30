using System;
using CardMatching.Core.Events;
using CardMatching.Core.Interfaces;
using UnityEngine;
using VContainer.Unity;


namespace CardMatching.Gameplay
{
    public class ScoreManager : IInitializable, IDisposable
    {
        private const float _comboPoint = 10;
        private const float _correctAnswerPoint = 5;

        [Tooltip("This is the minimum correct answers in a row to get Combo point")]
        private readonly int _minimumStreak = 2;

        private int _currentStreak;
        private readonly GameEvents _gameEvents;

        public ScoreManager(int minimumStreak, GameEvents gameEvents)
        {
            _minimumStreak = minimumStreak;
            _gameEvents = gameEvents;
        }
        
        public void Initialize()
        {
            _gameEvents.NewGameStarting += GameEvents_GameStarting;
            _gameEvents.MatchingCard += GameEvents_MatchingCard;
            _gameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }
        public void Dispose()
        {
            _gameEvents.NewGameStarting -= GameEvents_GameStarting;
            _gameEvents.MatchingCard -= GameEvents_MatchingCard;
            _gameEvents.MismatchingCard -= GameEvents_MismatchingCard;
        }
        

        private void GameEvents_GameStarting()
        {
            _currentStreak = 0;
        }

        private void GameEvents_MatchingCard(IGridBoxCardItem firstSelectedCardOne, IGridBoxCardItem secondSelectedCard)
        {
            _currentStreak++;
            _gameEvents.EarnedPoint?.Invoke(_correctAnswerPoint);

            if(_currentStreak > _minimumStreak)
                _gameEvents.EarnedComboPoint?.Invoke(_comboPoint);
        }

        private void GameEvents_MismatchingCard()
        {
            _currentStreak = 0;
        }
    }
}