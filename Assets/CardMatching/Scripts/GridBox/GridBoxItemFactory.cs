using CardMatching.Datas;
using CardMatching.ScriptableObjects;
using CardMatching.Utilities;
using UnityEngine;
using UnityEngine.Pool;


namespace CardMatching.GridBox
{
    public class GridBoxItemFactory : MonoBehaviour
    {
        [SerializeField] private CardDataSO _cardDataSO;
        [SerializeField] private GridBoxCardItem _prefabGridBoxCardItem;

        private ObjectPool<GridBoxCardItem> _gridBoxItemObjectPool;


        void Awake()
        {
            InitObjectPool();
        }


        #region Object Pool

        private void InitObjectPool()
        {
            CustomDebug.Log($"{this}-InitObjectPool");
            _gridBoxItemObjectPool = new ObjectPool<GridBoxCardItem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30);
        }

        private GridBoxCardItem CreatePooledItem()
        {
            CustomDebug.Log($"{this}-CreatePooledItem");
            GridBoxCardItem gridBoxItem = Instantiate(_prefabGridBoxCardItem);
            return gridBoxItem;
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(GridBoxCardItem cardItem)
        {
            CustomDebug.Log($"{this}-OnTakeFromPool-cardItem:{cardItem.name}");
            cardItem.SetActive(true);
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(GridBoxCardItem cardItem)
        {
            CustomDebug.Log($"{this}-OnReturnedToPool-cardItem:{cardItem.name}");
            cardItem.SetActive(false);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(GridBoxCardItem cardItem)
        {
            CustomDebug.Log($"{this}-OnDestroyPoolObject-cardItem:{cardItem.name}");
            Destroy(cardItem.gameObject);
        }


        public GridBoxCardItem GetGridBoxItem(GridBoxCardData gridBoxData, GridDimension gridLocation)
        {
            CustomDebug.Log($"{this}-GetGridBoxItem-CountAll:{_gridBoxItemObjectPool.CountAll}");
            GridBoxCardItem gridBoxItem = _gridBoxItemObjectPool.Get();
            gridBoxItem.Init(gridBoxData, gridLocation);
            //gridBoxItem.Disappeared = GridBoxItem_Disappeared;
            return gridBoxItem;
        }

        public void ReleaseGridBoxItem(GridBoxCardItem cardItem)
        {
            CustomDebug.Log($"{this}-ReleaseGridBoxItem-CountAll:{_gridBoxItemObjectPool.CountAll}, cardItem:{cardItem}");
            _gridBoxItemObjectPool.Release(cardItem);
        }

        #endregion
    }
}