using UnityEngine;


namespace CardMatching.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioSettingsData", menuName = "CardMatching/AudioSettingsData")]
    public class AudioSettingsSO : ScriptableObject
    {
        [Header("Sound Effects")]
        [Tooltip("AudioClip for flipping card")]
        [SerializeField] private AudioClip _flippingCardSound;
        [Tooltip("AudioClip for matching card")]
        [SerializeField] private AudioClip _matchingCardSound;
        [Tooltip("AudioClip for mismatching card")]
        [SerializeField] private AudioClip _mismatchingCardSound;
        [Tooltip("AudioClip for game over")]
        [SerializeField] private AudioClip _gameOverSound;


        // Properties
        public AudioClip FlippingCardSound => _flippingCardSound;
        public AudioClip MatchingCardSound => _matchingCardSound;
        public AudioClip MismatchingCardSound => _mismatchingCardSound;
        public AudioClip GameOverSound => _gameOverSound;
    }
}