using System;
using CardMatching.Core.CoreServices;
using CardMatching.Core.Events.Signals;
using CardMatching.Core.Settings;
using CardMatching.Core.Utils;
using CardMatching.Features.GameFlow.Signals;
using CardMatching.Features.LevelProgression.Signals;
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;


namespace CardMatching.Features.LevelProgression.View
{
    public class UnfinishedLevelProgressPanel : BasePanel, IInitializable
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        
        private CurrentGameDataSO _currentGameData;
        
        private ISubscriber<UnfinishedLevelProgressPanelShowSignal> _panelShowSub;
        private IPublisher<NewGameRequestedSignal> _newGameRequestedPub;
        private IPublisher<UnfinishedGameStartingSignal> _unfinishedGameStartingPub;
        private IDisposable _disposables;
        

        protected override void Awake()
        {
            base.Awake();
            _continueButton.onClick.AddListener(OnClickedContinueButton);
            _newGameButton.onClick.AddListener(OnClickedNewGameButton);
        }
        
        [Inject]
        public void Construct(ISubscriber<UnfinishedLevelProgressPanelShowSignal> panelShowSub,
        IPublisher<NewGameRequestedSignal> newGameRequestedPub,
        IPublisher<UnfinishedGameStartingSignal> unfinishedGameStartingPub)
        {
            CustomDebug.Log("UnfinishedLevelProgressPanel-Construct");
            _panelShowSub = panelShowSub;
            _newGameRequestedPub = newGameRequestedPub;
            _unfinishedGameStartingPub = unfinishedGameStartingPub;
        }
        
        public void Initialize()
        {
            CustomDebug.Log("UnfinishedLevelProgressPanel-Initialize");
            var bag = DisposableBag.CreateBuilder();
            _panelShowSub.Subscribe(OnShowPanelSignal).AddTo(bag);
            _disposables = bag.Build();
        }


        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(OnClickedContinueButton);
            _newGameButton.onClick.RemoveListener(OnClickedNewGameButton);
            _disposables?.Dispose();
        }


        public override void HidePanel()
        {
            base.HidePanel();
            _currentGameData = null;
        }

        private void OnClickedContinueButton()
        {
            _unfinishedGameStartingPub.Publish(new UnfinishedGameStartingSignal(){ GameData = _currentGameData});
            HidePanel();
        }

        private void OnClickedNewGameButton()
        {
            _newGameRequestedPub.Publish(new NewGameRequestedSignal());
            HidePanel();
        }

        private void OnShowPanelSignal(UnfinishedLevelProgressPanelShowSignal signal)
        {
            CustomDebug.Log("UnfinishedLevelProgressPanel-OnShowPanelSignal");
            _currentGameData = signal.GameData;
            ShowPanel();
        }

        
    }
}