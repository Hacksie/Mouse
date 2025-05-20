using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class LineOfSight: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private LayerMask losMask;
        private PlayerController player;

        private void Awake()
        {
            //this.AutoBind(ref sprite);
            //player = Game.Instance.Player;
        }

        private void Update()
        {
            sprite.enabled = !Physics2D.Linecast(this.transform.position, this.player.transform.position, losMask);
        }
    }
}
