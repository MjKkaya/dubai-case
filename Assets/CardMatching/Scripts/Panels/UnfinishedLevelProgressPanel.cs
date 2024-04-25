using CardMatching.Events;
using CardMatching.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


namespace CardMatching.Panels
{
    public class UnfinishedLevelProgressPanel : BasePanel
    {
        public CurrentGameDataSO CurrentGameData;


        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;


        protected override void Awake()
        {
            base.Awake();
            _continueButton.onClick.AddListener(OnClickedContinueButton);
            _newGameButton.onClick.AddListener(OnClickedNewGameButton);
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
            GameEvents.StartGameWithUnfinishedGameData?.Invoke(CurrentGameData);
            HidePanel();
        }

        private void OnClickedNewGameButton()
        {
            GameEvents.GameStarting?.Invoke();
            //UIEvents.BeginningPanelShow?.Invoke();
            HidePanel();
        }
    }
}