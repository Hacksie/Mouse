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
        [SerializeField] private bool stationary = false;
        [SerializeField] private float giveUpTime = 10f;
        [SerializeField] private float maxVisualRange = 30f;
        [SerializeField] private float minRoamTime = 1f;
        [SerializeField] private float maxRoamTime = 10.0f;
        [SerializeField] private float roamTime = 7f;
        
        public float ReactionTime { get => reactionTime; set => reactionTime = value; }
        public bool Aggressive { get => aggressive; set => aggressive = value; }
        public float GiveUpTime { get => giveUpTime; set => giveUpTime = value; }
        public float MaxVisualRange { get => maxVisualRange; set => maxVisualRange = value; }
        public float RecognitionTime { get => recognitionTime; set => recognitionTime = value; }
        public float MinRoamTime { get => minRoamTime; set => minRoamTime = value; }
        public float MaxRoamTime { get => maxRoamTime; set => maxRoamTime = value; }
        public float RoamTime { get => roamTime; set => roamTime = value; }
        public bool Stationary { get => this.stationary; set => this.stationary = value; }
    }
}
