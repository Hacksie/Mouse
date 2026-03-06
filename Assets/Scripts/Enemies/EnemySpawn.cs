
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private List<EnemyType> allowedEnemies = new();
        [SerializeField] private List<GameObject> allowedProps = new();

        public List<EnemyType> AllowedEnemies => this.allowedEnemies;
        public bool CanSpawnEnemy(EnemyType enemyType) => allowedEnemies.Contains(enemyType);

        public bool CanSpawnProp(string name) => allowedProps.Any(prop => prop.name == name);
    }
}
