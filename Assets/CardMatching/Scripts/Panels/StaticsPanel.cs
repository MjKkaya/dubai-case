using CardMatching.Events;
using UnityEngine;
using TMPro;
using System;

namespace CardMatching.Panels
{
    public class StaticsPanel : MonoBehaviour
    {
        private int _matchesCount;
        private int _turnsCount;

        [SerializeField] private TextMeshProUGUI _matchesText;
        [SerializeField] private TextMeshProUGUI _turnsText;


        private void OnEnable()
        {
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }

        private void OnDisable()
        {
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }


        private void ResetTexts()
        {
            _matchesCount = 0;
            _turnsCount = 0;
            _matchesText.text = string.Empty;
            _turnsText.text = string.Empty;
        }


        private void GameEvents_MismatchingCard()
        {
            _turnsCount++;
            _turnsText.text = _turnsCount.ToString();
        }

        private void GameEvents_MatchingCard()
        {
            _matchesCount++;
            _turnsCount++;
            _matchesText.text = _matchesCount.ToString();
            _turnsText.text = _turnsCount.ToString();
        }
    }
}