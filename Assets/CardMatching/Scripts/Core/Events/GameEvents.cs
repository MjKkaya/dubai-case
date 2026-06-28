using CardMatching.Core.Datas;
using CardMatching.Core.ScriptableObjects;
using System;
using CardMatching.Core.Interfaces;


namespace CardMatching.Core.Events
{
    public static class GameEvents
    {
        //Prepare new game with unfinished game data;
        public static Action<CurrentGameDataSO> UnfinishedGameStarting;
        public static Action NewGameStarting;

        //It returns the grid are dimension.
        public static Action<GridDimension, IGridBoxCardItem[,]> GameStarted;

        //It's return selected card icon index number.
        public static Action CardSelected;
        public static Action<IGridBoxCardItem> CardFlipped;
        public static Action StartedCardMatchingControl;
        public static Action<IGridBoxCardItem, IGridBoxCardItem> MatchingCard;
        public static Action MismatchingCard;
        public static Action GameOver;

        public static Action<float> EarnedPoint;
        public static Action<float> EarnedComboPoint;
    }
}