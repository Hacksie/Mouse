using UnityEngine;

namespace HackedDesign
{
    public class LineOfSight: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private LayerMask losMask;
        private PlayerController player;

        private void Start()
        {
            this.AutoBind(ref sprite);
            this.player = Game.Instance.Player;
        }

        private void Update() => sprite.enabled = !Physics2D.Linecast(this.transform.position, this.player.transform.position, losMask);
    }
}
