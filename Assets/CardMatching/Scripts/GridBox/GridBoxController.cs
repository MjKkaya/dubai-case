using System.Collections;
using System.Collections.Generic;
using CardMatching.Datas;
using CardMatching.Events;
using CardMatching.Utilities;
using UnityEngine;


namespace CardMatching.GridBox
{
    [RequireComponent(typeof(GridBoxItemFactory))]
    public class GridBoxController : MonoBehaviour
    {
        [SerializeField] private GridBoxItemFactory _gridBoxFactory;
        [SerializeField] private RectTransform _cardItemContainer;

        private Coroutine _coroutineCreateLevel;
        private GridBoxCardItem[,] _gridBoxCardItems;
        private List<GridBoxCardItem> _selectedCardItems = new ();

        private GridDimension _currentLevelGridAreaDimension;


        private void Start()
        {
            CreateLevel(new GridDimension(2, 4));
        }

        private void OnEnable()
        {
            GameEvents.CardSelected += GameEvents_CardSelected;
            GameEvents.StartedCardMatchingControl += GameEvents_StartedCardMatchingControl;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }

        private void OnDisable()
        {
            GameEvents.CardSelected -= GameEvents_CardSelected;
            GameEvents.StartedCardMatchingControl -= GameEvents_StartedCardMatchingControl;
            GameEvents.MatchingCard -= GameEvents_MatchingCard;
            GameEvents.MismatchingCard -= GameEvents_MismatchingCard;
        }


        public void CreateLevel(GridDimension gridAreaDimension)
        {
            _currentLevelGridAreaDimension = gridAreaDimension;
            if (_coroutineCreateLevel != null)
                StopCoroutine(_coroutineCreateLevel);
            _coroutineCreateLevel = StartCoroutine(ICreateLevel());
        }

        IEnumerator ICreateLevel()
        {
            CreateGridBoxItems();
            yield return null;
        }
        
        private void CreateGridBoxItems()
        {
            Vector2 gapForCardItem = CalculateGapBetweenCardItems(0.1f);
            Vector2 cardItemNewSize = CalculateCardItemSize(gapForCardItem);
            Vector2 cardItemHalfSize = cardItemNewSize * 0.5f;

            _gridBoxCardItems = new GridBoxCardItem[_currentLevelGridAreaDimension.Y, _currentLevelGridAreaDimension.X];

            int pairCount = _currentLevelGridAreaDimension.X * _currentLevelGridAreaDimension.Y / 2;
            List<GridBoxCardData> _cardDataList = new List<GridBoxCardData>(_gridBoxFactory.CreateCardDataListForCurrentLevel(pairCount));
            _cardDataList.AddRange(_cardDataList);
            _cardDataList.Shuffle();

            int indexNo = 0;
            Vector2 cardItemPosition;
            // Item's pivot point is 0.5
            Vector2 startingPositionOfItemConteiner = (new Vector2(_cardItemContainer.rect.width, _cardItemContainer.rect.height) * -0.5f) + cardItemHalfSize + gapForCardItem;
            GridBoxCardItem gridBoxCardItem;


            for (int y = 0; y < _currentLevelGridAreaDimension.Y; y++)
            {
                for (int x = 0; x < _currentLevelGridAreaDimension.X; x++)
                {
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

        private void FlipBackSelectedCards()
        {
            foreach (GridBoxCardItem cardItem in _selectedCardItems)
            {
                cardItem.StartFlipAniamtion();
            }
            _selectedCardItems.Clear();
        }

        private void DisappearSelectedCards()
        {
            foreach (GridBoxCardItem cardItem in _selectedCardItems)
            {
                _gridBoxFactory.ReleaseGridBoxItem(cardItem);
            }
            _selectedCardItems.Clear();
        }


        private void GameEvents_CardSelected(GridBoxCardItem selectedCardItem)
        {
            _selectedCardItems.Add(selectedCardItem);
            selectedCardItem.StartFlipAniamtion();
            GameEvents.CardFlipped?.Invoke();
        }

        private void GameEvents_StartedCardMatchingControl()
        {
            SetCardItemInteracable(false);
        }

        private void GameEvents_MatchingCard()
        {
            DisappearSelectedCards();
            SetCardItemInteracable(true);
        }

        private void GameEvents_MismatchingCard()
        {
            FlipBackSelectedCards();
            SetCardItemInteracable(true);
        }
    }
}