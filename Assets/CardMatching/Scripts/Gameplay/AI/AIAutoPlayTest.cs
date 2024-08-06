using System.Collections;
using System.Collections.Generic;
using CardMatching.Events;
using CardMatching.GridBox;
using CardMatching.Utilities;
using UnityEngine;


public static class AIAutoPlayTest
{
    //private static PointerEventData pointerEventData;
    private static List<GridBoxCardItem> _cardItemList = new ();
    private static GridBoxCardItem _selectedFirstCard;
    private static GridBoxCardItem _selectedSecondCard;
    private static Coroutine _startToOpenTwoCards;
    private static WaitForSeconds _firstWait = new(2f);
    private static WaitForSeconds _waitAfterFirstCard = new(0.5f);
    private static int _attempCount;



    public static void StartTest(GridBoxCardItem[,] _gridBoxCardItems)
    {
        _cardItemList.Clear();

        // access to CardItem object list - this for open card onthe screen
        //and
        //  access to CardItemData class list - this for open same card

        int gridAreaDimensionY = _gridBoxCardItems.GetLength(0); // for the number of rows
        int gridAreaDimensionX = _gridBoxCardItems.GetLength(1);  //  for the number of columns.

        CustomDebug.Log($"gridAreaDimensionX :{gridAreaDimensionX }, gridAreaDimensionY :{gridAreaDimensionY }");

        //GridBoxCardItem gridBoxCardItem = _gridBoxCardItems[gridAreaDimensionY-2, gridAreaDimensionX-2];
        //gridBoxCardItem.OnPointerClick(null);

        for (int y = 0; y < gridAreaDimensionY; y++)
        {
            for (int x = 0; x < gridAreaDimensionX; x++)
            {
                if(_gridBoxCardItems[y, x] != null)
                    _cardItemList.Add(_gridBoxCardItems[y, x]);
                else
                    CustomDebug.Log($"{y}:{x} is null");
                //gridBoxCardItem = _gridBoxCardItems[y, x];
            }
        }

        _attempCount = 0;
        OpenTwoCards();
    }


    private static void OpenTwoCards()
    {
        if(_cardItemList.Count > 1)
        {
            SetEventHandlers(true);

            if(_startToOpenTwoCards != null)
                CustomCoroutines.StopCoroutine(_startToOpenTwoCards);

            _startToOpenTwoCards = CustomCoroutines.StartCoroutine(StartToOpenTwoCards());
        }
        else
        {
            CustomDebug.Log($"OpenTwoCards-Card list count is {_cardItemList.Count}, _attempCount:{_attempCount}");
        }
    }

    private static IEnumerator StartToOpenTwoCards()
    {
        if(_attempCount == 0)
            yield return _firstWait;

        //CustomDebug.Log($"OpenTwoCards-Card list count is {_cardItemList.Count}, _attempCount:{_attempCount}");
        _attempCount++;

        int _selectedFirstCardIndexNo;

        do
        {
            _selectedFirstCardIndexNo = Random.Range(0, _cardItemList.Count);
            if (_cardItemList[_selectedFirstCardIndexNo].IsInteractable)
            {
                _selectedFirstCard = _cardItemList[_selectedFirstCardIndexNo];
                //CustomDebug.Log($"_selectedFirst: {_selectedFirstCard.name}, _selectedFirstIndexNo:{_selectedFirstCardIndexNo}");
                _selectedFirstCard.OnPointerClick(null);
            }
            else
                _selectedFirstCardIndexNo = -1;

        } while (_selectedFirstCardIndexNo == -1);

        yield return _waitAfterFirstCard;

        if(_cardItemList.Count < 5)
            yield return _waitAfterFirstCard;



        int _selectedSecondIndexNo;
        do
        {
            _selectedSecondIndexNo = Random.Range(0, _cardItemList.Count);
            if (_selectedFirstCardIndexNo != _selectedSecondIndexNo && _cardItemList[_selectedSecondIndexNo].IsInteractable)
            {
                _selectedSecondCard = _cardItemList[_selectedSecondIndexNo];
                //CustomDebug.Log($"_selectedSecond: {_selectedSecondCard.name}, _selectedSecondIndexNo:{_selectedSecondIndexNo}");
                _selectedSecondCard.OnPointerClick(null);
            }
            else
                _selectedSecondIndexNo = -1;

        } while (_selectedSecondIndexNo == -1);
    }


    private static void SetEventHandlers(bool isActive)
    {
        if(isActive)
        {
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }
        else
        {
            GameEvents.MatchingCard -= GameEvents_MatchingCard;
            GameEvents.MismatchingCard -= GameEvents_MismatchingCard;
        }

    }

    private static void GameEvents_MatchingCard(GridBoxCardItem firstSelectedCardOne, GridBoxCardItem secondSelectedCard)
    {
        SetEventHandlers(false);
        //CustomDebug.Log($"GameEvents_MatchingCard-_cardItemList: {_cardItemList.Count}, _attempCount:{_attempCount}");
        _cardItemList.Remove(_selectedFirstCard);
        _cardItemList.Remove(_selectedSecondCard);
        //CustomDebug.Log($"GameEvents_MatchingCard-_cardItemList: {_cardItemList.Count}");
        OpenTwoCards();
    }

    private static void GameEvents_MismatchingCard()
    {
        //CustomDebug.Log($"GameEvents_MismatchingCard-_cardItemList: {_cardItemList.Count}, _attempCount:{_attempCount}");
        SetEventHandlers(false);
        OpenTwoCards();
    }
}