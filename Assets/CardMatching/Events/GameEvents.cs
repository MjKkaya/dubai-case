using System;
using CardMatching.Datas;
using CardMatching.GridBox;


namespace CardMatching.Events
{
    public static class GameEvents
    {
        #region Gameplay events

        //It's return selected card icon index number.
        //public static Action<int, GridDimension> CardSelected;
        public static Action<GridBoxCardItem> CardSelected;
        public static Action CardFlipped;

        public static Action StartedCardMatchingControl;
        public static Action MatchingCard;
        public static Action MismatchingCard;
        public static Action GameOver;


        #endregion
    }
}