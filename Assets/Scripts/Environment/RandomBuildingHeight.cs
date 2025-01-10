using UnityEngine;

public class RandomBuildingHeight : MonoBehaviour
{
    [SerializeField] private int minX = -4;
    [SerializeField] private int maxX = 4;
    [SerializeField] private int minY = -4;
    [SerializeField] private int maxY = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var x = transform.position.x + Random.Range(minX, maxX);
        var y = transform.position.y + Random.Range(minY, maxY);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
