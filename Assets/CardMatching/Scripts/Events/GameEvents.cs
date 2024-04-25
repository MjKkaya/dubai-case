using CardMatching.Datas;
using CardMatching.GridBox;
using CardMatching.ScriptableObjects;
using System;


namespace CardMatching.Events
{
    public static class GameEvents
    {
        //Prepare new game with unfinished game data;
        public static Action<CurrentGameDataSO> StartGameWithUnfinishedGameData;
        public static Action GameStarting;

        //It returns the grid are dimension.
        public static Action<GridDimension, GridBoxCardItem[,]> GameStarted;

        //It's return selected card icon index number.
        public static Action<GridBoxCardItem> CardSelected;
        public static Action CardFlipped;
        public static Action StartedCardMatchingControl;
        public static Action MatchingCard;
        public static Action MismatchingCard;
        public static Action GameOver;

        public static Action<float> EarnedPoint;
        public static Action<float> EarnedComboPoint;
    }
}