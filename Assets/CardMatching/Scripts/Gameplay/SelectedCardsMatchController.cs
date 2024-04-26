using System.Collections.Generic;
using CardMatching.Events;
using CardMatching.GridBox;
using UnityEngine;


namespace CardMatching.Gameplay
{
    public class SelectedCardsMatchController : MonoBehaviour
    {
        private List<SelectedPairCard> _selectedPairCardList;


        void Awake()
        {
            _selectedPairCardList = new();
        }

        public void AddSelectedCardItem(GridBoxCardItem gridBoxCardItem)
        {
            SelectedPairCard selectedPairCard = GetEmptySelectedCardPaid();
            selectedPairCard.AddCard(gridBoxCardItem);
            CeckMatch(selectedPairCard);
        }


        private SelectedPairCard GetEmptySelectedCardPaid()
        {
            SelectedPairCard selectedPairCard;

            int count = _selectedPairCardList.Count;
            for (int i = 0; i < count; i++)
            {
                selectedPairCard = _selectedPairCardList[i];
                if (!selectedPairCard.IsFull())
                    return selectedPairCard;
            }

            selectedPairCard = new SelectedPairCard();
            _selectedPairCardList.Add(selectedPairCard);

            return selectedPairCard;
        }

        private void CeckMatch(SelectedPairCard selectedPairCard)
        {
            if (selectedPairCard.IsFull())
            {
                if(selectedPairCard.IsMatch())
                    GameEvents.MatchingCard?.Invoke(selectedPairCard.GetCardList());
                else
                    GameEvents.MismatchingCard?.Invoke(selectedPairCard.GetCardList());

                selectedPairCard.Clear();
            }
        }
    }


    public class SelectedPairCard
    {
        private GridBoxCardItem _firstSelectedItem;
        private GridBoxCardItem _secondSelectedItem;


        public void AddCard(GridBoxCardItem selectedItemData)
        {
            if (_firstSelectedItem == null)
                _firstSelectedItem = selectedItemData;
            else if (_secondSelectedItem == null)
                _secondSelectedItem = selectedItemData;
            else
                Debug.LogWarning($"{this}-AddCard: Item is full!!!");
        }

        public void Clear()
        {
            Debug.Log($"{this}-Clear:{_firstSelectedItem.name} / {_secondSelectedItem.name}");
            _firstSelectedItem = null;
            _secondSelectedItem = null;
        }


        public bool IsFull()
        {
            return _firstSelectedItem != null && _secondSelectedItem != null;
        }

        public bool IsMatch()
        {
            return _firstSelectedItem.CardIconIndex == _secondSelectedItem.CardIconIndex;
        }

        public List<GridBoxCardItem> GetCardList()
        {
            return new List<GridBoxCardItem>() { _firstSelectedItem, _secondSelectedItem };
        }
    }
}