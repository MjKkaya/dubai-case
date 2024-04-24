using CardMatching.Events;
using UnityEngine;


namespace CardMatching.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private const float _comboPoint = 10;
        private const float _correctAnswerPoint = 5;

        [Tooltip("This is the minimum correct answers in a row to get Combo point")]
        [Range(2, 5)]
        [SerializeField] private int _minimumStreak = 2;

        private int _currentStreak;


        private void OnEnable()
        {
            GameEvents.GameStarting += GameEvents_GameStarting;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }

        private void OnDisable()
        {
            GameEvents.GameStarting -= GameEvents_GameStarting;
            GameEvents.MatchingCard -= GameEvents_MatchingCard;
            GameEvents.MismatchingCard -= GameEvents_MismatchingCard;
        }


        private void GameEvents_GameStarting()
        {
            _currentStreak = 0;
        }

        private void GameEvents_MatchingCard()
        {
            _currentStreak++;
            GameEvents.EarnedPoint?.Invoke(_correctAnswerPoint);
            if(_currentStreak > _minimumStreak)
            {
                GameEvents.EarnedComboPoint?.Invoke(_comboPoint);
            }
        }

        private void GameEvents_MismatchingCard()
        {
            _currentStreak = 0;
        }
    }
}