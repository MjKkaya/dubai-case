using UnityEngine;


namespace CardMatching.Panels
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BasePanel : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;


        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            HidePanel();
        }


        public virtual void HidePanel()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void ShowPanel()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}