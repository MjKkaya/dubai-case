using System;
using CardMatching.Core.Events;
using CardMatching.Core.Interfaces;
using CardMatching.Core.Settings;
using UnityEngine;
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
        private readonly GameEvents _gameEvents;
        private readonly CurrentGameDataSO _currentGameData;


        public ScoreManager(int minimumStreak, GameEvents gameEvents, CurrentGameDataSO currentGameData)
        {
            _minimumStreak = minimumStreak;
            _gameEvents = gameEvents;
            _currentGameData = currentGameData;
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
            if (_currentGameData.IsGameCompleted) 
                return;

            _currentStreak++;
            _currentGameData.Score += _correctAnswerPoint;
            _gameEvents.EarnedPoint?.Invoke(_correctAnswerPoint);

            if(_currentStreak > _minimumStreak)
            {
                _currentGameData.Score += _correctAnswerPoint;
                _currentGameData.IsComboActive = true;
                _gameEvents.EarnedComboPoint?.Invoke(_comboPoint);
            }
        }

        private void GameEvents_MismatchingCard()
        {
            _currentStreak = 0;
            _currentGameData.IsComboActive = false;
        }
    }
}