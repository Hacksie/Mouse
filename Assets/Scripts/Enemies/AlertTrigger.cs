
using UnityEngine;

namespace HackedDesign
{
    public class AlertTrigger : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("player seen", this);
            }
        }
    }
}
