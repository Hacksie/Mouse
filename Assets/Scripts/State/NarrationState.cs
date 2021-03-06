using UnityEngine;

namespace HackedDesign
{
    public class NarrationState : IState
    {
        private UI.NarrationPanelPresenter narrationPanel;

        public NarrationState(UI.NarrationPanelPresenter narrationPanel)
        {
            this.narrationPanel = narrationPanel;
        }

        public void Begin()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            this.narrationPanel.Show();
        }

        public void Update()
        {

        }

        public void LateUpdate()
        {
            this.narrationPanel.Repaint();
        }

        public void End()
        {
            this.narrationPanel.Hide();
        }

        public void Interact()
        {
            
        }

        public void Hack()
        {
            
        }

        public void Dash()
        {
            
        }

        public void Overload()
        {
            
        }

        public void Start()
        {
            
        }

        public void Select()
        {
            
        }
    }
}