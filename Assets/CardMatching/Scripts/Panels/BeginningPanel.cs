using CardMatching.Events;
using UnityEngine;
using UnityEngine.UI;


namespace CardMatching.Panels
{
    public class BeginningPanel : BasePanel
    {
        [SerializeField] private Button _playButton;


        protected override void Awake()
        {
            base.Awake();
            _playButton.onClick.AddListener(OnClickedPlayButton);
            GameEvents.GameOver += GameEvents_GameOver;
        }

        private void OnDestroy()
        {
            _playButton.onClick.RemoveListener(OnClickedPlayButton);
            GameEvents.GameOver -= GameEvents_GameOver;
        }


        private void OnClickedPlayButton()
        {
            GameEvents.GameStarting?.Invoke();
            HidePanel();
        }

        private void GameEvents_GameOver()
        {
            ShowPanel();
        }
    }
}