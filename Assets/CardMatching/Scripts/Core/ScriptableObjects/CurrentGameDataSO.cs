using CardMatching.Core.Interfaces;
using CardMatching.Core.Datas;
using CardMatching.Core.Events;
using CardMatching.Core.Helpers;
using CardMatching.Utilities;
using UnityEngine;


namespace CardMatching.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CurrentGameDataSO", menuName = "CardMatching/CurrentGameDataSO")]
    public class CurrentGameDataSO : ScriptableObject
    {
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

        private IGridBoxCardItem[,] _gridBoxCardItems;
        private bool isGameCompleted; 
        private GameEvents _gameEvents;

        
        public void Initialize(GameEvents gameEvents)
        {
            _gameEvents = gameEvents; 
            Reset();
            _gameEvents.NewGameStarting += GameEvents_NewGameStarting;
            _gameEvents.GameStarted += GameEvents_GameStarted;
            _gameEvents.MatchingCard += GameEvents_MatchingCard;
            _gameEvents.MismatchingCard += GameEvents_MismatchingCard;
            _gameEvents.EarnedPoint += GameEvents_EarnedPoint;
            _gameEvents.EarnedComboPoint += GameEvents_EarnedComboPoint;
        }
        
        public void Dispose()
        {
            if (_gameEvents == null) 
                return;
            _gameEvents.NewGameStarting -= GameEvents_NewGameStarting;
            _gameEvents.GameStarted -= GameEvents_GameStarted;
            _gameEvents.MatchingCard -= GameEvents_MatchingCard;
            _gameEvents.MismatchingCard -= GameEvents_MismatchingCard;
            _gameEvents.EarnedPoint -= GameEvents_EarnedPoint;
            _gameEvents.EarnedComboPoint -= GameEvents_EarnedComboPoint;
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
            CustomDebug.Log($"{this}-CheckGameOver:{MatchesCount}/{PairCount}");
            if (MatchesCount == PairCount)
            {
                Reset();
                isGameCompleted = true;
                _gameEvents.GameOver?.Invoke();
            }
        }


        private void GameEvents_NewGameStarting()
        {
            CustomDebug.Log($"{this}-GameEvents_NewGameStarting");
            Reset();
        }

        private void GameEvents_GameStarted(GridDimension gridDimension, IGridBoxCardItem[,] gridBoxCardItems)
        {
            CustomDebug.Log($"{this}-GameEvents_GameStarted");
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

        private void GameEvents_MatchingCard(IGridBoxCardItem firstSelectedCardOne, IGridBoxCardItem secondSelectedCard)
        {
            MatchesCount++;
            TurnCount++;
            CheckGameOver();
        }

        private void GameEvents_MismatchingCard()
        {
            IsComboActive = false;
            TurnCount++;
        }
    }
}