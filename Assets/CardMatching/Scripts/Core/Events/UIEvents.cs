using System;
using CardMatching.Core.Settings;


namespace CardMatching.Core.Events
{
    public class UIEvents
    {
        // Show the beginningPanel
        public Action BeginningPanelShow;

        // Show the UnfinishedLevelProgressPanel
        public Action<CurrentGameDataSO> UnfinishedLevelProgressPanelShow;

        public Action NewGameRequested;
    }
}