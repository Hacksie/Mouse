

using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Speaker", menuName = "Mouse/Dialog/Speaker")]
    public class Speaker : ScriptableObject
    {
        public List<Sprite> sprites;
    }
}
