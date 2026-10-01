using CardMatching.Core.CoreServices;
using CardMatching.Core.Events;
using CardMatching.Core.Utils;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace CardMatching.GameFlow.View
{
    public class BeginningPanel : BasePanel
    {
        [SerializeField] private Button _playButton;
        
        private GameEvents _gameEvents;
        private UIEvents _uiEvents;

        
        protected override void Awake()
        {
            base.Awake();
            _playButton.onClick.AddListener(OnClickedPlayButton);
            CustomDebug.Log("BeginningPanel-Awake");
        }
        
        [Inject]
        public void Construct(GameEvents gameEvents, UIEvents uiEvents)
        {
            _gameEvents = gameEvents;
            _gameEvents.GameOver += GameEvents_GameOver;
            _uiEvents = uiEvents;
            CustomDebug.Log("BeginningPanel-Construct");
        }

        public override void Initialize()
        {
            _uiEvents.BeginningPanelShow += OnShowEvent;
            CustomDebug.Log("BeginningPanel-Initialize");
        }

        void Start()
        {
            CustomDebug.Log("BeginningPanel-Start");
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnClickedPlayButton);
            _gameEvents.GameOver -= GameEvents_GameOver;

            if (_uiEvents != null) 
                _uiEvents.BeginningPanelShow -= OnShowEvent;
        }


        private void OnClickedPlayButton()
        {
            _gameEvents.NewGameStarting?.Invoke();
            HidePanel();
        }

        private void OnShowEvent()
        {
            ShowPanel();
        }

        private void GameEvents_GameOver()
        {
            ShowPanel();
        }
    }
}