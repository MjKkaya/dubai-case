using System;
using CardMatching.Core.Events;
using CardMatching.Core.ScriptableObjects;
using CardMatching.Utilities;
using UnityEngine;
using VContainer.Unity;


namespace CardMatching.Infrastructure
{
    public class UnfinishedLevelProgressManager: IStartable, IDisposable
    {
        private const string _unfinishedGameDataKey = "unfinished-game-data";

        private readonly CurrentGameDataSO _currentGameData;
        private readonly GameEvents _gameEvents;
        private readonly UIEvents _uiEvents;
        
        public UnfinishedLevelProgressManager(CurrentGameDataSO currentGameData, GameEvents gameEvents, UIEvents uiEvents)
        {
            _currentGameData = currentGameData;
            _gameEvents = gameEvents;
            _uiEvents = uiEvents;
        }
        
        public void Start()
        {
            Application.quitting += SaveOnQuit; 
            _gameEvents.NewGameStarting += GameEvents_GameStarting;
            _gameEvents.GameOver += GameEvents_GameOver;
            _gameEvents.ApplicationPaused += GameEvents_ApplicationPaused;
            
            LoadLastUnfinishedGameData();
            if (_currentGameData.TurnCount > 0)
                _uiEvents.UnfinishedLevelProgressPanelShow?.Invoke(_currentGameData);
            else
                _uiEvents.BeginningPanelShow?.Invoke();
        }
        
        public void Dispose()
        {
            Application.quitting -= SaveOnQuit;

            if (_gameEvents != null)
            {
                _gameEvents.NewGameStarting -= GameEvents_GameStarting;
                _gameEvents.GameOver -= GameEvents_GameOver;
                _gameEvents.ApplicationPaused -= GameEvents_ApplicationPaused;
            }
        }
        

        private void SaveLastUnfinishedGameData()
        {
            CustomDebug.Log($"{this}-SaveLastUnfinishedGameData");
            PlayerPrefs.SetString(_unfinishedGameDataKey, JsonUtility.ToJson(_currentGameData));
            PlayerPrefs.Save();
        }

        private void LoadLastUnfinishedGameData()
        {
            string data = PlayerPrefs.GetString(_unfinishedGameDataKey, null);
            if(!string.IsNullOrEmpty(data))
                JsonUtility.FromJsonOverwrite(data, _currentGameData);
        }

        private void DeleteUnfinishedGameData()
        {
            PlayerPrefs.DeleteKey(_unfinishedGameDataKey);
        }


        private void GameEvents_GameStarting()
        {
            DeleteUnfinishedGameData();
        }

        private void GameEvents_GameOver()
        {
            DeleteUnfinishedGameData();
        }
        
        private void SaveOnQuit()
        {
            CustomDebug.Log($"{this}-SaveOnQuit-TurnCount:{_currentGameData.TurnCount}");
            if (_currentGameData.TurnCount > 0)
            {
                _currentGameData.PrepareOneDimensionArray();
                SaveLastUnfinishedGameData();
            }
        }
        
        private void GameEvents_ApplicationPaused(bool isPaused)
        {
            if (isPaused)
                SaveOnQuit();
        }
    }
}