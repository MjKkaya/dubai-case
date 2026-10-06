using CardMatching.Core.CoreServices;
using CardMatching.Core.Infrastructure;
using CardMatching.Core.Interfaces;
using CardMatching.Core.Settings;
using CardMatching.Features.LevelProgression.Installers;
using CardMatching.Features.LevelProgression.Services;
using CardMatching.Features.LevelProgression.View;
using CardMatching.Features.MatchMechanic.Controllers;
using CardMatching.Features.MatchMechanic.Installers;
using CardMatching.Features.MatchMechanic.View;
using CardMatching.Features.ScoreSystem.Controllers;
using CardMatching.Features.ScoreSystem.Installers;
using CardMatching.Features.GameFlow.View;
using CardMatching.Features.GameFlow.Installers;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using CardMatching.Features.GameFlow.Controllers;
using MessagePipe;
using CardMatching.Feature.AudioSystem;
using CardMatching.CompositionRoot.Installers;
using CardMatching.Core.Events.Signals;


namespace CardMatching.CompositionRoot
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Game Data Settings")]
        [SerializeField] private CurrentGameDataSO _currentGameData;
        [SerializeField] private AudioSettingsSO _audioSettings;

        [Header("Scene References")]
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private GameplayManager _gameplayManager;
        [SerializeField] private GridBoxController _gridBoxController;
        [SerializeField] private GridBoxItemFactory _gridBoxItemFactory;

        [SerializeField] private BeginningPanel _beginningPanel;
        [SerializeField] private StaticsPanel _staticsPanel;
        [SerializeField] private UnfinishedLevelProgressPanel _progressPanel;

        [Header("Manager Settings")]
        [Tooltip("Minimum correct answers in a row to get Combo point")]
        [Range(2, 5)]
        [SerializeField] private int _minimumStreak = 2;


        protected override void Configure(IContainerBuilder builder)
        {
            // --- MESSAGE PIPE ---
            // Every Feature put own signels to GameLifetimeScope
            var options = builder.RegisterMessagePipe();
            builder.RegisterCoreSignals(options);
            builder.RegisterGameFlowSignals(options);
            builder.RegisterMatchMechanicSignals(options);
            builder.RegisterScoreSystemSignals(options);
            builder.RegisterLevelProgressionSignals(options);


            // --- CORE DATAS ---
            builder.RegisterInstance(_currentGameData);

            /*
            // Sistem inşası (Build) tamamlandığında tetiklenecek olay:
            builder.RegisterBuildCallback(container =>
            {
                // VContainer'dan kendi yarattığı GameEvents'i bana vermesini istiyorum
                var createdGameEvents = container.Resolve<GameEvents>();
                
                // Ve bunu Core'daki Data nesneme manuel olarak veriyorum (Pure DI)
                _currentGameData.Initialize(createdGameEvents); 
            });
            */


            // --- INFRASTRUCTURE ---
            builder.RegisterComponent<IAudioService>(_audioManager);
            builder.RegisterInstance(_audioSettings);
            builder.RegisterEntryPoint<GameplaySounds>();
            builder.RegisterEntryPoint<UnfinishedLevelProgressManager>();
            builder.Register<ISaveRepository, PlayerPrefsSaveRepository>(Lifetime.Singleton);

            // --- GAMEPLAY ---
            // YENİ: CommandInvoker'ı normal bir C# sınıfı olarak kaydet (Scoped = oyun boyunca 1 tane)
            builder.Register<CardMatchCommandInvoker>(Lifetime.Scoped);
            builder.RegisterComponent(_gameplayManager).AsImplementedInterfaces();
            builder.RegisterEntryPoint<ScoreManager>().WithParameter(_minimumStreak);
            builder.RegisterComponent(_gridBoxController).AsImplementedInterfaces();
            builder.RegisterComponent(_gridBoxItemFactory);
            builder.RegisterEntryPoint<GameSessionController>();

            // --- UI ---
            // AsImplementedInterfaces() diyerek VContainer'a bu sınıftaki IInitializable'ı bulmasını ve tetiklemesini söylüyoruz.
            // builder.RegisterComponent(_panelManager).AsImplementedInterfaces();
            builder.RegisterComponent(_beginningPanel).AsImplementedInterfaces();
            builder.RegisterComponent(_staticsPanel).AsImplementedInterfaces();
            builder.RegisterComponent(_progressPanel).AsImplementedInterfaces();
        }


        private void OnApplicationPause(bool pauseStatus)
        {
            if (Container != null)
            {
                var publisher = Container.Resolve<IPublisher<ApplicationPausedSignal>>();
                publisher.Publish(new ApplicationPausedSignal() { IsPaused = pauseStatus });
            }
        }

        // protected override void OnDestroy()
        // {
        // ÖNEMLİ: Scope scripti kapanırken SO'daki eventleri temizleyelim (Memory Leak olmasın)
        // if (_currentGameData != null)
        //     _currentGameData.Dispose();
        //     base.OnDestroy();
        // }
    }
}