using System;
using CardMatching.ScriptableObjects;


namespace CardMatching.Events
{
    public static class UIEvents
    {
        // Show the beginningPanel
        public static Action BeginningPanelShow;

        // Show the UnfinishedLevelProgressPanel
        public static Action<CurrentGameDataSO> UnfinishedLevelProgressPanelShow;
    }
}