
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "InfoRepository", menuName = "Mouse/Info/Repository")]
    public class InfoRepository : ScriptableObject
    {
        public List<Dialog> dialogs;
        public List<Faction> factions;
        public List<Corp> corps;
        public List<Actor> actors;
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
