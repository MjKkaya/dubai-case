using System.Collections;
using System.Collections.Generic;
using CardMatching.Datas;
using CardMatching.Events;
using CardMatching.ScriptableObjects;
using CardMatching.Utilities;
using UnityEngine;


namespace CardMatching.GridBox
{
    [RequireComponent(typeof(GridBoxItemFactory))]
    public class GridBoxController : MonoBehaviour
    {
        private const int _minDimentionValue = 2;
        private const int _maxnDimentionValue = 6;

        private readonly WaitForSeconds _waitForFirstTimeCardShowing = new (1f);

        [SerializeField] private GridBoxItemFactory _gridBoxFactory;
        [SerializeField] private RectTransform _cardItemContainer;

        private GridBoxCardItem[,] _gridBoxCardItems;

        private GridDimension _currentLevelGridAreaDimension;


        private void OnEnable()
        {
            GameEvents.StartGameWithUnfinishedGameData += GameEvents_StartGameWithUnfinishedGameData;
            GameEvents.GameStarting+= GameEvents_GameStarting;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }

        private void OnDisable()
        {
            GameEvents.StartGameWithUnfinishedGameData -= GameEvents_StartGameWithUnfinishedGameData;
            GameEvents.GameStarting -= GameEvents_GameStarting;
            GameEvents.MatchingCard -= GameEvents_MatchingCard;
            GameEvents.MismatchingCard -= GameEvents_MismatchingCard;
        }


        private void StartGameWithUnfinishedGameData(CurrentGameDataSO currentGameDataSO)
        {
            _currentLevelGridAreaDimension = new GridDimension(currentGameDataSO.GridAreaDimensionX, currentGameDataSO.GridAreaDimensionY);
            List<GridBoxCardData> _cardDataList = new(_gridBoxFactory.CreateCardDataListWithCurrentData(currentGameDataSO.IconIndexArray));
            CreateGridBoxItems(_cardDataList);
            GameEvents.GameStarted?.Invoke(_currentLevelGridAreaDimension, _gridBoxCardItems);
            SetCardItemInteracable(true);
            //It was closed because player can abuse this situation.
            //StartCoroutine(ShowAllCardforShortTime()); 
        }

        private void CreateNewLevel()
        {
            _currentLevelGridAreaDimension = CreateRandomGridDimension();
            int pairCount = _currentLevelGridAreaDimension.X * _currentLevelGridAreaDimension.Y / 2;
            List<GridBoxCardData> _cardDataList = CreateCardDataList(pairCount);
            CreateGridBoxItems(_cardDataList);
            GameEvents.GameStarted?.Invoke(_currentLevelGridAreaDimension, _gridBoxCardItems);
            StartCoroutine(ShowAllCardforShortTime());
        }

        private GridDimension CreateRandomGridDimension()
        {
            int sideX = Random.Range(_minDimentionValue, _maxnDimentionValue);
            int sideY = Random.Range(_minDimentionValue, _maxnDimentionValue);

            //prevent odd number of total cards
            if (sideX%2 != 0 && sideY % 2 != 0)
            {
                if (sideX % 2 != 0)
                    sideX--;
                else
                    sideY--;
            }

            return new GridDimension(sideX, sideY);
        }


        private List<GridBoxCardData> CreateCardDataList(int pairCount)
        {
            List<GridBoxCardData> _cardDataList = new(_gridBoxFactory.CreateCardDataListForCurrentLevel(pairCount));
            _cardDataList.AddRange(_cardDataList);
            _cardDataList.Shuffle();
            return _cardDataList;
        }

        private void CreateGridBoxItems(List<GridBoxCardData> _cardDataList)
        {
            Vector2 gapForCardItem = CalculateGapBetweenCardItems(0.1f);
            Vector2 cardItemNewSize = CalculateCardItemSize(gapForCardItem);
            Vector2 cardItemHalfSize = cardItemNewSize * 0.5f;

            _gridBoxCardItems = new GridBoxCardItem[_currentLevelGridAreaDimension.Y, _currentLevelGridAreaDimension.X];

            int indexNo = 0;
            Vector2 cardItemPosition;
            // Item's pivot point is 0.5
            Vector2 startingPositionOfItemConteiner = (new Vector2(_cardItemContainer.rect.width, _cardItemContainer.rect.height) * -0.5f) + cardItemHalfSize + gapForCardItem;
            GridBoxCardItem gridBoxCardItem;


            for (int y = 0; y < _currentLevelGridAreaDimension.Y; y++)
            {
                for (int x = 0; x < _currentLevelGridAreaDimension.X; x++)
                {
                    //don't create opened card. This control for continue game!
                    if (_cardDataList[indexNo].CardIconIndex == CurrentGameDataSO.EmptyBoxIconIndex)
                    {
                        indexNo++;
                        continue;
                    }

                    cardItemPosition = startingPositionOfItemConteiner;
                    cardItemPosition.x += (cardItemNewSize.x + gapForCardItem.x) * x;
                    cardItemPosition.y += (cardItemNewSize.y + gapForCardItem.y) * y;

                    gridBoxCardItem = _gridBoxFactory.GetGridBoxItem(_cardDataList[indexNo], new GridDimension(x, y));
                    gridBoxCardItem.transform.SetParent(_cardItemContainer, false);
                    gridBoxCardItem.RectTransform.sizeDelta = cardItemNewSize;
                    gridBoxCardItem.RectTransform.anchoredPosition = cardItemPosition;

                    _gridBoxCardItems[y, x] = gridBoxCardItem;

                    Debug.Log($"{this}-indexNo:{indexNo}, data:{_cardDataList[indexNo]}, name:{gridBoxCardItem.name}");
                    indexNo++;
                }
            }
        }

