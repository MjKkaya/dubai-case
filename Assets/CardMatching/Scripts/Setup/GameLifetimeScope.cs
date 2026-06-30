using CardMatching.Core.Events;
using CardMatching.Core.Interfaces;
using CardMatching.Core.ScriptableObjects;
using CardMatching.Gameplay;
using CardMatching.Gameplay.GridBox;
using CardMatching.Infrastructure;
using CardMatching.Infrastructure.Audio;
using CardMatching.UI;
using CardMatching.UI.Panels;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace CardMatching.Setup
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Game Data Settings")]
        [SerializeField] private CurrentGameDataSO _currentGameData;
        [SerializeField] private AudioSettingsSO _audioSettings;
        
        [Header("Scene References")]
        [SerializeField] private PanelManager _panelManager;
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
            // --- Events ---
            builder.Register<GameEvents>(Lifetime.Singleton);
            builder.Register<UIEvents>(Lifetime.Singleton);
            
            
            // --- CORE DATAS ---
            builder.RegisterInstance(_currentGameData);
            
            // Sistem inşası (Build) tamamlandığında tetiklenecek olay:
            builder.RegisterBuildCallback(container =>
            {
                // VContainer'dan kendi yarattığı GameEvents'i bana vermesini istiyorum
                var createdGameEvents = container.Resolve<GameEvents>();
                
                // Ve bunu Core'daki Data nesneme manuel olarak veriyorum (Pure DI)
                _currentGameData.Initialize(createdGameEvents); 
            });
            
            
            // --- INFRASTRUCTURE ---
            builder.RegisterComponent<IAudioService>(_audioManager);
            builder.RegisterInstance(_audioSettings);
            builder.RegisterEntryPoint<GameplaySounds>();
            builder.RegisterEntryPoint<UnfinishedLevelProgressManager>();
            
            // --- GAMEPLAY ---
            // YENİ: CommandInvoker'ı normal bir C# sınıfı olarak kaydet (Scoped = oyun boyunca 1 tane)
            builder.Register<CardMatchCommandInvoker>(Lifetime.Scoped);
            builder.RegisterComponent(_gameplayManager).AsImplementedInterfaces();
            builder.RegisterEntryPoint<ScoreManager>().WithParameter(_minimumStreak);
            builder.RegisterComponent(_gridBoxController).AsImplementedInterfaces();
            builder.RegisterComponent(_gridBoxItemFactory);
            
            // --- UI ---
            // AsImplementedInterfaces() diyerek VContainer'a bu sınıftaki IInitializable'ı bulmasını ve tetiklemesini söylüyoruz.
            builder.RegisterComponent(_panelManager).AsImplementedInterfaces();
            builder.RegisterComponent(_beginningPanel);
            builder.RegisterComponent(_staticsPanel);
            builder.RegisterComponent(_progressPanel);
            
        }
        
        // ÖNEMLİ: Scope scripti kapanırken SO'daki eventleri temizleyelim (Memory Leak olmasın)
        protected override void OnDestroy()
        {
            if (_currentGameData != null)
                _currentGameData.Dispose();
                
            base.OnDestroy();
        }
    }
}