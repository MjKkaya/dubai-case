using System.Collections.Generic;
using CardMatching.Datas;
using CardMatching.ScriptableObjects;
using CardMatching.Utilities;
using UnityEngine;
using UnityEngine.Pool;


public class GridBoxItemDataFactory : MonoBehaviour
{
    [SerializeField] private CardDataSO _cardDataSO;

    private ObjectPool<GridBoxCardData> _gridBoxItemDataPool;


    private void Awake()
    {
        InitObjectPool();
    }


    #region Object Pool

    private void InitObjectPool()
    {
        CustomDebug.Log($"{this}-InitObjectPool");
        _gridBoxItemDataPool = new ObjectPool<GridBoxCardData>(CreatePooledData, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 30);
    }

    private GridBoxCardData CreatePooledData()
    {
        CustomDebug.Log($"{this}-CreatePooledData");
        GridBoxCardData gridBoxItem = new ();
        return gridBoxItem;
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GridBoxCardData cardData)
    {
        CustomDebug.Log($"{this}-OnTakeFromPool-cardData:{cardData}");
        cardData.Clear();
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GridBoxCardData cardData)
    {
        CustomDebug.Log($"{this}-OnReturnedToPool-cardData:{cardData}");
        cardData.Clear();
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GridBoxCardData cardData)
    {
        CustomDebug.Log($"{this}-OnDestroyPoolObject-cardData:{cardData}");
        cardData.Clear();
    }


    public GridBoxCardData GetGridBoxItemData()
    {
        CustomDebug.Log($"{this}-GetGridBoxItemData-CountAll:{_gridBoxItemDataPool.CountAll}");
        GridBoxCardData gridBoxItemData = _gridBoxItemDataPool.Get();
        return gridBoxItemData;
    }

    public void ReleaseGridBoxItemData(GridBoxCardData cardData)
    {
        CustomDebug.Log($"{this}-ReleaseGridBoxItemData-CountAll:{_gridBoxItemDataPool.CountAll}, cardData:{cardData.CardIconIndex}");
        _gridBoxItemDataPool.Release(cardData);
    }

    #endregion


    public void CreateCardDataListForCurrentLevel(List<GridBoxCardData> dataList, int pairCount)
    {
        int allCardIconCount = _cardDataSO.CardIconList.Count;

        List<int> iconIndexList = new();
        for (int i = 0; i < allCardIconCount; i++)
        {
            iconIndexList.Add(i);
        }

        CustomDebug.Log($"{this}-CreateCardDataListForCurrentLevel-pairCount{pairCount}, allCardIconCount:{allCardIconCount}");

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

        CustomDebug.Log($"{this}-CreateCardDataListForCurrentLevel-roundCount:{roundCount}, overItemCount:{overItemCount}");

        if (roundCount > 0)
        {
            for (int i = 0; i < roundCount; i++)
            {
                for (int k = 0; k < allCardIconCount; k++)
                {
                    GridBoxCardData gridBoxCardData = GetGridBoxItemData();
                    gridBoxCardData.Set(k, _cardDataSO.CardIconList[k], _cardDataSO.CardCoverSprite);
                    dataList.Add(gridBoxCardData);
                }
            }
        }

        int randomIndex;
        for (int i = 0; i < overItemCount; i++)
        {
            randomIndex = Random.Range(0, iconIndexList.Count);

            GridBoxCardData gridBoxCardData = GetGridBoxItemData();
            gridBoxCardData.Set(iconIndexList[randomIndex], _cardDataSO.CardIconList[iconIndexList[randomIndex]], _cardDataSO.CardCoverSprite);
            dataList.Add(gridBoxCardData);

            iconIndexList.RemoveAt(randomIndex);
        }
    }

    public void CreateCardDataListWithCurrentData(List<GridBoxCardData> dataList, int[] iconIndexArray)
    {
        dataList.Clear();
        int iconIndexArrayLength = iconIndexArray.Length;
        int iconIndex;

        for (int i = 0; i < iconIndexArrayLength; i++)
        {
            iconIndex = iconIndexArray[i];
            if (iconIndex == GridBoxCardData.EmptyIndexNo)
                dataList.Add(new GridBoxCardData(iconIndex, null, null));
            else
                dataList.Add(new GridBoxCardData(iconIndex, _cardDataSO.CardIconList[iconIndex], _cardDataSO.CardCoverSprite));
        }
    }
}