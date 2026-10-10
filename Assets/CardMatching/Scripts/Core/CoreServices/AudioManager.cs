using System.Collections.Generic;
using CardMatching.Core.Interfaces;
using CardMatching.Core.Enums;
using UnityEngine;
using CardMatching.Core.Settings.Audio;


namespace CardMatching.Core.CoreServices
{
    public class AudioManager : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource _audioSource;
        private readonly Dictionary<AudioID, AudioEventSO> _audioCache = new Dictionary<AudioID, AudioEventSO>();


        public void LoadDatabase(AudioSettingsSO database)
        {
            if (database == null || database.AudioEntries == null) 
                return;

            foreach (var entry in database.AudioEntries)
            {
                if (!_audioCache.ContainsKey(entry.ID))
                    _audioCache.Add(entry.ID, entry.AudioEvent);
                else
                    Debug.LogWarning($"[AudioManager] Çakışma! {entry.ID} zaten yüklü.");
            }
        }

        public void UnloadDatabase(AudioSettingsSO database)
        {
            if (database == null || database.AudioEntries == null) return;
            foreach (var entry in database.AudioEntries)
            {
                if (_audioCache.ContainsKey(entry.ID))
                    _audioCache.Remove(entry.ID); 
            }
        }


        public void PlaySFX(AudioID audioId)
        {
            if (_audioCache.TryGetValue(audioId, out AudioEventSO audioData))
            {
                if (audioData.Clips != null && audioData.Clips.Length > 0)
                {
                    AudioClip clip = audioData.Clips[Random.Range(0, audioData.Clips.Length)];
                    _audioSource.volume = audioData.Volume;
                    _audioSource.pitch = Random.Range(audioData.PitchRange.x, audioData.PitchRange.y);
                    _audioSource.PlayOneShot(clip);
                }
            }
        }


        /*
MULTI-SCENE (ÇOKLU SAHNE) MİMARİSİNE GEÇİŞ NOTU: İleride çoklu sahneye (Örn: Lobi Sahnesi ve Oyun Sahnesi) geçtiğinizde yapmanız gereken tek değişiklik şudur:

AudioManager nesnesi, Unity'de Lobi (Root) Sahnesinde duracak ve ProjectLifetimeScope içine builder.RegisterComponent ile eklenecektir.
Lobi Sahnesindeki Loader, LobbyAudioDatabase'i yükleyecektir.
Kullanıcı eşleşme bulup Oyun (Match) Sahnesine geçtiğinde, GameLifetimeScope sadece SceneAudioLoader'ı kaydedip içine GameplayAudioDatabase verecektir. (AudioManager'ı tekrar kaydetmeyecektir, çünkü o zaten Root'tan otomatik olarak gelecektir).
Oyun bitip lobiye dönüldüğünde GameLifetimeScope yok olacağı için, SceneAudioLoader'ın Dispose() metodu otomatik tetiklenecek ve RAM'deki oyun sesleri kusursuzca temizlenecektir.

        */
    }
}