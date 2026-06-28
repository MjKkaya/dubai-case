using System;
using CardMatching.Core.ScriptableObjects;


namespace CardMatching.Core.Events
{
    public static class UIEvents
    {
        // Show the beginningPanel
        public static Action BeginningPanelShow;

        // Show the UnfinishedLevelProgressPanel
        public static Action<CurrentGameDataSO> UnfinishedLevelProgressPanelShow;
    }
}