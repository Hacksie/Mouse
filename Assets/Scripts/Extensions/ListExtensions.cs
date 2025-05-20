//using System;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public static class ListExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var list = new List<T>(source);
            int n = list.Count;
            while (n > 1)
            {
                int k = Random.Range(0, n--);
                (list[n], list[k]) = (list[k], list[n]);
            }
            return list;
        }
    }
}
