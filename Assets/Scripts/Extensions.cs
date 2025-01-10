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
    }
}
