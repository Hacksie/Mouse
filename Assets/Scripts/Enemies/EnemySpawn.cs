
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
            var position = Random.insideUnitCircle * radius;
            position.y = Mathf.Abs(position.y);

            return position;
        }

        public Vector3 GetGroundSpawnPosition()
        {
            var positionX = Random.Range(-radius, radius);
            return new Vector3(positionX, transform.position.y, 0);
        }
    }
}
