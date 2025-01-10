using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Ping", menuName = "Mouse/PU/Ping")]
    public class Ping : Hack
    {
        public override void Trigger(GameObject target)
        {
            Game.Instance.Player.Ping();
        }


    }
}
