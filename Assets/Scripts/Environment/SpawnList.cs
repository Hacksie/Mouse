
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "SpawnList", menuName = "Mouse/Level/SpawnList")]
    public class SpawnList: ScriptableObject
    {
        public List<GameObject> bgProps = new List<GameObject>();
        public List<GameObject> obstacles = new List<GameObject>();
        public List<GameObject> enemies = new List<GameObject>();
        public int minLevel = 0;
        public int maxLevel = 100;
        public float obstacleChance = 0.5f;
        public float enemyChance = 0.25f;
    }
}
