using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Mouse/Data/GameData")]
    public class GameData : ScriptableObject 
    {
        public Action changeActions;
        [SerializeField] private int health = 0;
        [SerializeField] private int ammo = 0;

        public int Health { get => health; set { health = value; changeActions.Invoke(); } }

        public int Ammo { get => ammo; set { ammo = value; changeActions.Invoke(); } }

        public void Reset()
        {
            Health = 100;
            Ammo = 0;
        }
    }
}
