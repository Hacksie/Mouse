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

        [SerializeField] private List<Hack> activeHacks = new();
        [SerializeField] private List<Hack> repoHacks = new();

        public float PingRadius => this.characterData.pingRadius;

        public List<Hack> ActiveHacks => activeHacks;
        public List<Hack> RepoHacks { get => repoHacks; set => repoHacks = value; }

        public bool HasPistol => characterData.hasPistol;

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
        public int KineticLevel => this.characterData.kinetic;
        public int DigitalLevel => this.characterData.digital;
        public int StealthLevel => this.characterData.stealth;
        public WeaponSettings CurrentWeapon => this.characterData.currentWeapon;

        public OSTab CurrentTab { 
            get => this.characterData.currentTab; 
            set { 
                this.characterData.currentTab = value; 
                changeActions?.Invoke(); 
            } 
        }

        public int Ammo { 
            get => this.characterData.ammo; 
            set
            {
                this.characterData.ammo = value;
                changeActions?.Invoke();
            }
        }

        private void Awake() => this.AutoBind(ref characterData);

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

        public void DecreaseHealth(int amount) => Health = Mathf.Max(Health - amount, 0);

        public bool HasAmmo => this.characterData.infiniteAmmo || this.characterData.ammo > 0;

        public void DecreaseAmmo(int amount = 1) => Ammo = Mathf.Max(Ammo - amount, 0);

    }
}

