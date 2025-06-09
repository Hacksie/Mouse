
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private List<EnemyType> allowedEnemies = new();

        public List<EnemyType> AllowedEnemies => this.allowedEnemies;

        public bool CanSpawn(EnemyType enemyType) => allowedEnemies.Contains(enemyType);

    }
}
