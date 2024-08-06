using CardMatching.Datas;
using CardMatching.GridBox;

namespace CardMatching.Utilities
{
    public static class Tools
    {
        public static int[] ConvertGridBoxCardItemsToOneDimension(GridBoxCardItem[,] gridBoxCardItems)
        {
            int _yDimension = gridBoxCardItems.GetLength(0);
            int _xDimension = gridBoxCardItems.GetLength(1);
            int[] indexArray = new int[gridBoxCardItems.Length];

            int indexNo = 0;
            GridBoxCardItem cardItem;
            for (int y = 0; y < _yDimension; y++)
            {
                for (int x = 0; x < _xDimension; x++)
                {
                    cardItem = gridBoxCardItems[y, x];
                    if (cardItem != null && cardItem.isActiveAndEnabled)
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