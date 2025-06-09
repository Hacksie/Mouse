

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Speaker", menuName = "Mouse/Dialog/Speaker")]
    public class Speaker : ScriptableObject
    {
        public List<Sprite> sprites;

        public Sprite GetEmotion(Emotion emotion) => sprites[(int)emotion];

        public Sprite GetEmotion(string emotion) => Enum.TryParse<Emotion>(emotion, out var emotionEnum) ? GetEmotion(emotionEnum) : GetEmotion(Emotion.Default);
    }

    public enum Emotion
    {
        Default,
        Blink,
        Confused,
        Upset,
        Angry,
        Furious,
        Happy,
        Glee
    }
}
