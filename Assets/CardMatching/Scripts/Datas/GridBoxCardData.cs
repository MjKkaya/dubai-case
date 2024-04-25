using System;
using UnityEngine;


namespace CardMatching.Datas
{
    [SerializeField]
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


        private Sprite _cardIcon;
        public Sprite CardIcon
        {
            get
            {
                return _cardIcon;
            }
        }

        private Sprite _coverImage;
        public Sprite CoverImage
        {
            get
            {
                return _coverImage;
            }
        }

        public GridBoxCardData(int cardIconIndex, Sprite cardIcon, Sprite coverImage)
        {
            _cardIconIndex = cardIconIndex;
            _cardIcon = cardIcon;
            _coverImage = coverImage;
        }
    }


    public struct GridDimension
    {
        public int X { get; private set; }
        public int Y { get; private set; }


        public GridDimension(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
}
