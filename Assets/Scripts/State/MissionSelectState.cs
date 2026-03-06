using HackedDesign.UI;
using UnityEngine;

namespace HackedDesign
{
    public class MissionSelectState : IState
    {
        private readonly IPresenter missionSelectPresenter;
        
        public bool PlayerActionAllowed => true;
        public bool Battle => true;


        public MissionSelectState(IPresenter missionSelectPresenter)
        {
            this.missionSelectPresenter = missionSelectPresenter;
        }

        public void Begin()
        {
            missionSelectPresenter.Show();
            missionSelectPresenter.Repaint();
            //GameObject.FindGameObjectWithTag("Respawn").transform.position = player.transform.position; // FIXME:
        }

        public void End()
        {
            missionSelectPresenter.Hide();
        }

        public void Update()
        {
 
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {
            
        }

        public void Menu()
        {
            
        }

        public void Select()
        {

        }
    }
}