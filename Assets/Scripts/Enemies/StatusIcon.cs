#nullable enable
using UnityEngine;

namespace HackedDesign
{
    public class StatusIcon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer? sprite;
        [SerializeField] private Sprite? alertSprite;
        [SerializeField] private Sprite? searchingSprite;
        [SerializeField] private Sprite? pickupSprite;

        void Awake()
        {
            this.AutoBind(ref sprite);
            sprite.Require(nameof(sprite));
            Hide();
        }

        public void Hide() => sprite!.sprite = null;

        public void Alert() => sprite!.sprite = alertSprite;

        public void Searching() => sprite!.sprite = searchingSprite;

        public void Pickup() => sprite!.sprite = pickupSprite;
    }
}
