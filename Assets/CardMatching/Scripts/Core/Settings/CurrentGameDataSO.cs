using CardMatching.Core.Interfaces;
using CardMatching.Core.Utils;
using UnityEngine;


namespace CardMatching.Core.Settings
{
    [CreateAssetMenu(fileName = "CurrentGameDataSO", menuName = "CardMatching/CurrentGameDataSO")]
    public class CurrentGameDataSO : ScriptableObject
    {
        public bool IsGameRunning => GridBoxCardItems != null && !IsGameCompleted;
        
        //We prevent Unity's JSON engine and the Inspector from crashing when trying to read this variable!        
        [System.NonSerialized] 
        public IGridBoxCardItem[,] GridBoxCardItems;
        
        public bool IsGameCompleted; 

        [Header("Only set at the beginning of the game")]
        public int[] IconIndexArray;
        public int PairCount;
        public int GridAreaDimensionX;
        public int GridAreaDimensionY;
        

        [Header("It's updated during game.")]
        public float Score;
        public bool IsComboActive;
        public int MatchesCount;
        public int TurnCount;

        
        public void PrepareOneDimensionArray()
        {
            if (GridBoxCardItems != null)
                IconIndexArray = Tools.ConvertGridBoxCardItemsToOneDimension(GridBoxCardItems);
        }

        public void ResetData()
        {
            GridBoxCardItems = null;
            IconIndexArray = null;
            PairCount = 0;
            GridAreaDimensionX = 0;
            GridAreaDimensionY = 0;
            
            Score = 0;
            IsComboActive = false;
            MatchesCount = 0;
            TurnCount = 0;
            IsGameCompleted = false;
        }
    }
}