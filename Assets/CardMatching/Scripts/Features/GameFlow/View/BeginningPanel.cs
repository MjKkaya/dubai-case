using System;
using CardMatching.Core.CoreServices;
using CardMatching.Core.Events.Signals;
using CardMatching.Core.Utils;
using CardMatching.Features.GameFlow.Signals;
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;


namespace CardMatching.Features.GameFlow.View
{
    public class BeginningPanel : BasePanel, IInitializable
    {
        [SerializeField] private Button _playButton;
        
        private IPublisher<NewGameRequestedSignal> _newGameRequestedPub;
        private ISubscriber<BeginningPanelShowSignal> _showPanelSub;
        private ISubscriber<GameOverSignal> _gameOverSub;
        private IDisposable _disposables;

        
        protected override void Awake()
        {
            base.Awake();
            _playButton.onClick.AddListener(OnClickedNewGameButton);
            CustomDebug.Log("BeginningPanel-Awake");
        }
        
        [Inject]
        public void Construct(IPublisher<NewGameRequestedSignal> newGameRequestedPub, ISubscriber<BeginningPanelShowSignal> showPanelSub, ISubscriber<GameOverSignal> gameOverSub)
        {
            CustomDebug.Log("BeginningPanel-Construct");
            _newGameRequestedPub = newGameRequestedPub;
            _showPanelSub = showPanelSub;
            _gameOverSub = gameOverSub;
        }

        public void Initialize()
        {
            CustomDebug.Log("BeginningPanel-Initialize");

            var bag = DisposableBag.CreateBuilder();
            _showPanelSub.Subscribe(OnShowPanelSignal).AddTo(bag);
            _gameOverSub.Subscribe(OnGameOverSignal).AddTo(bag);

            _disposables = bag.Build();
        }


        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnClickedNewGameButton);
            _disposables?.Dispose();
        }


        private void OnClickedNewGameButton()
        {
            _newGameRequestedPub.Publish(new NewGameRequestedSignal());
            HidePanel();
        }

        private void OnShowPanelSignal(BeginningPanelShowSignal signal)
        {
            ShowPanel();
        }

        private void OnGameOverSignal(GameOverSignal signal)
        {
            ShowPanel();
        }
    }
}