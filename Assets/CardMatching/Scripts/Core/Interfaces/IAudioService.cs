using CardMatching.Core.Enums;
using CardMatching.Core.Settings.Audio;


namespace CardMatching.Core.Interfaces
{
    public interface IAudioService
    {
        void PlaySFX(AudioID audioId);
        void LoadDatabase(AudioSettingsSO database);
        void UnloadDatabase(AudioSettingsSO database);
    }
}