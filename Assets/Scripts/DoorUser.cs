using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class DoorUser : MonoBehaviour
    {
        private List<Door> doorList = new List<Door>();

        public void TryGoUpDoor()
        {
            foreach(var door in doorList)
            {
                if(door.Direction == DoorDirection.Up && door.Destination.position != null)
                {
                    //Debug.Log("Go up");
                    transform.position = door.Destination.position;
                    return;
                }
            }

            //Debug.Log("Can't go up");
        }

        public void TryGoDownDoor()
        {
            foreach(var door in doorList)
            {
                if(door.Direction == DoorDirection.Down && door.Destination.position != null)
                {
                    //Debug.Log("Go down");
                    transform.position = door.Destination.position;
                    return;
                }
            }
            //Debug.Log("Can't go down");
        }

        public void AddDoor(Door door)
        {
            //Debug.Log("Add door " + this.name);
            doorList.Add(door);
        }

        public void RemoveDoor(Door door)
        {
            //Debug.Log("Remove door " + this.name);
            doorList.Remove(door);
        }        
    }
}