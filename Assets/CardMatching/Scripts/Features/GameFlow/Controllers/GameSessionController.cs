using System;
using CardMatching.Core.Events;
using CardMatching.Core.Interfaces;
using CardMatching.Core.Settings;
using CardMatching.Core.Utils;
using VContainer.Unity;
using UnityEngine;

namespace CardMatching.Features.GameFlow.Controllers
{
    public class GameSessionController : IInitializable, IDisposable
    {
        private readonly CurrentGameDataSO _currentGameData;
        private readonly GameEvents _gameEvents;
        private readonly UIEvents _uiEvents;

        public GameSessionController(CurrentGameDataSO currentGameData, GameEvents gameEvents, UIEvents uiEvents)
        {
            _currentGameData = currentGameData;
            _gameEvents = gameEvents;
            _uiEvents = uiEvents;
        }

        public void Initialize()
        {
            _uiEvents.NewGameRequested += UIEvents_NewGameRequested;
            _gameEvents.GameStarted += GameEvents_GameStarted;
            _gameEvents.MatchingCard += GameEvents_MatchingCard;
            _gameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }

        public void Dispose()
        {
            _uiEvents.NewGameRequested -= UIEvents_NewGameRequested;
            _gameEvents.GameStarted -= GameEvents_GameStarted;
            _gameEvents.MatchingCard -= GameEvents_MatchingCard;
            _gameEvents.MismatchingCard -= GameEvents_MismatchingCard;
        }

        private void UIEvents_NewGameRequested()
        {
            Debug.Log($"<color=orange>[GameSessionController]</color> NewGameStarting tetiklendi. Eski veri siliniyor.");
            _currentGameData.ResetData();
            _gameEvents.NewGameStarting?.Invoke(); 
        }

        private void GameEvents_GameStarted(GridDimension gridDimension, IGridBoxCardItem[,] gridBoxCardItems)
        {
            try 
            {
                _currentGameData.GridBoxCardItems = gridBoxCardItems;
                _currentGameData.IconIndexArray = Tools.ConvertGridBoxCardItemsToOneDimension(gridBoxCardItems);
                _currentGameData.PairCount = gridDimension.X * gridDimension.Y / 2;
                _currentGameData.GridAreaDimensionX = gridDimension.X;
                _currentGameData.GridAreaDimensionY = gridDimension.Y;
                _currentGameData.IsGameCompleted = false;
            }
            catch(Exception e)
            {
                Debug.LogError($"<color=red>[GameSessionController]</color> HATA OLUŞTU: {e.Message}\n{e.StackTrace}");
            }
        }

        private void GameEvents_MatchingCard(IGridBoxCardItem first, IGridBoxCardItem second)
        {
            _currentGameData.MatchesCount++;
            _currentGameData.TurnCount++;
            CheckGameOver();
        }

        private void GameEvents_MismatchingCard()
        {
            _currentGameData.TurnCount++;
        }

        private void CheckGameOver()
        {
            if (_currentGameData.MatchesCount == _currentGameData.PairCount)
            {
                _currentGameData.IsGameCompleted = true;
                _currentGameData.GridBoxCardItems = null; // Oyun bitti, grid'i temizle
                _gameEvents.GameOver?.Invoke();
            }
        }
    }
}
