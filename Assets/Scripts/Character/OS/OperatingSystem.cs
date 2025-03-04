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
        //[SerializeField] private GameDataRepository gameData;
        private CharacterData characterData = new CharacterData();
        [SerializeField] public CharacterSettings settings;

        [SerializeField] private List<Hack> activeHacks = new List<Hack>();
        [SerializeField] private List<Hack> repoHacks = new List<Hack>();
        //private int currentSlot = 0;

        //public int health = 0;
        //public int ammo = 0;
        //public int ram = 100;
        //public bool infAmmo = false;
        //public bool infHealth = false;

        public float PingRadius { get => characterData.pingRadius;  }

        public List<Hack> ActiveHacks { get => activeHacks; }
        public List<Hack> RepoHacks { get => repoHacks; set => repoHacks = value; }

        public int Health
        {
            get => this.characterData.health;
            set
            {
                // FIXME: Settings.maxhealth
                this.characterData.health = Mathf.Clamp(value, 0, settings.maxHealth);
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

        public int KineticLevel { get => this.characterData.kinetic; }
        public int DigitalLevel { get => this.characterData.digital; }
        public int StealthLevel {  get => this.characterData.stealth; }

        public WeaponSettings CurrentWeapon {  get => this.characterData.currentWeapon; }

        public OSTab CurrentTab { get => this.characterData.currentTab; set { this.characterData.currentTab = value; changeActions.Invoke(); } }

        public int Ammo { get => this.characterData.ammo; set { this.characterData.ammo = value; changeActions.Invoke(); } }

        private void Awake()
        {
            this.AutoBind(ref characterData);
        }

        public void Reset()
        {
            characterData.health = settings.health;
            characterData.ammo = settings.ammo;
            characterData.infAmmo = settings.infiniteAmmo;
            characterData.infHealth = settings.infiniteHealth;
            characterData.currentWeapon = settings.weaponSettings.Count > 0 ? settings.weaponSettings[0] : null;

            //for (int i = 0; i < hacks.Length; i++)
            //{
            //    hacks[i] = "";
            //}
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

        public bool HasAmmo { get => this.characterData.infAmmo || this.characterData.ammo > 0; }

        public void DecreaseAmmo()
        {
            Ammo = Mathf.Max(Ammo - 1, 0);
        }

    }
}

