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

        // for test - displaying in the inspector.
        [SerializeField] private int _cardIconIndex;
        [SerializeField] private Image _displayImage;
        private Image _raycatsTargetImage;
        private bool _isOpen = false;


        private void Awake()
        {
            _raycatsTargetImage = GetComponent<Image>();
        }


        public void Init(GridBoxCardData gridBoxCardData, GridDimension gridLocation)
        {
            _cardIconIndex = gridBoxCardData.CardIconIndex;
            _gridBoxCardData = gridBoxCardData;
            GridLocation = gridLocation;
            SetDisplayImageSprite();
        }

        public void ReloadCard()
        {
            _isOpen = false;
            SetDisplayImageSprite();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetInteractible(bool isActive)
        {
            _raycatsTargetImage.raycastTarget = isActive;
        }


        private void SetDisplayImageSprite()
        {
            _displayImage.sprite = _isOpen ? _gridBoxCardData.CardIcon: _gridBoxCardData.CoverImage;
            _displayImage.preserveAspect = _isOpen;
        }

        private void SetName()
        {
            name = $"GridBoxItem_{GridLocation.Y}_{GridLocation.X}";
        }


        #region Flipping Animation

        public void StartFlipAniamtion()
        {
            Debug.Log($"StartFlipAniamtion-_isOpen: {_isOpen}");
            transform.transform.DOScaleX(0.0f, CardDataSO.Instance.FlipAniamtionTime).SetEase(CardDataSO.Instance.FlipAnimationEase).OnComplete(FlipAnimationSecondStep);
        }

        private void FlipAnimationSecondStep()
        {
            _isOpen = !_isOpen;
            SetDisplayImageSprite();
            transform.transform.DOScaleX(1.0f, CardDataSO.Instance.FlipAniamtionTime).SetEase(CardDataSO.Instance.FlipAnimationEase);
        }

        #endregion


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Mj------");
            SetInteractible(false);
            GameEvents.CardSelected?.Invoke(this);
        }
    }
}
