using CardMatching.Core.Events;
using CardMatching.Core.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace CardMatching.UI.Panels
{
    public class UnfinishedLevelProgressPanel : BasePanel
    {
        [HideInInspector]
        public CurrentGameDataSO CurrentGameData;

        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        
        private GameEvents _gameEvents;
        

        protected override void Awake()
        {
            base.Awake();
            _continueButton.onClick.AddListener(OnClickedContinueButton);
            _newGameButton.onClick.AddListener(OnClickedNewGameButton);
        }
        
        [Inject]
        public void Construct(GameEvents gameEvents)
        {
            _gameEvents = gameEvents;
        }
        


        private void OnDestroy()
        {
            _continueButton.onClick.RemoveListener(OnClickedContinueButton);
            _newGameButton.onClick.RemoveListener(OnClickedNewGameButton);
        }


        public override void HidePanel()
        {
            base.HidePanel();
            CurrentGameData = null;
        }

        private void OnClickedContinueButton()
        {
            _gameEvents.UnfinishedGameStarting?.Invoke(CurrentGameData);
            HidePanel();
        }

        private void OnClickedNewGameButton()
        {
            _gameEvents.NewGameStarting?.Invoke();
            HidePanel();
        }
    }
}