

using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Mouse/Dialog/Dialog")]
    public class Dialog : ScriptableObject
    {
        public List<Page> pages;
    }

    [System.Serializable]
    public class Page
    {
        public string speaker;
        public int speakerFrame;
        public string text;
    }
}
