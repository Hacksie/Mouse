//using System;
using UnityEngine;

namespace HackedDesign
{
    public static class LayerMaskExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns>True if the layer mask contains a layer</returns>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}
