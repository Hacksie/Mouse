using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"Multiple instances of {typeof(T)} detected. Keeping the first one.");
                Destroy(gameObject); // Optional: Enforce singleton
                return;
            }

            Instance = this as T;
        }
    }
}
