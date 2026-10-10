using System;
using CardMatching.Core.Interfaces;
using CardMatching.Core.Settings.Audio;
using VContainer.Unity;


namespace CardMatching.CompositionRoot.Loaders
{
    public class SceneAudioLoader : IInitializable, IDisposable
    {
        private readonly IAudioService _audioService;
        private readonly AudioSettingsSO _sceneAudioDatabase; 

        
        public SceneAudioLoader(IAudioService audioService, AudioSettingsSO sceneAudioDatabase)
        {
            _audioService = audioService;
            _sceneAudioDatabase = sceneAudioDatabase;
        }

        public void Initialize()
        {
            _audioService.LoadDatabase(_sceneAudioDatabase);  
        } 
        
        public void Dispose() 
        {
            _audioService.UnloadDatabase(_sceneAudioDatabase);
        } 
    }
}