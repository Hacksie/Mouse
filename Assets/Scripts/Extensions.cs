//using System;
using System.Collections;
using UnityEngine;

namespace HackedDesign
{
    public static class Extensions
    {
        public static float NextFloat(this System.Random rng, float min, float max)
        {
            return (float)(rng.NextDouble() * (max - min) + min);
        }

        public static float NextFloat(this System.Random rng, float max)
        {
            return (float)(rng.NextDouble() * max);
        }
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

        /// <summary>
        /// Converts an int to a hex string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHexString(this int value)
        {
            return value.ToString("X");
        }

        /// <summary>
        /// Converts an int to a hex string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHex(this long value)
        {
            return value.ToString("X");
        }

        /// <summary>
        /// Converts an int to a Base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64String(this int value)
        {
            System.Span<byte> bytes = stackalloc byte[4];
            System.BitConverter.TryWriteBytes(bytes, value);
            return System.Convert.ToBase64String(bytes).TrimEnd('=');
        }

        /// <summary>
        /// Converts an int to a Base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToBase64String(this long value)
        {
            System.Span<byte> bytes = stackalloc byte[8];
            System.BitConverter.TryWriteBytes(bytes, value);
            return System.Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Automatically binds a component in a monobehaviour to a reference if the reference is null. 
        /// Then logs a reminder warning 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="monoBehaviour"></param>
        /// <param name="reference"></param>
        public static void AutoBind<T>(this MonoBehaviour monoBehaviour, ref T reference)
        {
            if (reference == null)
            {
                reference ??= monoBehaviour.GetComponent<T>();
                Debug.LogWarning(nameof(reference) +" was not bound in the editor", monoBehaviour);
            }
        }

        /// https://github.com/adammyhre/Unity-GOAP/blob/master/Assets/_Project/Scripts/Utilities/GameObjectExtensions.cs
        /// <summary>
        /// Returns the object itself if it exists, null otherwise.
        /// </summary>
        /// <remarks>
        /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
        /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
        /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
        /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
        /// </remarks>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object being checked.</param>
        /// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
    }

    /// <summary>
    /// https://github.com/adammyhre/Unity-GOAP/blob/master/Assets/_Project/Scripts/Utilities/Vector3Extensions.cs
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Sets any x y z values of a Vector3
        /// </summary>
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }


    }
}
