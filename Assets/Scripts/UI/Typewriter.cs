﻿using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using UnityEngine.UI;

namespace HackedDesign
{
    public class Typewriter: MonoBehaviour
    {
        private const string ColorTagEnd = "</color>";

        [SerializeField] private UnityEngine.UI.Text text;
        [SerializeField] private int speed = 30;

        private string origText;
        private CancellationTokenSource cts;
        private Task currentTask;

        public string OriginalText { get => this.origText; set => this.origText = value; }
        public Text Text { get => this.text; set => this.text = value; }

        private void Awake() => OriginalText = Text.text;

        public void Play()
        {
            this.cts?.Cancel();
            this.cts = new CancellationTokenSource();
            this.currentTask = PlayTypewriter(cts.Token);
        }

        public void Play(string text)
        {
            OriginalText = text;
            Play();
        }

        public void Finish()
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();
            Text.text = OriginalText;
        }

        private async Task PlayTypewriter(CancellationToken token)
        {
            int tagLevel = 0;
            Text.text = "";
            // FIXME: Assumes all tags are color tags
            for (int i = 0; i < OriginalText.Length; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(speed, token);
                token.ThrowIfCancellationRequested();

                if (OriginalText[i] == '<')
                {
                    // end tag
                    if (i + 1 < OriginalText.Length)
                    {
                        tagLevel += OriginalText[i + 1] == '/' ? -1 : 1;
                    }

                    i = OriginalText.IndexOf('>', i);

                    if (i == -1)
                    {
                        Text.text = OriginalText;
                        break;
                    }

                    i++;
                }

                if(i < (OriginalText.Length - 1))
                {
                    string lowChar = "<color=\"#006600\">" + OriginalText[i + 1] + ColorTagEnd;

                    Text.text = OriginalText[..i] + lowChar + string.Concat(Enumerable.Repeat(ColorTagEnd, tagLevel));
                }
                else
                {

                    Text.text = OriginalText[..i] + string.Concat(Enumerable.Repeat(ColorTagEnd, tagLevel));
                }
            }

            token.ThrowIfCancellationRequested();
            await Task.Delay(speed, token);
            token.ThrowIfCancellationRequested();

            Text.text = OriginalText;
        }
    }
}

