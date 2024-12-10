using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private DoorDirection direction;
        [SerializeField] private Transform destination;

        public DoorDirection Direction { get => direction; private set => direction = value; }
        public Transform Destination { get => destination; private set => destination = value; }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                var doorUser = other.GetComponent<DoorUser>(); // FIXME: Make this a separate component
                doorUser.AddDoor(this);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                var doorUser = other.GetComponent<DoorUser>(); // FIXME: Make this a separate component
                doorUser.RemoveDoor(this);
            }            
        }
    }

    public enum DoorDirection
    {
        Up,
        Down
    }
}