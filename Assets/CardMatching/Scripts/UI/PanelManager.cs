using System;
using CardMatching.Core.Events;
using CardMatching.UI.Panels;
using CardMatching.Core.ScriptableObjects;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace CardMatching.UI
{
    public class PanelManager : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private BasePanel _beginningPanel;
        [SerializeField] private UnfinishedLevelProgressPanel _unfinishedLevelProgressPanel;

        private UIEvents _uiEvents;
        
        
        [Inject]
        public void Construct(UIEvents uiEvents)
        {
            _uiEvents = uiEvents;
        }
        
        
        public void Initialize()
        {
            _uiEvents.BeginningPanelShow += UIEvents_BeginningPanelShow;
            _uiEvents.UnfinishedLevelProgressPanelShow += UIEvents_UnfinishedLevelProgressPanelShow;
        }

        public void Dispose()
        {
            _uiEvents.BeginningPanelShow -= UIEvents_BeginningPanelShow;
            _uiEvents.UnfinishedLevelProgressPanelShow -= UIEvents_UnfinishedLevelProgressPanelShow;
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