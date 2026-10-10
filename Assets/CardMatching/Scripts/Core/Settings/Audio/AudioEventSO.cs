using UnityEngine;

namespace CardMatching.Core.Settings.Audio
{
    [CreateAssetMenu(fileName = "New Audio Event", menuName = "CardMatching/Audio/Audio Event")]
    public class AudioEventSO : ScriptableObject
    {
        public AudioClip[] Clips; 
        [Range(0f, 1f)] public float Volume = 1f;
        public Vector2 PitchRange = new Vector2(1f, 1f); 
    }
}