using CardMatching.Datas;
using CardMatching.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


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
            Debug.Log($"{this}-InitObjectPool");
            _gridBoxItemObjectPool = new ObjectPool<GridBoxCardItem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30);
        }

        private GridBoxCardItem CreatePooledItem()
        {
            Debug.Log($"{this}-CreatePooledItem");
            GridBoxCardItem gridBoxItem = Instantiate(_prefabGridBoxCardItem);
            return gridBoxItem;
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(GridBoxCardItem cardItem)
        {
            Debug.Log($"{this}-OnTakeFromPool-cardItem:{cardItem.name}");
            cardItem.SetActive(true);
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(GridBoxCardItem cardItem)
        {
            Debug.Log($"{this}-OnReturnedToPool-cardItem:{cardItem.name}");
            cardItem.SetActive(false);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(GridBoxCardItem cardItem)
        {
            Debug.Log($"{this}-OnDestroyPoolObject-cardItem:{cardItem.name}");
            Destroy(cardItem.gameObject);
        }


        public GridBoxCardItem GetGridBoxItem(GridBoxCardData gridBoxData, GridDimension gridLocation)
        {
            GridBoxCardItem gridBoxItem = _gridBoxItemObjectPool.Get();
            gridBoxItem.Init(gridBoxData, gridLocation);
            //gridBoxItem.Disappeared = GridBoxItem_Disappeared;
            return gridBoxItem;
        }

        public void ReleaseGridBoxItem(GridBoxCardItem cardItem)
        {
            _gridBoxItemObjectPool.Release(cardItem);
        }

        #endregion


        public GridBoxCardData[] CreateCardDataListForCurrentLevel(int pairCount)
        {
            GridBoxCardData[] gridBoxCardDatas = new GridBoxCardData[pairCount];
            int allCardIconCount = _cardDataSO.CardIconList.Count;

            List<int> iconIndexList = new();
            for (int i = 0; i < allCardIconCount; i++)
            {
                iconIndexList.Add(i);
            }

            Debug.Log($"{this}-CreateCardDataListForCurrentLevel-pairCount{pairCount}, allCardIconCount:{allCardIconCount}");

            //if pairCount bigger than cardIconCount we don't need to get random index size.
            //examp: pairCount = 15 and cardIconcount = 10, we only need 5 random index. Because first 10 random index must be same current 10 icons index.
            int overItemCount;
            int roundCount = 0;
            if (pairCount >= allCardIconCount)
            {
                roundCount = pairCount / allCardIconCount;
                overItemCount = (roundCount * pairCount) - allCardIconCount;
            }
            else
            {
                overItemCount = pairCount;
            }

            Debug.Log($"{this}-CreateCardDataListForCurrentLevel-roundCount:{roundCount}, overItemCount:{overItemCount}");

            int gridBoxCardDatasNextIndex = 0;
            if (roundCount > 0)
            {
                for (int i = 0; i < roundCount; i++)
                {
                    for (int k = 0; k < allCardIconCount; k++)
                    {
                        gridBoxCardDatas[gridBoxCardDatasNextIndex] = new GridBoxCardData(k, _cardDataSO.CardIconList[k], _cardDataSO.CardCoverSprite);
                        gridBoxCardDatasNextIndex++;
                    }
                }
            }

            int randomIndex;
            for (int i = 0; i < overItemCount; i++)
            {
                randomIndex = Random.Range(0, iconIndexList.Count);
                gridBoxCardDatas[gridBoxCardDatasNextIndex + i] = new GridBoxCardData(iconIndexList[randomIndex], _cardDataSO.CardIconList[iconIndexList[randomIndex]], _cardDataSO.CardCoverSprite);
                iconIndexList.RemoveAt(randomIndex);
            }

            return gridBoxCardDatas;
        }


        public GridBoxCardData[] CreateCardDataListWithCurrentData(int[] iconIndexArray)
        {
            int iconIndexArrayLength = iconIndexArray.Length;
            GridBoxCardData[] gridBoxCardDatas = new GridBoxCardData[iconIndexArrayLength];

            int iconIndex;
            for (int i = 0; i < iconIndexArrayLength; i++)
            {
                iconIndex = iconIndexArray[i];
                if (iconIndex == CurrentGameDataSO.EmptyBoxIconIndex)
                    gridBoxCardDatas[i] = new GridBoxCardData(iconIndex, null, null);
                else
                    gridBoxCardDatas[i] = new GridBoxCardData(iconIndex, _cardDataSO.CardIconList[iconIndex], _cardDataSO.CardCoverSprite);
            }

            return gridBoxCardDatas;
        }
    }
}