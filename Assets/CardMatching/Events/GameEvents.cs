using System;


namespace CardMatching.Events
{
    public static class GameEvents
    {
        #region Gameplay events

        public static Action MatchingCard;
        public static Action MismatchingCard;
        public static Action GameOver;

        #endregion
    }
}