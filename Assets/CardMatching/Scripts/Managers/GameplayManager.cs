using CardMatching.Events;
using CardMatching.GridBox;
using UnityEngine;


namespace CardMatching.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        private const int _emptyIndexNumber = -1;

        private int _firstSelectedCardIconIndex;
        private int _secondSelectedCardIconIndex;

        private int _pairCount;
        private int _matchesCount;
        private int _turnsCount;


        private void OnEnable()
        {
            ResetOpenedIndexs();
            GameEvents.CardSelected += GameEvents_CardSelected;
            GameEvents.GameStarted += GameEvents_GameStarted;
        }

        private void OnDisable()
        {
            GameEvents.CardSelected -= GameEvents_CardSelected;
            GameEvents.GameStarted -= GameEvents_GameStarted;
        }


        private void CheckSelectedCardResult()
        {
            Debug.Log($"{this}-CheckSelectedCardResult-selectedIndex:{_firstSelectedCardIconIndex}/{_secondSelectedCardIconIndex}");
            if(_firstSelectedCardIconIndex == _secondSelectedCardIconIndex)
            {
                GameEvents.MatchingCard?.Invoke();
                Invoke(nameof(CheckGameOver), 1f);
            }
            else
                GameEvents.MismatchingCard?.Invoke();

            _turnsCount++;
            ResetOpenedIndexs();
        }

        private void CheckGameOver()
        {
            _matchesCount++;
            if (_matchesCount == _pairCount)
            {
                GameEvents.GameOver?.Invoke();
                ResetGameStats();
            }
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

        private void GameEvents_CardSelected(GridBoxCardItem gridBoxCardItem)
        {
            SetSelectedCardIconIndexs(gridBoxCardItem.CardIconIndex);
            if (_secondSelectedCardIconIndex != _emptyIndexNumber)
            {
                GameEvents.StartedCardMatchingControl?.Invoke();
                Invoke(nameof(CheckSelectedCardResult), 1f);
            }
        }

        private void GameEvents_GameStarted(int pairCount)
        {
            _pairCount = pairCount;
        }
    }
}