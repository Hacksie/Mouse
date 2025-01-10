
using UnityEngine;

namespace HackedDesign
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] public float radius = 1f;
        [SerializeField] public bool air = true;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawSphere(transform.position, radius);
        }

        public Vector3 GetAirSpawnPosition()
        {
            var pos = Random.insideUnitCircle * radius;
            pos.y = Mathf.Abs(pos.y);

            return pos;
        }

        public Vector3 GetGroundSpawnPosition()
        {
            var posx = Random.Range(-radius, radius);
            return new Vector3(posx, transform.position.y, 0);
        }
    }
}
