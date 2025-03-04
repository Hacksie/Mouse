using UnityEngine;

namespace HackedDesign
{
    public static class Extensions
    {
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
