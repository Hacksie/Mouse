using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "Mouse/Settings/Weapon")]
    public class WeaponSettings : ScriptableObject
    {
        public Sprite icon;
        public int minKineticLevel = 0;
        public string longDescription;
        public int minShootDamage = 10;
        public int maxShootDamage = 100;
        public int minMeleeDamage = 10;
        public int maxMeleeDamage = 100;
    }
}
