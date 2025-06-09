using UnityEngine;

namespace HackedDesign
{
    public class RandomBuildingHeight : MonoBehaviour
    {
        //[SerializeField] private int minX = -4;
        //[SerializeField] private int maxX = 4;
        [SerializeField] private int minY = -4;
        [SerializeField] private int maxY = 4;

        void Start()
        {
           
            float x = transform.position.x;
            //if (Game.Instance.GameSettings.BuildingGapChance >= Random.value)
            //{
            //    Debug.Log("Building gap", this);
            //    x += 8; // Random.Range(minX, maxX + 1);
            //}
            var y = transform.position.y + Random.Range(minY, maxY + 1);
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}