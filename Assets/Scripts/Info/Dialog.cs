

using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Dialog : ScriptableObject
    {
        public List<Page> pages;
    }

    [System.Serializable]
    public class Page
    {
        public string speaker;
        public string text;
    }
}
