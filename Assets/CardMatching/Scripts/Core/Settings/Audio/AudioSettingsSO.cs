using System;
using System.Collections.Generic;
using CardMatching.Core.Enums;
using UnityEngine;


namespace CardMatching.Core.Settings.Audio
{
    [Serializable]
    public struct AudioEntry
    {
        public AudioID ID;
        public AudioEventSO AudioEvent;
    }


    [CreateAssetMenu(fileName = "AudioSettingsData", menuName = "CardMatching/AudioSettingsData")]
    public class AudioSettingsSO : ScriptableObject
    {
        public List<AudioEntry> AudioEntries;
    }
}