        IEnumerator ShowAllCardforShortTime()
        {
            GridBoxCardItem gridBoxItem;
            for (int y = 0; y < _currentLevelGridAreaDimension.Y; y++)
            {
                for (int x = 0; x < _currentLevelGridAreaDimension.X; x++)
                {
                    gridBoxItem = _gridBoxCardItems[y, x];
                    if (gridBoxItem != null && gridBoxItem.gameObject.activeInHierarchy)
                        gridBoxItem.SetDisplayImageSprite(true);
                }
            }

            yield return _waitForFirstTimeCardShowing;

            for (int y = 0; y < _currentLevelGridAreaDimension.Y; y++)
            {
                for (int x = 0; x < _currentLevelGridAreaDimension.X; x++)
                {
                    gridBoxItem = _gridBoxCardItems[y, x];
                    if (gridBoxItem != null && gridBoxItem.gameObject.activeInHierarchy)
                    {
                        gridBoxItem.SetDisplayImageSprite(false);
                        gridBoxItem.SetInteractible(true);
                    }
                }
            }
        }


        private Vector2 CalculateGapBetweenCardItems(float gapRation)
        {
            Vector2 gap;

            float gapX = _cardItemContainer.rect.width / _currentLevelGridAreaDimension.X;
            float gapY = _cardItemContainer.rect.height/ _currentLevelGridAreaDimension.Y;
            gapX *= gapRation;
            gapY *= gapRation;

            gap = new Vector2(gapX, gapY);

            Debug.Log($"{this}-CalculateGapBetweenCardItems-_cardItemContainer:{_cardItemContainer.rect.width}/{_cardItemContainer.rect.height} gap:{gap}");
            return gap;
        }

        private Vector2 CalculateCardItemSize(Vector2 gap)
        {
            Vector2 newSize;

            float gapRemovedWidth = _cardItemContainer.rect.width - (gap.x * (_currentLevelGridAreaDimension.X +1));
            float gapRemovedHeight = _cardItemContainer.rect.height - (gap.y * (_currentLevelGridAreaDimension.Y +1));

            float itemNewSizeX = gapRemovedWidth / _currentLevelGridAreaDimension.X;
            float itemNewSizeY = gapRemovedHeight / _currentLevelGridAreaDimension.Y;

            newSize = new Vector2(itemNewSizeX, itemNewSizeY);
            Debug.Log($"{this}-CalculateCardItemSize-newSize: {newSize}");

            return newSize;
        }

        private void SetCardItemInteracable(bool isActive)
        {
            if (_gridBoxCardItems == null || _gridBoxCardItems.Length == 0)
                return;

            GridBoxCardItem gridBoxItem;
            for (int y = 0; y < _currentLevelGridAreaDimension.Y; y++)
            {
                for (int x = 0; x < _currentLevelGridAreaDimension.X; x++)
                {
                    gridBoxItem = _gridBoxCardItems[y, x];
                    if (gridBoxItem != null && gridBoxItem.gameObject.activeInHierarchy)
                        gridBoxItem.SetInteractible(isActive);
                }
            }
        }

        private void FlipBackSelectedCards(List<GridBoxCardItem> selectedCardItems)
        {
            foreach (GridBoxCardItem cardItem in selectedCardItems)
            {
                cardItem.StartFlipAniamtion();
            }
        }

        private void DisappearMatchedCards(List<GridBoxCardItem> selectedCardItems)
        {
            foreach (GridBoxCardItem cardItem in selectedCardItems)
            {
                _gridBoxFactory.ReleaseGridBoxItem(cardItem);
            }
        }


        #region Game Events

        private void GameEvents_StartGameWithUnfinishedGameData(CurrentGameDataSO currentGameDataSO)
        {
            StartGameWithUnfinishedGameData(currentGameDataSO);
        }

        private void GameEvents_GameStarting()
        {
            CreateNewLevel();
        }

        private void GameEvents_MatchingCard(List<GridBoxCardItem> cardList)
        {
            DisappearMatchedCards(cardList);
        }

        private void GameEvents_MismatchingCard(List<GridBoxCardItem> cardList)
        {
            FlipBackSelectedCards(cardList);
        }

        #endregion
    }
}