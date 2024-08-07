using CardMatching.Events;
using CardMatching.GridBox;
using CardMatching.Managers;
using UnityEngine;


namespace CardMatching.Gameplay
{
    [RequireComponent(typeof(AudioManager))]
    public class GameplaySounds : MonoBehaviour
    {
        [Tooltip("Required AudioManager component")]
        [SerializeField] AudioManager _audioManager;


        private void OnEnable()
        {
            GameEvents.CardSelected += GameEvents_CardSelected;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
            GameEvents.GameOver += GameEvents_GameOver;
        }

        private void OnDisable()
        {
            GameEvents.CardSelected -= GameEvents_CardSelected;
            GameEvents.MatchingCard -= GameEvents_MatchingCard;
            GameEvents.MismatchingCard -= GameEvents_MismatchingCard;
            GameEvents.GameOver -= GameEvents_GameOver;
        }

        private void Start()
        {
            if (_audioManager == null)
                _audioManager = GetComponent<AudioManager>();
        }


        // Play the flipping card sound effect
        private void GameEvents_CardSelected()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.FlippingCardSound);
        }

        // Play the matching card sound effect
        private void GameEvents_MatchingCard(GridBoxCardItem firstSelectedCardOne, GridBoxCardItem secondSelectedCard)
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.MatchingCardSound);
        }

        // Play the mismatching card sound effect
        private void GameEvents_MismatchingCard()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.MismatchingCardSound);
        }

        // Play the game over sound effect
        private void GameEvents_GameOver()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.GameOverSound);
        }
    }
}
