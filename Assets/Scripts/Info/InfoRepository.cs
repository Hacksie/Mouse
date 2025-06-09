
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class InfoRepository : AutoSingleton<InfoRepository>
    {
        public List<WeaponSettings> weapons;
        public List<Faction> factions;
        public List<Corp> corps;
        public List<Actor> actors;

        public WeaponSettings GetWeaponByName(string name) => weapons.FirstOrDefault(w => w.name == name);
    }

    [CreateAssetMenu(fileName = "Faction", menuName = "Mouse/Info/Faction")]
    public class Faction : ScriptableObject
    {
        public string description;
        public Sprite icon;
    }

    [CreateAssetMenu(fileName = "Corp", menuName = "Mouse/Info/Corp")]
    public class Corp : ScriptableObject
    {
        public string description;
        public Sprite icon;
    }

    [CreateAssetMenu(fileName = "Actor", menuName = "Mouse/Info/Actor")]
    public class Actor :  ScriptableObject
    {
        public string description;
        public Sprite icon;
    }

    public enum MissionType
    {
        Extraction,
        Infiltrate,
        Ghost,
        HitAndRun,
        Bedlam,
    }
}
