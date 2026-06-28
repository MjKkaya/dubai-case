using CardMatching.Core.Datas;
using CardMatching.Core.Interfaces;


namespace CardMatching.Core.Helpers
{
    public static class Tools
    {
        public static int[] ConvertGridBoxCardItemsToOneDimension(IGridBoxCardItem[,] gridBoxCardItems)
        {
            int _yDimension = gridBoxCardItems.GetLength(0);
            int _xDimension = gridBoxCardItems.GetLength(1);
            int[] indexArray = new int[gridBoxCardItems.Length];

            int indexNo = 0;
            IGridBoxCardItem cardItem;
            for (int y = 0; y < _yDimension; y++)
            {
                for (int x = 0; x < _xDimension; x++)
                {
                    cardItem = gridBoxCardItems[y, x];
                    if (cardItem != null && cardItem.IsActive)
                        indexArray[indexNo] = cardItem.CardIconIndex;
                    else
                        indexArray[indexNo] = GridBoxCardData.EmptyIndexNo;
                    indexNo++;
                }
            }

            return indexArray;
        }
    }
}