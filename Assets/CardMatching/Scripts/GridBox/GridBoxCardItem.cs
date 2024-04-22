using CardMatching.Datas;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace CardMatching.GridBox
{
    public class GridBoxCardItem : MonoBehaviour, IPointerClickHandler
    {
        public Action<GridBoxCardItem> Disappeared;
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

        private GridLocation mGridLocation;
        public GridLocation GridLocation
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


        [SerializeField] private Image _coverImage;
        [SerializeField] private Image _iconImage;

        private bool isOpen;


        void Start()
        {
            isOpen = false;
            FlipCard(false);
        }

        public void Init(GridBoxCardData gridBoxCardData, GridLocation gridLocation, Action<PointerEventData> onPointerClick)
        {
            _gridBoxCardData = gridBoxCardData;
            _onPointerClick = onPointerClick;
            GridLocation = gridLocation;
        }

        

        public void ReloadCard()
        {
            isOpen = false;
            FlipCard(false);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            _onPointerClick = null;
        }


        private void FlipCard(bool isOpen)
        {
            _coverImage.gameObject.SetActive(!isOpen);
            _iconImage.gameObject.SetActive(isOpen);
        }

        private void FlipCardWithAnimation(bool isOpen)
        {
            FlipCard(isOpen);
        }

        private void SetName()
        {
            name = $"GridBoxItem_{GridLocation.X}_{GridLocation.Y}";
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("GridBoxCardItem-OnPointerClick");
            isOpen =! isOpen;
            FlipCard(isOpen);
            _onPointerClick?.Invoke(eventData);
        }
    }
}
