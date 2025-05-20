using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class OperatingSystem : MonoBehaviour
    {
        [SerializeField] public Action changeActions;
        [SerializeField] public Action dieActions;
        [SerializeField] public Action hitActions;
        private CharacterData characterData = new();
        //[SerializeField] public CharacterSettings settings;

        [SerializeField] private List<Hack> activeHacks = new();
        [SerializeField] private List<Hack> repoHacks = new();

        private float momentum = 0;
        //private int currentSlot = 0;

        //public float MomentumFactor { get; set; }

        public float PingRadius { get => characterData.pingRadius;  }

        public List<Hack> ActiveHacks { get => activeHacks; }
        public List<Hack> RepoHacks { get => repoHacks; set => repoHacks = value; }

        public bool HasPistol { get => characterData.hasPistol; }

        public int Health
        {
            get => this.characterData.health;
            set
            {
                this.characterData.health = Mathf.Clamp(value, 0, characterData.maxHealth);
                changeActions?.Invoke();
                if (characterData.health <= 0)
                {
                    dieActions?.Invoke();
                }
                else
                {
                    hitActions?.Invoke();
                }
            }
        }

        public int CurrentMission { get => this.characterData.currentMission; set => this.characterData.currentMission = value; }
        public int KineticLevel { get => this.characterData.kinetic; }
        public int DigitalLevel { get => this.characterData.digital; }
        public int StealthLevel {  get => this.characterData.stealth; }

        //public float MomentumFactor { get => this.characterData.momentumFactor; }

        //public float Momentum { get { return this.momentum; } set { this.momentum = Mathf.Clamp(value, 0, 5); this.changeActions.Invoke(); } }

        public WeaponSettings CurrentWeapon {  get => this.characterData.currentWeapon; }

        public OSTab CurrentTab { get => this.characterData.currentTab; set { this.characterData.currentTab = value; changeActions?.Invoke(); } }

        public int Ammo { get => this.characterData.ammo; set { this.characterData.ammo = value; changeActions?.Invoke(); } }

        private void Awake()
        {
            this.AutoBind(ref characterData);
        }

        public void Reset(CharacterSettings settings)
        {
            characterData.Reset(settings);
            changeActions?.Invoke();
        }

        public void Trigger(int slot)
        {
            Debug.Log("os trigger " + slot);

            /*
            if (activeHacks.Count > slot)
            {
                gameData.AddTask(new OSTask(activeHacks[slot].name, activeHacks[slot].puUsage));
                activeHacks[slot].Trigger(null);
            }*/
        }

        public float GetRamUsage()
        {
            float total = 0;
            /*
            foreach (var task in puTaskList)
            {
                total += task.Amount;
            }*/

            return total;
        }

        public void DecreaseHealth(int amount)
        {
            Health = Mathf.Max(Health - amount, 0);
        }

        public bool HasAmmo { get => this.characterData.infiniteAmmo || this.characterData.ammo > 0; }

        public void DecreaseAmmo(int amount = 1)
        {
            Ammo = Mathf.Max(Ammo - amount, 0);
        } 

    }
}

