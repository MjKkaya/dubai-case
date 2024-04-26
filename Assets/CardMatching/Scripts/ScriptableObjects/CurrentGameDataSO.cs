using System.Collections.Generic;
using CardMatching.Datas;
using CardMatching.Events;
using CardMatching.GridBox;
using CardMatching.Utilities;
using UnityEngine;


namespace CardMatching.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CurrentGameDataSO", menuName = "CardMatching/CurrentGameDataSO")]
    [SerializeField]
    public class CurrentGameDataSO : ScriptableObject
    {
        public const int EmptyBoxIconIndex = -1;

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

        private GridBoxCardItem[,] _gridBoxCardItems;
        private bool isGameCompleted; 


        private void OnEnable()
        {
            Reset();
            GameEvents.GameStarted += GameEvents_GameStarted;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
            GameEvents.EarnedPoint += GameEvents_EarnedPoint;
            GameEvents.EarnedComboPoint += GameEvents_EarnedComboPoint;
        }

        private void OnDisable()
        {
            GameEvents.GameStarted -= GameEvents_GameStarted;
            GameEvents.MatchingCard -= GameEvents_MatchingCard;
            GameEvents.MismatchingCard -= GameEvents_MismatchingCard;
            GameEvents.EarnedPoint -= GameEvents_EarnedPoint;
            GameEvents.EarnedComboPoint -= GameEvents_EarnedComboPoint;
        }


        public void PrepareOneDimensionArray()
        {
            IconIndexArray = Tools.ConvertGridBoxCardItemsToOneDimension(_gridBoxCardItems);
        }

        private void Reset()
        {
            _gridBoxCardItems = null;
            IconIndexArray = null;
            PairCount = 0;
            GridAreaDimensionX = 0;
            GridAreaDimensionY = 0;
            
            Score = 0;
            IsComboActive = false;
            MatchesCount = 0;
            TurnCount = 0;
        }

        private void CheckGameOver()
        {
            if (MatchesCount == PairCount)
            {
                Reset();
                isGameCompleted = true;
                GameEvents.GameOver?.Invoke();
            }
        }


        private void GameEvents_GameStarted(GridDimension gridDimension, GridBoxCardItem[,] gridBoxCardItems)
        {
            _gridBoxCardItems = gridBoxCardItems;
            IconIndexArray = Tools.ConvertGridBoxCardItemsToOneDimension(_gridBoxCardItems);
            PairCount = gridDimension.X * gridDimension.Y / 2;
            GridAreaDimensionX = gridDimension.X;
            GridAreaDimensionY = gridDimension.Y;
            isGameCompleted = false;
        }

        private void GameEvents_EarnedPoint(float point)
        {
            if (isGameCompleted)
                return;
            Score += point;
        }

        private void GameEvents_EarnedComboPoint(float point)
        {
            if (isGameCompleted)
                return;
            Score += point;
            IsComboActive = true;
        }

        private void GameEvents_MatchingCard(List<GridBoxCardItem> cardList)
        {
            MatchesCount++;
            TurnCount++;
            CheckGameOver();
        }

        private void GameEvents_MismatchingCard(List<GridBoxCardItem> cardList)
        {
            IsComboActive = false;
            TurnCount++;
        }
    }
}