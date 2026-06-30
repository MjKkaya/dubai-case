using CardMatching.Core.Events;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


namespace CardMatching.UI.Panels
{
    public class BeginningPanel : BasePanel
    {
        [SerializeField] private Button _playButton;
        
        private GameEvents _gameEvents;

        
        protected override void Awake()
        {
            base.Awake();
            _playButton.onClick.AddListener(OnClickedPlayButton);
        }
        
        [Inject]
        public void Construct(GameEvents gameEvents)
        {
            _gameEvents = gameEvents;
            _gameEvents.GameOver += GameEvents_GameOver; // Awake'ten buraya taşıdık
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnClickedPlayButton);
            _gameEvents.GameOver -= GameEvents_GameOver;
        }


        private void OnClickedPlayButton()
        {
            _gameEvents.NewGameStarting?.Invoke();
            HidePanel();
        }

        private void GameEvents_GameOver()
        {
            ShowPanel();
        }
    }
}