using CardMatching.Datas;
using CardMatching.GridBox;
using CardMatching.ScriptableObjects;
using System;


namespace CardMatching.Events
{
    public static class GameEvents
    {
        //Prepare new game with unfinished game data;
        public static Action<CurrentGameDataSO> UnfinishedGameStarting;
        public static Action NewGameStarting;

        //It returns the grid are dimension.
        public static Action<GridDimension, GridBoxCardItem[,]> GameStarted;

        //It's return selected card icon index number.
        public static Action CardSelected;
        public static Action<GridBoxCardItem> CardFlipped;
        public static Action StartedCardMatchingControl;
        public static Action<GridBoxCardItem, GridBoxCardItem> MatchingCard;
        public static Action MismatchingCard;
        public static Action GameOver;

        public static Action<float> EarnedPoint;
        public static Action<float> EarnedComboPoint;
    }
}