using CardMatching.Datas;
using CardMatching.Events;
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
            GameEvents.CardFlipped += CardFlipped;
            GameEvents.MatchingCard += GameEvents_MatchingCard;
            GameEvents.MismatchingCard += GameEvents_MismatchingCard;
            GameEvents.GameOver += GameEvents_GameOver;
        }

        private void OnDisable()
        {
            GameEvents.CardFlipped -= CardFlipped;
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
        private void CardFlipped()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.FlippingCardSound);
        }

        // Play the matching card sound effect
        private void GameEvents_MatchingCard()
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
