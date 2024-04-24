using CardMatching.Events;
using UnityEngine;
using UnityEngine.UI;


namespace CardMatching.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BeginningPanel : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        private CanvasGroup _canvasGroup;


        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _playButton.onClick.AddListener(OnClickedPlayButton);
            GameEvents.GameOver += GameEvents_GameOver;
        }

        private void OnDestroy()
        {
            GameEvents.GameOver -= GameEvents_GameOver;
        }


        private void OnClickedPlayButton()
        {
            GameEvents.GameStarting?.Invoke();
            HidePanel();
        }


        private void HidePanel()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
        }

        private void ShowPanel()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
        }

        private void GameEvents_GameOver()
        {
            ShowPanel();
        }
    }
}