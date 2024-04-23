using System.Collections.Generic;
using UnityEngine;


namespace CardMatching.Utilities
{
    public static class UIExtensions
    {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            int listCount = list.Count;
            int lastIndex = list.Count - 1;

            for (var i = 0; i < lastIndex; ++i)
            {
                int randomIndex = Random.Range(i, listCount);
                var tmp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = tmp;
            }
        }
    }
}