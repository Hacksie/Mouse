
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace HackedDesign
{
    public class ElevatorManager: AutoSingleton<ElevatorManager>
    {
        [SerializeField] private PlayerController player;
        public List<GameObject> elevators;

        public void Refresh() => elevators = GameObject.FindGameObjectsWithTag("Elevator").OrderBy(s => s.transform.position.y).ToList();

        public void GoToFloor(int floor)
        {
            if(floor <=0)
            {
                return;
            }

            if(elevators.Count < floor)
            {
                return;
            }

            player.Teleport(elevators[floor - 1].transform.position);
        }
    }
}
