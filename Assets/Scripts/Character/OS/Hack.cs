using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Hack", menuName = "Mouse/PU/Generic Hack")]
    public abstract class Hack : ScriptableObject
    {
        [SerializeField] public string shortName;
        [SerializeField] public Sprite buttonIcon;
        [SerializeField] public Sprite puIcon;
        [SerializeField] public float puUsage;
        [SerializeField] public float puTime;
        public abstract void Trigger(GameObject target);


    }
}
