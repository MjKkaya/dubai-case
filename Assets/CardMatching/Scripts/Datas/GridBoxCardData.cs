using UnityEngine;


namespace CardMatching.Datas
{
    public class GridBoxCardData
    {
        private int _cardIconIndex;
        public int CardIconIndex
        {
            get
            {
                return _cardIconIndex;
            }
            set
            {
                _cardIconIndex = value;
            }
        }


        public Sprite _cardIcon;
        public Sprite CardIcon
        {
            get
            {
                return _cardIcon;
            }
            set
            {
                _cardIcon = value;
            }
        }
    }

    public struct GridLocation
    {
        public int X { get; private set; }
        public int Y { get; private set; }


        public GridLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }

        public GridLocation CreateAndAdd(int x, int y)
        {
            return new GridLocation(X + x, Y + y);
        }

        public override string ToString() => $"({X}, {Y})";
    }
}
