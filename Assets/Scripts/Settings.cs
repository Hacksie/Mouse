
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Settings", menuName = "State/Settings")]
    public class Settings : ScriptableObject
    {
        [Header("Game settings")]
        public bool skipIntro = false;

        [Header("Player Settings")]
        public float moveSpeed = 10.0f;
        public float dashSpeed = 10.0f;
        public float dashTime = 0.25f;
        public LayerMask groundMask;
        public LayerMask aimMask;
    }
}