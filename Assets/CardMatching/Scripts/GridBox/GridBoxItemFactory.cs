using CardMatching.Datas;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;


namespace CardMatching.GridBox
{
    public class GridBoxItemFactory : MonoBehaviour
    {
        [SerializeField] private GridBoxCardItem _prefabGridBoxCardItem;


        private ObjectPool<GridBoxCardItem> mGridBoxItemObjectPool;


        void Start()
        {
            InitObjectPool();
        }


        #region Object Pool

        private void InitObjectPool()
        {
            mGridBoxItemObjectPool = new ObjectPool<GridBoxCardItem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30);
        }

        private GridBoxCardItem CreatePooledItem()
        {
            GridBoxCardItem gridBoxItem = Instantiate(_prefabGridBoxCardItem);
            return gridBoxItem;
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(GridBoxCardItem cardItem)
        {
            Debug.Log($"{this}-OnTakeFromPool-CardIconIndex:{cardItem.CardIconIndex}");
            cardItem.SetActive(true);
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(GridBoxCardItem cardItem)
        {
            Debug.Log($"{this}-OnReturnedToPool-CardIconIndex:{cardItem.CardIconIndex}");
            cardItem.SetActive(false);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(GridBoxCardItem cardItem)
        {
            Debug.Log($"{this}-OnDestroyPoolObject-CardIconIndex:{cardItem.CardIconIndex}");
            Destroy(cardItem.gameObject);
        }


        public GridBoxCardItem GetGridBoxItem(GridBoxCardData gridBoxData, GridLocation gridLocation, Action<PointerEventData> onPointerClick)
        {
            GridBoxCardItem gridBoxItem = mGridBoxItemObjectPool.Get();
            gridBoxItem.Init(gridBoxData, gridLocation, onPointerClick);
            gridBoxItem.Disappeared = GridBoxItem_Disappeared;
            return gridBoxItem;
        }

        private void GridBoxItem_Disappeared(GridBoxCardItem cardItem)
        {
            ReleaseGridBoxItem(cardItem);
        }

        public void ReleaseGridBoxItem(GridBoxCardItem cardItem)
        {
            mGridBoxItemObjectPool.Release(cardItem);
        }

        #endregion
    }
}