using CardMatching.Events;
using UnityEngine;
using TMPro;


namespace CardMatching.Panels
{
    public class StaticsPanel : MonoBehaviour
    {
        private const string _defaultText = "0";

        private int _matchesCount;
        private int _turnsCount;

        [SerializeField] private TextMeshProUGUI _matchesText;
        [SerializeField] private TextMeshProUGUI _turnsText;


        private void OnEnable()
        {
            GameEvents.GameStarting += GameEvents_GameStarting;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }

        private void OnDisable()
        {
            GameEvents.GameStarting -= GameEvents_GameStarting;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
        }


        private void ResetDataAndText()
        {
            _matchesCount = 0;
            _turnsCount = 0;
            _matchesText.text = _defaultText;
            _turnsText.text = _defaultText;
        }


        private void GameEvents_GameStarting()
        {
            ResetDataAndText();
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