using UnityEngine;

namespace HackedDesign
{
    public class RandomBuildingHeight : MonoBehaviour
    {
        [SerializeField] private int minX = -4;
        [SerializeField] private int maxX = 4;
        [SerializeField] private int minY = -4;
        [SerializeField] private int maxY = 4;

        void Start()
        {
            /*
            var x = transform.position.x + Random.Range(minX, maxX);
            var y = transform.position.y + Random.Range(minY, maxY);
            transform.position = new Vector3(x, y, transform.position.z);*/
        }
    }
}