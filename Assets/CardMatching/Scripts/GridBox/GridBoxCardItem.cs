using CardMatching.Datas;
using CardMatching.Events;
using CardMatching.ScriptableObjects;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace CardMatching.GridBox
{
    public class GridBoxCardItem : MonoBehaviour, IPointerClickHandler
    {
        //public Action<GridBoxCardItem> Disappeared;
        private Action<PointerEventData> _onPointerClick;


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

        private GridDimension mGridLocation;
        public GridDimension GridLocation
        {
            get
            {
                return mGridLocation;
            }
            set
            {
                mGridLocation = value;
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

        private bool _isOpen = false;


        public void Init(GridBoxCardData gridBoxCardData, GridDimension gridLocation, Action<PointerEventData> onPointerClick)
        {
            _gridBoxCardData = gridBoxCardData;
            _onPointerClick = onPointerClick;
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
            _onPointerClick = null;
        }


        private void SetDisplayImageSprite()
        {
            _displayImage.sprite = _isOpen ? _gridBoxCardData.CardIcon: _gridBoxCardData.CoverImage;
            _displayImage.preserveAspect = _isOpen;
        }

        //private void FlipCardWithAnimation(bool isOpen)
        //{
        //    FlipCard(isOpen);
        //}

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
            transform.transform.DOScaleX(1.0f, CardDataSO.Instance.FlipAniamtionTime).SetEase(CardDataSO.Instance.FlipAnimationEase).OnComplete(OnCompletedFlipAniamton);
        }

        private void OnCompletedFlipAniamton()
        {
            //m_IsOpen = !m_IsOpen;
            Debug.Log($"OnCompletedFlipAniamton-m_IsOpen: {_isOpen}");
        }

        #endregion


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("GridBoxCardItem-OnPointerClick");
            StartFlipAniamtion();
            _onPointerClick?.Invoke(eventData);
            UIEvents.FlippingCard?.Invoke();
        }
    }
}
