using CardMatching.Core.CoreServices;
using CardMatching.Core.Events;
using CardMatching.Core.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace CardMatching.Features.LevelProgression.View
{
    public class UnfinishedLevelProgressPanel : BasePanel
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        
        private CurrentGameDataSO _currentGameData;
        private GameEvents _gameEvents;
        private UIEvents _uiEvents;
        

        protected override void Awake()
        {
            base.Awake();
            _continueButton.onClick.AddListener(OnClickedContinueButton);
            _newGameButton.onClick.AddListener(OnClickedNewGameButton);
        }
        
        [Inject]
        public void Construct(GameEvents gameEvents, UIEvents uiEvents)
        {
            _gameEvents = gameEvents;
            _uiEvents = uiEvents;
        }
        
        public override void Initialize()
        {
            _uiEvents.UnfinishedLevelProgressPanelShow += OnShowEvent;
        }


        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(OnClickedContinueButton);
            _newGameButton.onClick.RemoveListener(OnClickedNewGameButton);

             if (_uiEvents != null) 
                _uiEvents.UnfinishedLevelProgressPanelShow -= OnShowEvent;
        }


        public override void HidePanel()
        {
            base.HidePanel();
            _currentGameData = null;
        }

        private void OnClickedContinueButton()
        {
            _gameEvents.UnfinishedGameStarting?.Invoke(_currentGameData);
            HidePanel();
        }

        private void OnClickedNewGameButton()
        {
            _uiEvents.NewGameRequested?.Invoke();
            HidePanel();
        }

        private void OnShowEvent(CurrentGameDataSO data)
        {
            _currentGameData = data;
            ShowPanel();
        }
    }
}