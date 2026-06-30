using CardMatching.Core.Events;
using CardMatching.Core.Interfaces;
using UnityEngine;
using TMPro;
using CardMatching.Core.ScriptableObjects;
using VContainer;


namespace CardMatching.UI.Panels
{
    public class StaticsPanel : MonoBehaviour
    {
        private float _currentScore;
        private float CurrentScore
        {
            set
            {
                _currentScore = value;
                _scoreText.text = _currentScore.ToString();
            }
        }

        private int _matchesCount;
        private int MatchesCount
        {
            set
            {
                _matchesCount = value;
                _matchesText.text = _matchesCount.ToString();
            }
        }

        private int _turnsCount;
        private int TurnsCount
        {
            set
            {
                _turnsCount = value;
                _turnsText.text = _turnsCount.ToString();
            }
        }

        [SerializeField] private TextMeshProUGUI _matchesText;
        [SerializeField] private TextMeshProUGUI _turnsText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        
        private GameEvents _gameEvents;
        
        [Inject]
        public void Construct(GameEvents gameEvents)
        {
            _gameEvents = gameEvents;
            
            _gameEvents.UnfinishedGameStarting += GameEvents_UnfinishedGameStarting;
            _gameEvents.NewGameStarting += GameEvents_NewGameStarting;
            _gameEvents.MatchingCard += GameEvents_MatchingCard;
            _gameEvents.MismatchingCard += GameEvents_MismatchingCard;
            _gameEvents.EarnedPoint += GameEvents_EarnedPoint;
            _gameEvents.EarnedComboPoint += GameEvents_EarnedComboPoint;
        }


        private void OnDestroy()
        {
            if(_gameEvents == null)
                return;
            
            _gameEvents.UnfinishedGameStarting -= GameEvents_UnfinishedGameStarting;
            _gameEvents.NewGameStarting -= GameEvents_NewGameStarting;
            _gameEvents.MatchingCard += GameEvents_MatchingCard;
            _gameEvents.MismatchingCard += GameEvents_MismatchingCard;
            _gameEvents.EarnedPoint -= GameEvents_EarnedPoint;
            _gameEvents.EarnedComboPoint -= GameEvents_EarnedComboPoint;
        }


        private void ResetDataAndText()
        {
            CurrentScore = 0;
            MatchesCount = 0;
            TurnsCount = 0;
        }


        private void GameEvents_UnfinishedGameStarting(CurrentGameDataSO currentGameDataSO)
        {
            CurrentScore = currentGameDataSO.Score;
            MatchesCount = currentGameDataSO.MatchesCount;
            TurnsCount = currentGameDataSO.TurnCount;
        }

        private void GameEvents_NewGameStarting()
        {
            ResetDataAndText();
        }

        private void GameEvents_MismatchingCard()
        {
            TurnsCount = ++_turnsCount;
        }

        private void GameEvents_MatchingCard(IGridBoxCardItem firstSelectedCardOne, IGridBoxCardItem secondSelectedCard)
        {
            MatchesCount = ++_matchesCount;
            TurnsCount = ++_turnsCount;
        }

        private void GameEvents_EarnedPoint(float point)
        {
            CurrentScore = _currentScore + point;
        }

        private void GameEvents_EarnedComboPoint(float point)
        {
            CurrentScore = _currentScore + point;
        }
    }
}