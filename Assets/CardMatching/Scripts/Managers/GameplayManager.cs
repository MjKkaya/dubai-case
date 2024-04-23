using CardMatching.Events;
using UnityEngine;


namespace CardMatching.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        private const int _emptyIndexNumber = -1;

        private int _firstSelectedCardIconIndex;
        private int _secondSelectedCardIconIndex;

        private int _matchesCount;
        private int _turnsCount;


        private void OnEnable()
        {
            ResetOpenedIndexs();
            GameEvents.FlippingCard += GameEvents_FlippingCard;
        }

        private void OnDisable()
        {
            GameEvents.FlippingCard -= GameEvents_FlippingCard;
        }


        private void CheckSelectedCardResult()
        {
            Debug.Log($"{this}-CheckSelectedCardResult-selectedIndex:{_firstSelectedCardIconIndex}/{_secondSelectedCardIconIndex}");
            if(_firstSelectedCardIconIndex == _secondSelectedCardIconIndex)
            {
                _matchesCount = 0;
                GameEvents.MatchingCard?.Invoke();
            }
            else
                GameEvents.MismatchingCard?.Invoke();

            _turnsCount++;
            ResetOpenedIndexs();
        }


        private void ResetOpenedIndexs()
        {
            _firstSelectedCardIconIndex = _emptyIndexNumber;
            _secondSelectedCardIconIndex = _emptyIndexNumber;
        }

        private void ResetGameStats()
        {
            _matchesCount = 0;
            _turnsCount = 0;
        }


        private void SetSelectedCardIconIndexs(int openedCardIconIndex)
        {
            if (_firstSelectedCardIconIndex == _emptyIndexNumber)
                _firstSelectedCardIconIndex = openedCardIconIndex;
            else
                _secondSelectedCardIconIndex = openedCardIconIndex;
        }

        

        private void GameEvents_FlippingCard(int openedCardIconIndex)
        {
            SetSelectedCardIconIndexs(openedCardIconIndex);
            if (_secondSelectedCardIconIndex != _emptyIndexNumber)
                Invoke(nameof(CheckSelectedCardResult), 1f);
        }
    }
}