using System;

using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "EnemySettings", menuName = "Mouse/Settings/Enemy")]
    public class EnemySettings : ScriptableObject
    {
        [SerializeField] private float reactionTime = 1.5f;
        [SerializeField] private float recognitionTime = 0.66f;
        [SerializeField] private bool aggressive = true;
        [SerializeField] private float giveUpTime = 10f;
        [SerializeField] private float maxVisualRange = 30f;

        public float ReactionTime { get => reactionTime; set => reactionTime = value; }
        public bool Aggressive { get => aggressive; set => aggressive = value; }
        public float GiveUpTime { get => giveUpTime; set => giveUpTime = value; }
        public float MaxVisualRange { get => maxVisualRange; set => maxVisualRange = value; }
        public float RecognitionTime { get => recognitionTime; set => recognitionTime = value; }
    }
        
}
