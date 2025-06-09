

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HackedDesign
{
    //[CreateAssetMenu(fileName = "Dialog", menuName = "Mouse/Dialog/Dialog")]
    //public class Dialog : ScriptableObject
    //{
    //    public List<Page> pages;
    //}

    //[System.Serializable]
    //public class Page
    //{
    //    public string sequence;
    //    public string speaker;
    //    [FormerlySerializedAs("speakerFrame")]
    //    public Emotion emotion;
    //    //public int speakerFrame;
    //    public string text;
    //}

    public class DialogLine
    {
        public string Sequence { get; set; }
        public string Speaker { get; set; }
        public string Emotion { get; set; }
        //public int speakerFrame;
        public string Text { get; set; }
    }
}
