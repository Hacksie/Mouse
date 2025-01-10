using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class CharacterData
    {
        //[SerializeField] public Action changeActions;
        //[SerializeField] public Action dieActions;
        //[SerializeField] public Action hitActions;
        //[SerializeField] public CharacterSettings settings;

        private List<OSTask> puTaskList = new List<OSTask>();
        public List<OSTask> PuTaskList { get => puTaskList; set => puTaskList = value; }
        public string saveName;
        public int health = 0;
        public int ammo = 0;
        public int ram = 100;
        public int maxRam = 100;
        public bool infAmmo = false;
        public bool infHealth = false;

        public Dictionary<int, int> hacks = new Dictionary<int, int>();
        //public string[] hacks = new string[6];

        //public int Health { 
        //    get => this.health;
        //    set {
        //        // FIXME: Settings.maxhealth
        //        this.health = Mathf.Clamp(value, 0, settings.maxHealth);
        //        changeActions?.Invoke();
        //        if (health <= 0)
        //        {
        //            dieActions?.Invoke();
        //        }
        //        else
        //        {
        //            hitActions?.Invoke();
        //        }
        //    } 
        //}

        //public int Ammo { get => this.ammo; set { this.ammo = value; changeActions.Invoke(); } }

        //public bool HasAmmo { get => this.infAmmo || this.ammo > 0; }

        //private void Start()
        //{
        //    Reset();
        //}

        //public void DecreaseAmmo()
        //{
        //    Ammo = Mathf.Max(Ammo - 1, 0);
        //}

        //public void DecreaseHealth(int amount)
        //{
        //    Health = Mathf.Max(Health - amount, 0);
        //}

        //public void Reset()
        //{
        //    health = settings.health;
        //    ammo = settings.ammo;
        //    infAmmo = settings.infiniteAmmo;
        //    infHealth = settings.infiniteHealth;

        //    //for (int i = 0; i < hacks.Length; i++)
        //    //{
        //    //    hacks[i] = "";
        //    //}
        //    changeActions?.Invoke();
        //}

        //public float GetRamUsage()
        //{
        //    float total = 0;
        //    /*
        //    foreach (var task in puTaskList)
        //    {
        //        total += task.Amount;
        //    }*/

        //    return total;
        //}
    }

    public class OSTask

    {
        private readonly string name;
        private readonly float amount;

        public OSTask(string name, float amount)
        {
            this.name = name;
            this.amount = amount;
        }

        public float Amount => amount;

        public string Name => name;
    }

}
