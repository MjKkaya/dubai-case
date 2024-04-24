using System;
using CardMatching.GridBox;


namespace CardMatching.Events
{
    public static class GameEvents
    {
        //It has to send paircount of the game.
        public static Action GameStarting;
        public static Action<int> GameStarted;

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