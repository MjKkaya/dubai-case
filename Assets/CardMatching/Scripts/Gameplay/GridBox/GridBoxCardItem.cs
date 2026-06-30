using CardMatching.Core.Interfaces;
using CardMatching.Core.Datas;
using CardMatching.Core.Events;
using CardMatching.Core.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace CardMatching.Gameplay.GridBox
{
    public class GridBoxCardItem : MonoBehaviour, IPointerClickHandler, IGridBoxCardItem
    {
        private GameEvents _gameEvents;
        
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

        public bool IsInteractable
        {
            get
            {
                return _canvasGroup.blocksRaycasts;
            }
        }
        
        public bool IsActive => isActiveAndEnabled;

        [SerializeField] private Image _displayImage;
        private CanvasGroup _canvasGroup;
        private bool _isOpen;
        private CardSettingsSO _cardSettings;


        private void Awake()
        {
            //CustomDebug.Log($"{this}-Awake");
            _canvasGroup = GetComponent<CanvasGroup>();
        }


        public void Init(GridBoxCardData gridBoxCardData, GridDimension gridLocation, GameEvents gameEvents,CardSettingsSO cardSettings)
        {
            _gridBoxCardData = gridBoxCardData;
            GridLocation = gridLocation;
            _isOpen = false;
            _gameEvents = gameEvents;
            _cardSettings = cardSettings;
            
            SetInteraction(false);
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

        public void SetInteraction(bool isActive)
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
            //CustomDebug.Log($"StartFlipAniamtion-_isOpen: {_isOpen}");
            transform.transform.DOScaleX(0.0f, _cardSettings.FlipAniamtionTime).SetEase(_cardSettings.FlipAnimationEase).OnComplete(FlipAnimationSecondStep);
        }

        private void FlipAnimationSecondStep()
        {
            _isOpen = !_isOpen;
            SetDisplayImageSprite();
            transform.transform.DOScaleX(1.0f, _cardSettings.FlipAniamtionTime).SetEase(_cardSettings.FlipAnimationEase).OnComplete(OnCompletedFlipAnimation);
        }

        private void OnCompletedFlipAnimation()
        {
            if(_isOpen)
                _gameEvents.CardFlipped?.Invoke(this);
            else
                SetInteraction(true);
        }


        private void StartFadeAnimation()
        {
            _canvasGroup.DOFade(0, _cardSettings.FlipAniamtionTime).SetEase(_cardSettings.FlipAnimationEase).OnComplete(OnCompletedFadeAnimation);
        }

        private void OnCompletedFadeAnimation()
        {
            gameObject.SetActive(false);
        }

        #endregion

        /// <summary>
        /// The method is member of IPointerClickHandler
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            SetInteraction(false);
            StartFlipAniamtion();
            _gameEvents.CardSelected?.Invoke();
        }
    }
}
