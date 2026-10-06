using System;
using CardMatching.Core.Events.Signals;
using CardMatching.Core.Interfaces;
using CardMatching.Core.Settings;
using CardMatching.Core.Utils;
using CardMatching.Features.GameFlow.Signals;
using CardMatching.Features.LevelProgression.Signals;
using MessagePipe;
using UnityEngine;
using VContainer.Unity;


namespace CardMatching.Features.LevelProgression.Services
{
    public class UnfinishedLevelProgressManager : IStartable, IDisposable
    {
        private const string _unfinishedGameDataKey = "unfinished-game-data";

        private readonly CurrentGameDataSO _currentGameData;
        private readonly ISaveRepository _saveRepository;

        private readonly IPublisher<UnfinishedLevelProgressPanelShowSignal> _unfinishedLevelProgressPanelShowPub;
        private readonly IPublisher<BeginningPanelShowSignal> _beginningPanelShowPub;
        private readonly ISubscriber<NewGameStartingSignal> _newGameStartingSub;
        private readonly ISubscriber<GameOverSignal> _gameOverSub;
        private readonly ISubscriber<ApplicationPausedSignal> _applicationPausedSub;
        private IDisposable _disposables;



        public UnfinishedLevelProgressManager(CurrentGameDataSO currentGameData, ISaveRepository saveRepository, IPublisher<UnfinishedLevelProgressPanelShowSignal> unfinishedLevelProgressPanelShowPub, IPublisher<BeginningPanelShowSignal> beginningPanelShowPub, ISubscriber<NewGameStartingSignal> newGameStartingSub, ISubscriber<GameOverSignal> gameOverSub, ISubscriber<ApplicationPausedSignal> applicationPausedSub)
        {
            _currentGameData = currentGameData;
            _saveRepository = saveRepository;

            _unfinishedLevelProgressPanelShowPub = unfinishedLevelProgressPanelShowPub;
            _beginningPanelShowPub = beginningPanelShowPub;

            _newGameStartingSub = newGameStartingSub;
            _gameOverSub = gameOverSub;
            _applicationPausedSub = applicationPausedSub;
        }

        public void Start()
        {
            CustomDebug.Log("UnfinishedLevelProgressManager-Start");
            Application.quitting += SaveOnQuit;

            var bag = DisposableBag.CreateBuilder();
            _newGameStartingSub.Subscribe(OnNewGameStartingSignal).AddTo(bag);
            _gameOverSub.Subscribe(OnGameOverSignal).AddTo(bag);
            _applicationPausedSub.Subscribe(OnApplicationPausedSignal).AddTo(bag);
            _disposables = bag.Build();


            LoadLastUnfinishedGameData();

            if (_currentGameData.TurnCount > 0)
                _unfinishedLevelProgressPanelShowPub.Publish(new UnfinishedLevelProgressPanelShowSignal(){GameData = _currentGameData});
            else
                _beginningPanelShowPub.Publish(new BeginningPanelShowSignal());
        }


        public void Dispose()
        {
            Application.quitting -= SaveOnQuit;
            _disposables?.Dispose();
        }


        private void SaveLastUnfinishedGameData()
        {
            CustomDebug.Log($"{this}-SaveLastUnfinishedGameData");
            _saveRepository.Save(_unfinishedGameDataKey, _currentGameData);
        }

        private void LoadLastUnfinishedGameData()
        {
            _saveRepository.LoadInto(_unfinishedGameDataKey, _currentGameData);
        }

        private void DeleteUnfinishedGameData()
        {
            _saveRepository.Delete(_unfinishedGameDataKey);
        }


        #region Events

        private void OnNewGameStartingSignal(NewGameStartingSignal signal)
        {
            DeleteUnfinishedGameData();
        }

        private void OnGameOverSignal(GameOverSignal signal)
        {
            DeleteUnfinishedGameData();
        }

        private void OnApplicationPausedSignal(ApplicationPausedSignal signal)
        {
            if (signal.IsPaused)
                SaveOnQuit();
        }

        private void SaveOnQuit()
        {
            CustomDebug.Log($"{this}-SaveOnQuit-TurnCount:{_currentGameData.TurnCount}/{_currentGameData.IsGameRunning}");
            if (_currentGameData.IsGameRunning)
            {
                _currentGameData.PrepareOneDimensionArray();
                SaveLastUnfinishedGameData();
            }
        }

        #endregion


        
    }
}