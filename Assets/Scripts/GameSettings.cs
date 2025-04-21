using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Mouse/Settings/Game")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private bool skipIntro = false;
        [SerializeField] private bool autorun = true;
        [SerializeField] private bool startPistol = false;
        [SerializeField] private float knockbackTime = 0.15f;
        [SerializeField] private float knockbackFreezeTime = 0.20f;
        [SerializeField] private float knockbackAmount = 10f;
        [SerializeField] private float minBasePropGap = 10f;
        [SerializeField] private float maxBasePropGap = 20f;
        [SerializeField] private float buildingGapChance = 0.66f;

        public bool SkipIntro { get => skipIntro; }
        public bool Autorun { get => autorun; }
        public bool StartPistol { get => startPistol; }
        public float KnockbackTime { get => knockbackTime; }
        public float KnockbackFreezeTime { get => knockbackFreezeTime; }
        public float KnockbackAmount { get => knockbackAmount; }
        public float MinBasePropGap { get => minBasePropGap; }
        public float MaxBasePropGap { get => maxBasePropGap; }
        public float BuildingGapChance { get => buildingGapChance; }
    }
}
