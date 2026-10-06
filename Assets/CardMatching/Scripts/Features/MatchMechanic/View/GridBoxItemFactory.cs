using CardMatching.Core.Settings;
using CardMatching.Core.Utils;
using CardMatching.Features.MatchMechanic.Signals;
using MessagePipe;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;


namespace CardMatching.Features.MatchMechanic.View
{
    public class GridBoxItemFactory : MonoBehaviour
    {
        [SerializeField] private CardSettingsSO _cardSettingsSo;
        [SerializeField] private GridBoxCardItem _prefabGridBoxCardItem;

        private ObjectPool<GridBoxCardItem> _gridBoxItemObjectPool;
        private IPublisher<CardFlippedSignal> _cardFlippedPub;
        private IPublisher<CardSelectedSignal> _cardSelectedPub;


        [Inject]
        public void Construct(IPublisher<CardFlippedSignal> cardFlippedPub, IPublisher<CardSelectedSignal> cardSelectedPub)
        {
            _cardFlippedPub = cardFlippedPub;
            _cardSelectedPub = cardSelectedPub;
        }
        

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
            gridBoxItem.Init(gridBoxData, gridLocation, _cardSettingsSo, _cardFlippedPub, _cardSelectedPub);
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