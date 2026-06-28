using CardMatching.Core.Events;
using CardMatching.Core.Interfaces;
using CardMatching.Gameplay.GridBox;
using CardMatching.Utilities;
using UnityEngine;

namespace CardMatching.Gameplay
{
    public class CardMatchCommand : ICommand
    {
        private GridBoxCardItem _firstSelectedItem;
        private GridBoxCardItem _secondSelectedItem;


        public void AddCard(IGridBoxCardItem selectedItemData)
        {
            if (_firstSelectedItem == null)
                _firstSelectedItem = (GridBoxCardItem)selectedItemData;
            else if (_secondSelectedItem == null)
                _secondSelectedItem = (GridBoxCardItem)selectedItemData;
            else
                Debug.LogWarning($"{this}-AddCard: Item is full!!!");
        }

        public void Clear()
        {
            CustomDebug.Log($"{this}-Clear:{_firstSelectedItem.name} / {_secondSelectedItem.name}");
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

        public void Execute()
        {
            if (IsMatch())
            {
                GameEvents.MatchingCard?.Invoke(_firstSelectedItem, _secondSelectedItem);
            }
            else
            {
                _firstSelectedItem.StartFlipAniamtion();
                _secondSelectedItem.StartFlipAniamtion();
                GameEvents.MismatchingCard?.Invoke();
            }
        }

        public void Undo()
        {
        }
    }
}