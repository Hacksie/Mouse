using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class RandomSkin: MonoBehaviour
    {
        [SerializeField] private List<Material> skins = new List<Material>();
        [SerializeField] private SpriteRenderer sprite;

        void Awake()
        {
            this.AutoBind(ref sprite);
        }

        void Start()
        {
            var idx = Random.Range(0, skins.Count);
            sprite.material = skins[idx];
        }
    }
}
