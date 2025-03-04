using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Ping", menuName = "Mouse/PU/Ping")]
    public class Ping : Hack
    {
        public override void Trigger(GameObject target)
        {
            var results = Physics2D.OverlapCircleAll(Game.Instance.Player.transform.position, Game.Instance.Player.Character.OperatingSystem.PingRadius);

            foreach (var result in results)
            {
                if ((result.CompareTag("Interactable") || result.CompareTag("Player") || result.CompareTag("Enemy")))
                {
                    if (result.TryGetComponent<Interactable>(out var hl)) {
                        hl.Ping();
                    }
                }
            }
        }
    }
}
