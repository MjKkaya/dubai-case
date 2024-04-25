using CardMatching.Events;
using UnityEngine;
using TMPro;
using CardMatching.ScriptableObjects;
using CardMatching.GridBox;
using System.Collections.Generic;

namespace CardMatching.Panels
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


        private void OnEnable()
        {
            GameEvents.StartGameWithUnfinishedGameData += GameEvents_StartGameWithUnfinishedGameData;
            GameEvents.GameStarting += GameEvents_GameStarting;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
            GameEvents.EarnedPoint += GameEvents_EarnedPoint;
            GameEvents.EarnedComboPoint += GameEvents_EarnedComboPoint;
        }

        private void OnDisable()
        {
            GameEvents.StartGameWithUnfinishedGameData -= GameEvents_StartGameWithUnfinishedGameData;
            GameEvents.GameStarting -= GameEvents_GameStarting;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
            GameEvents.EarnedPoint -= GameEvents_EarnedPoint;
            GameEvents.EarnedComboPoint -= GameEvents_EarnedComboPoint;
        }


        private void ResetDataAndText()
        {
            CurrentScore = 0;
            MatchesCount = 0;
            TurnsCount = 0;
        }


        private void GameEvents_StartGameWithUnfinishedGameData(CurrentGameDataSO currentGameDataSO)
        {
            CurrentScore = currentGameDataSO.Score;
            MatchesCount = currentGameDataSO.MatchesCount;
            TurnsCount = currentGameDataSO.TurnCount;
        }

        private void GameEvents_GameStarting()
        {
            ResetDataAndText();
        }

        private void GameEvents_MismatchingCard(List<GridBoxCardItem> cardList)
        {
            TurnsCount = ++_turnsCount;
        }

        private void GameEvents_MatchingCard(List<GridBoxCardItem> cardList)
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