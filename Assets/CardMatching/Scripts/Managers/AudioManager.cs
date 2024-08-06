using CardMatching.ScriptableObjects;
using UnityEngine;


namespace CardMatching.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSettingsSO AudioSettingsData => _audioSettingsData;


        [Tooltip("AudioSettings ScriptableObject storing sounds data and volume settings")]
        [SerializeField] AudioSettingsSO _audioSettingsData;

        [SerializeField] AudioSource _audioSource;


        public void PlaySFX(AudioClip clip)
        {
            //CustomDebug.Log($"{this}");
            _audioSource.Stop();
            if (clip != null)
                _audioSource.PlayOneShot(clip);
            else
                _audioSource.Stop();
        }
    }
}