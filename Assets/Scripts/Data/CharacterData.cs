using System.Collections.Generic;

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
        public int maxHealth = 100;
        public int health = 0;
        public int ammo = 0;
        public int ram = 100;
        public int maxRam = 100;
        public int pingRadius = 10;
        public bool infiniteAmmo = false;
        public bool infinityHealth = false;
        public int currentMission = 2;
        public bool hasPistol = true;
        public float momentumFactor = 0.05f;

        public int kinetic = 1;
        public int digital = 1;
        public int stealth = 1;

        public WeaponSettings currentWeapon;

        public OSTab currentTab;


        public Dictionary<int, int> hacks = new Dictionary<int, int>();

        public void Reset(CharacterSettings settings)
        {
            health = settings.startingHealth;
            ammo = settings.startingAmmo;
            infiniteAmmo = settings.infiniteAmmo;
            infinityHealth = settings.infiniteHealth;
            currentWeapon = settings.weaponSettings.Count > 0 ? settings.weaponSettings[0] : null;
            maxHealth = settings.startingHealth;
        }
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

    public enum OSTab
    {
        Character,
        Inventory,
        Shop,
        Music,
        Info
    }

}
