using HackedDesign.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    public class DialogManager: MonoBehaviour
    {
        [SerializeField] DialogPresenter presenter;
        [SerializeField] List<Speaker> speakers;
        [SerializeField] List<Dialog> dialogs = new List<Dialog>();
        [SerializeField] List<Dialog> hotdogAdvice = new List<Dialog>();
        public static DialogManager Instance { get; private set; }
        private DialogManager() =>  Instance = this;

        private UnityAction dialogOverCallback;


        public Dialog CurrentDialog { get; set; }
        public List<Speaker> Speakers { get => speakers; set => speakers = value; }

        //public int CurrentPage { get; set; } = 0;

        private void Start()
        {
            presenter.finishedEvent.AddListener(new UnityAction(DialogFinished));
        }

        public Sprite GetSpeakerSprite(Page page)
        {
            return GetSpeakerSprite(page.speaker, page.speakerFrame);
        }

        public Sprite GetSpeakerSprite(string name, int frame)
        {
            return speakers.FirstOrDefault(x => x.name == name).sprites[frame];
        }

        public void SetDialogByName(string name)
        {
            CurrentDialog = dialogs.First(x => x.name == name);
        }

        public void SetHotdogDialogByName(string name)
        {
            CurrentDialog = hotdogAdvice.First(x => x.name == name);
        }

        public void SetRandomHotdogDialog()
        {
            if (hotdogAdvice.Count > 0)
            {
                CurrentDialog = hotdogAdvice[Random.Range(0, hotdogAdvice.Count)];
            }
            else
            {
                CurrentDialog = null;
            }
        }

        public void ShowDialog(string name)
        {
            ShowDialog(name, null);
        }


        public void ShowDialog(string name, UnityAction dialogOverAction)
        {
            Debug.Log("Show Dialog", this);
            this.dialogOverCallback = dialogOverAction;
            //CurrentPage = 0;
            SetDialogByName(name);
            presenter.Show();
            presenter.Repaint();
        }

        public void ShowHotdogDialog(string name)
        {
            ShowHotdogDialog(name, null);
        }

        public void ShowHotdogDialog(string name, UnityAction dialogOverAction)
        {

            this.dialogOverCallback = dialogOverAction;
            //CurrentPage = 0;
            SetHotdogDialogByName(name);
            presenter.Show();
            presenter.Repaint();
        }

        public void DialogFinished()
        {
            presenter.Hide();
            this.dialogOverCallback?.Invoke();
        }
    }
}
