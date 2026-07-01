using CardMatching.Core.Datas;
using CardMatching.Core.ScriptableObjects;
using System;
using CardMatching.Core.Interfaces;


namespace CardMatching.Core.Events
{
    public class GameEvents
    {
        //Prepare new game with unfinished game data;
        public Action<CurrentGameDataSO> UnfinishedGameStarting;
        public Action NewGameStarting;

        //It returns the grid are dimension.
        public Action<GridDimension, IGridBoxCardItem[,]> GameStarted;

        //It's return selected card icon index number.
        public Action CardSelected;
        public Action<IGridBoxCardItem> CardFlipped;
        public Action StartedCardMatchingControl;
        public Action<IGridBoxCardItem, IGridBoxCardItem> MatchingCard;
        public Action MismatchingCard;
        public Action GameOver;

        public Action<float> EarnedPoint;
        public Action<float> EarnedComboPoint;
        
        // Mobil cihazlarda arka plana atılma durumu
        public Action<bool> ApplicationPaused;
    }
}