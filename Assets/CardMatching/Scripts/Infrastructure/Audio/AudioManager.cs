using CardMatching.Core.Interfaces;
using UnityEngine;


namespace CardMatching.Infrastructure.Audio
{
    public class AudioManager : MonoBehaviour, IAudioService
    {
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