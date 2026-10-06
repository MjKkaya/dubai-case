using CardMatching.Core.Interfaces;
using CardMatching.Core.Settings;


namespace CardMatching.Core.Events.Signals
{
    public struct NewGameStartingSignal { }

    public struct UnfinishedGameStartingSignal { public CurrentGameDataSO GameData; }

    public struct GameStartedSignal
    {
        public GridDimension Dimension;
        public IGridBoxCardItem[,] GridItems;
    }

    public struct GameOverSignal { }

    public struct ApplicationPausedSignal { public bool IsPaused; }
}