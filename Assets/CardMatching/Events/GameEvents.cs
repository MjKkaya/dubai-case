using System;


namespace CardMatching.Events
{
    public static class GameEvents
    {
        #region Gameplay events

        //It's return selected card icon index number.
        public static Action<int> FlippingCard;
        public static Action MatchingCard;
        public static Action MismatchingCard;
        public static Action GameOver;

        #endregion
    }
}