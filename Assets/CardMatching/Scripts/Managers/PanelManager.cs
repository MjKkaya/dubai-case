using CardMatching.Events;
using CardMatching.Panels;
using CardMatching.ScriptableObjects;
using UnityEngine;


namespace CardMatching.Managers
{
    public class PanelManager : MonoBehaviour
    {
        [SerializeField] private BasePanel _beginningPanel;
        [SerializeField] private UnfinishedLevelProgressPanel _unfinishedLevelProgressPanel;

        
        private void Awake()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }


        private void SubscribeToEvents()
        {
            UIEvents.BeginningPanelShow += UIEvents_BeginningPanelShow;
            UIEvents.UnfinishedLevelProgressPanelShow += UIEvents_UnfinishedLevelProgressPanelShow;
        }

        private void UnsubscribeFromEvents()
        {
            UIEvents.BeginningPanelShow -= UIEvents_BeginningPanelShow;
            UIEvents.UnfinishedLevelProgressPanelShow -= UIEvents_UnfinishedLevelProgressPanelShow;
        }


        private void UIEvents_BeginningPanelShow()
        {
            _beginningPanel.ShowPanel();
        }

        private void UIEvents_UnfinishedLevelProgressPanelShow(CurrentGameDataSO currentGameData)
        {
            _unfinishedLevelProgressPanel.CurrentGameData = currentGameData;
            _unfinishedLevelProgressPanel.ShowPanel();
        }
    }
}