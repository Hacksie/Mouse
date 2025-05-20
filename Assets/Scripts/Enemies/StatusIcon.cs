using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class StatusIcon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Sprite alertSprite;
        [SerializeField] private Sprite searchingSprite;
        [SerializeField] private Sprite pickupSprite;

        void Awake()
        {
            this.AutoBind(ref sprite);
            Hide();
        }

        public void Hide()
        {
            sprite.sprite = null;
        }

        public void Alert()
        {
            sprite.sprite = alertSprite;
        }

        public void Searching()
        {
            sprite.sprite = searchingSprite;
        }

        public void Pickup()
        {
            sprite.sprite = pickupSprite;
        }

    }
}
