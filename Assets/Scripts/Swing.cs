using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] float min = 100;
    [SerializeField] float max = 260;
    [SerializeField] float speed = 1;

    private int direction = 1;
    private float currentRotation = 180;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation += direction * speed * Time.deltaTime;
        if(currentRotation > max || currentRotation < min)
        {
            direction *= -1;
        };

        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
