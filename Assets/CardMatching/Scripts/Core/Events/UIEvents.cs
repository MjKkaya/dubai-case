using System;
using CardMatching.Core.ScriptableObjects;


namespace CardMatching.Core.Events
{
    public class UIEvents
    {
        // Show the beginningPanel
        public Action BeginningPanelShow;

        // Show the UnfinishedLevelProgressPanel
        public Action<CurrentGameDataSO> UnfinishedLevelProgressPanelShow;
    }
}