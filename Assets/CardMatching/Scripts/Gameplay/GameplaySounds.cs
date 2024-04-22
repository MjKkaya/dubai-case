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
            UIEvents.FlippingCard += PlayFlippingCardSound;

            GameEvents.MatchingCard += PlayMatchingCardSound;
            GameEvents.MismatchingCard += PlayMismatchingCardSound;
            GameEvents.GameOver += PlayGameOverSound;
        }

        private void OnDisable()
        {
            UIEvents.FlippingCard -= PlayFlippingCardSound;

            GameEvents.MatchingCard -= PlayMatchingCardSound;
            GameEvents.MismatchingCard -= PlayMismatchingCardSound;
            GameEvents.GameOver -= PlayGameOverSound;
        }

        private void Start()
        {
            if (_audioManager == null)
                _audioManager = GetComponent<AudioManager>();
        }


        // Play the flipping card sound effect
        private void PlayFlippingCardSound()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.FlippingCardSound);
        }

        // Play the matching card sound effect
        private void PlayMatchingCardSound()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.MatchingCardSound);
        }

        // Play the mismatching card sound effect
        private void PlayMismatchingCardSound()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.MismatchingCardSound);
        }

        // Play the game over sound effect
        private void PlayGameOverSound()
        {
            _audioManager.PlaySFX(_audioManager.AudioSettingsData.GameOverSound);
        }
    }
}
