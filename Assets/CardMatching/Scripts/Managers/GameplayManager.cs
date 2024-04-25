using CardMatching.Events;
using CardMatching.GridBox;
using CardMatching.ScriptableObjects;
using UnityEngine;


namespace CardMatching.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        private const int _emptyIndexNumber = -1;

        [SerializeField] private CurrentGameDataSO currentGameData;

        private int _firstSelectedCardIconIndex;
        private int _secondSelectedCardIconIndex;


        private void OnEnable()
        {
            ResetOpenedIndexs();
            GameEvents.CardSelected += GameEvents_CardSelected;
        }

        private void OnDisable()
        {
            GameEvents.CardSelected -= GameEvents_CardSelected;
        }


        private void CheckSelectedCardResult()
        {
            Debug.Log($"{this}-CheckSelectedCardResult-selectedIndex:{_firstSelectedCardIconIndex}/{_secondSelectedCardIconIndex}");
            if(_firstSelectedCardIconIndex == _secondSelectedCardIconIndex)
            {
                GameEvents.MatchingCard?.Invoke();
                Invoke(nameof(CheckGameOver), 0.5f);

            }
            else
                GameEvents.MismatchingCard?.Invoke();

            ResetOpenedIndexs();
        }

        private void CheckGameOver()
        {
            if (currentGameData.MatchesCount == currentGameData.PairCount)
                GameEvents.GameOver?.Invoke();
        }

        private void ResetOpenedIndexs()
        {
            _firstSelectedCardIconIndex = _emptyIndexNumber;
            _secondSelectedCardIconIndex = _emptyIndexNumber;
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
                Invoke(nameof(CheckSelectedCardResult), 0.75f);
            }
        }
    }
}