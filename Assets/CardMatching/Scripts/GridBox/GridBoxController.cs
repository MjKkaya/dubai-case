using CardMatching.Datas;
using CardMatching.Events;
using CardMatching.ScriptableObjects;
using CardMatching.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CardMatching.GridBox
{
    [RequireComponent(typeof(GridBoxItemFactory))]
    public class GridBoxController : MonoBehaviour
    {
        private const int _minDimentionValue = 4;
        private const int _maxDimentionValue = 4;

        private readonly WaitForSeconds _waitForFirstTimeCardShowing = new (1f);

        [SerializeField] private GridBoxItemFactory _gridBoxFactory;
        [SerializeField] private GridBoxItemDataFactory _gridBoxItemDataFactory;
        [SerializeField] private RectTransform _cardItemContainer;

        //We create max distance because prevent to memory fragment
        //readonlykeywords only prevent to assign new istance to the variable!
        private readonly GridBoxCardItem[,] _gridBoxCardItems = new GridBoxCardItem[_maxDimentionValue, _maxDimentionValue];
        private readonly List<GridBoxCardData> _gridBoxCardItemDataList = new();

        private GridDimension _currentLevelGridAreaDimension;


        private void OnEnable()
        {
            GameEvents.UnfinishedGameStarting += GameEvents_UnfinishedGameStarting;
            GameEvents.NewGameStarting+= GameEvents_GameStarting;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            //GameEvents.MismatchingCard += GameEvents_MismatchingCard;
            GameEvents.GameOver += GameEvents_GameOver;
        }

        private void OnDisable()
        {
            GameEvents.UnfinishedGameStarting -= GameEvents_UnfinishedGameStarting;
            GameEvents.NewGameStarting -= GameEvents_GameStarting;
            GameEvents.MatchingCard -= GameEvents_MatchingCard;
            //GameEvents.MismatchingCard -= GameEvents_MismatchingCard;
            GameEvents.GameOver -= GameEvents_GameOver;
        }


        private void StartGameWithUnfinishedGameData(CurrentGameDataSO currentGameDataSO)
        {
            _currentLevelGridAreaDimension = new GridDimension(currentGameDataSO.GridAreaDimensionX, currentGameDataSO.GridAreaDimensionY);
            _gridBoxItemDataFactory.CreateCardDataListWithCurrentData(_gridBoxCardItemDataList, currentGameDataSO.IconIndexArray);
            CreateGridBoxItems(_gridBoxCardItemDataList);
            GameEvents.GameStarted?.Invoke(_currentLevelGridAreaDimension, _gridBoxCardItems);
            SetCardItemInteracable(true);
            //StartCoroutine(ShowAllCardforShortTime()); //It was closed because player can abuse this situation.
        }

        private void CreateNewLevel()
        {
            _currentLevelGridAreaDimension = CreateRandomGridDimension();
            int pairCount = _currentLevelGridAreaDimension.X * _currentLevelGridAreaDimension.Y / 2;
            FillGridBoxCardItemDataList(pairCount);
            CreateGridBoxItems(_gridBoxCardItemDataList);
            GameEvents.GameStarted?.Invoke(_currentLevelGridAreaDimension, _gridBoxCardItems);
            StartCoroutine(ShowAllCardforShortTime());
            //AIAutoPlayTest.StartTest(_gridBoxCardItems);
        }

        private GridDimension CreateRandomGridDimension()
        {
            int sideX = Random.Range(_minDimentionValue, _maxDimentionValue+1);
            int sideY = Random.Range(_minDimentionValue, _maxDimentionValue+1);
            CustomDebug.Log($"{this}-sideX:{sideX}, sideY:{sideY}");
            //prevent odd number of total cards
            if (sideX%2 != 0 && sideY % 2 != 0)
            {
                if (sideX % 2 != 0)
                    sideX--;
                else
                    sideY--;
            }

            CustomDebug.Log($"{this}-sideX:{sideX}, sideY:{sideY}");
            return new GridDimension(sideX, sideY);
        }


        private void FillGridBoxCardItemDataList(int pairCount)
        {
            _gridBoxCardItemDataList.Clear();
            _gridBoxItemDataFactory.CreateCardDataListForCurrentLevel(_gridBoxCardItemDataList, pairCount);
            _gridBoxCardItemDataList.AddRange(_gridBoxCardItemDataList);
            _gridBoxCardItemDataList.Shuffle();
        }

        private void CreateGridBoxItems(List<GridBoxCardData> _cardDataList)
        {
            Vector2 gapForCardItem = CalculateGapBetweenCardItems(0.1f);
            Vector2 cardItemNewSize = CalculateCardItemSize(gapForCardItem);
            Vector2 cardItemHalfSize = cardItemNewSize * 0.5f;

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
                    if (_cardDataList[indexNo].CardIconIndex == GridBoxCardData.EmptyIndexNo)
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

                    CustomDebug.Log($"{this}-indexNo:{indexNo}, data:{_cardDataList[indexNo]}, name:{gridBoxCardItem.name}");
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
                        gridBoxItem.SetInteraction(true);
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

            CustomDebug.Log($"{this}-CalculateGapBetweenCardItems-_cardItemContainer:{_cardItemContainer.rect.width}/{_cardItemContainer.rect.height} gap:{gap}");
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
            CustomDebug.Log($"{this}-CalculateCardItemSize-newSize: {newSize}");

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
                        gridBoxItem.SetInteraction(isActive);
                }
            }
        }

        //private void FlipBackSelectedCards(List<GridBoxCardItem> selectedCardItems)
        //{
        //    foreach (GridBoxCardItem cardItem in selectedCardItems)
        //    {
        //        cardItem.StartFlipAniamtion();
        //    }
        //}

        private void DisappearMatchedCards(GridBoxCardItem firstSelectedCardOne, GridBoxCardItem secondSelectedCard)
        {
            _gridBoxFactory.ReleaseGridBoxItem(firstSelectedCardOne);
            _gridBoxFactory.ReleaseGridBoxItem(secondSelectedCard);
            //foreach (GridBoxCardItem cardItem in selectedCardItems)
            //{
            //    _gridBoxFactory.ReleaseGridBoxItem(cardItem);
            //}
        }

        private void ReleaseGridBoxCardItemDataList()
        {
            foreach (GridBoxCardData data in _gridBoxCardItemDataList)
            {
                //We must to check this control because this is twin list, ex: if we have 6 card in the list, that means; 3 different card data and each data class duplicated.
                if(data.CardIconIndex != GridBoxCardData.EmptyIndexNo)
                    _gridBoxItemDataFactory.ReleaseGridBoxItemData(data);
            }
        }


        #region Game Events

        private void GameEvents_UnfinishedGameStarting(CurrentGameDataSO currentGameDataSO)
        {
            StartGameWithUnfinishedGameData(currentGameDataSO);
        }

        private void GameEvents_GameStarting()
        {
            CreateNewLevel();
        }

        private void GameEvents_MatchingCard(GridBoxCardItem firstSelectedCardOne, GridBoxCardItem secondSelectedCard)
        {
            DisappearMatchedCards(firstSelectedCardOne, secondSelectedCard);
        }

        //private void GameEvents_MismatchingCard(List<GridBoxCardItem> cardList)
        //{
        //    FlipBackSelectedCards(cardList);
        //}

        private void GameEvents_GameOver()
        {
            ReleaseGridBoxCardItemDataList();
        }

        #endregion
    }
}