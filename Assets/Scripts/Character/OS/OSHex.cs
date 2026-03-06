using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HackedDesign
{
    public class OSHex: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer borderSprite;
        [SerializeField] private SpriteRenderer hackSprite;

        [SerializeField] private List<Sprite> hackSprites;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
