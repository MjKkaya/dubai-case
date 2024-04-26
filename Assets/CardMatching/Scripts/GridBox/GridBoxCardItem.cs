using CardMatching.Datas;
using CardMatching.Events;
using CardMatching.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace CardMatching.GridBox
{
    public class GridBoxCardItem : MonoBehaviour, IPointerClickHandler
    {
        private GridBoxCardData _gridBoxCardData;
        public GridBoxCardData GridBoxData
        {
            get
            {
                return _gridBoxCardData;
            }
        }

        public int CardIconIndex
        {
            get
            {
                return _gridBoxCardData != null ? _gridBoxCardData.CardIconIndex : 0;
            }
        }

        private GridDimension _gridLocation;
        public GridDimension GridLocation
        {
            get
            {
                return _gridLocation;
            }
            set
            {
                _gridLocation = value;
                SetName();
            }
        }

        private RectTransform mRectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (mRectTransform == null)
                    mRectTransform = GetComponent<RectTransform>();

                return mRectTransform;
            }
        }

        [SerializeField] private Image _displayImage;
        private CanvasGroup _canvasGroup;
        private bool _isOpen;


        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }


        public void Init(GridBoxCardData gridBoxCardData, GridDimension gridLocation)
        {
            _gridBoxCardData = gridBoxCardData;
            GridLocation = gridLocation;
            _isOpen = false;
            SetInteractible(false);
            SetDisplayImageSprite();
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                _canvasGroup.alpha = 1;
                gameObject.SetActive(isActive);
            }
            else
                StartFadeAnimation();
        }

        public void SetInteractible(bool isActive)
        {
            _canvasGroup.blocksRaycasts = isActive;
        }


        public void SetDisplayImageSprite()
        {
            _displayImage.sprite = _isOpen ? _gridBoxCardData.CardIcon: _gridBoxCardData.CoverImage;
            _displayImage.preserveAspect = _isOpen;
        }

        public void SetDisplayImageSprite(bool isOpen)
        {
            _displayImage.sprite = isOpen ? _gridBoxCardData.CardIcon : _gridBoxCardData.CoverImage;
            _displayImage.preserveAspect = isOpen;
        }

        private void SetName()
        {
            name = $"GridBoxItem_{GridLocation.Y}_{GridLocation.X}";
        }


        #region Animations

        public void StartFlipAniamtion()
        {
            Debug.Log($"StartFlipAniamtion-_isOpen: {_isOpen}");
            transform.transform.DOScaleX(0.0f, CardDataSO.Instance.FlipAniamtionTime).SetEase(CardDataSO.Instance.FlipAnimationEase).OnComplete(FlipAnimationSecondStep);
        }

        private void FlipAnimationSecondStep()
        {
            _isOpen = !_isOpen;
            SetDisplayImageSprite();
            transform.transform.DOScaleX(1.0f, CardDataSO.Instance.FlipAniamtionTime).SetEase(CardDataSO.Instance.FlipAnimationEase).OnComplete(OnCompletedFlipAnimation);
        }

        private void OnCompletedFlipAnimation()
        {
            if(_isOpen)
                GameEvents.CardFlipped?.Invoke(this);
            else
                SetInteractible(true);
        }


        private void StartFadeAnimation()
        {
            _canvasGroup.DOFade(0, CardDataSO.Instance.FlipAniamtionTime).SetEase(CardDataSO.Instance.FlipAnimationEase).OnComplete(OnCompletedFadeAnimation);
        }

        private void OnCompletedFadeAnimation()
        {
            gameObject.SetActive(false);
        }

        #endregion


        public void OnPointerClick(PointerEventData eventData)
        {
            SetInteractible(false);
            StartFlipAniamtion();
            GameEvents.CardSelected?.Invoke();
        }
    }
}
