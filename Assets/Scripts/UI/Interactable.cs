using EPOOutline;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HackedDesign
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private Outlinable outlinable;
        [SerializeField] private UnityEvent interactAction;

        private bool interact = false;
        private bool target = false;
        private bool ping = false;

        private float pingTimer = 0;

        

        private void Awake()
        {
            
            this.AutoBind(ref outlinable);
             Target(false);
        }

        public void Ping()
        {
            pingTimer = Time.time;
            ping = true;
            //Target(true);

        }

        public void TriggerInteract()
        {
            Debug.Log("Invoke interact");
            interactAction?.Invoke();
        }

        private void Update()
        {
            if (pingTimer + 4 < Time.time)
            {
                ping = false;
            }

            if (interact)
            {
                outlinable.enabled = true;

                if ((Game.Instance.Player.transform.position - this.transform.position).magnitude < 2.5f)
                {
                    outlinable.OutlineParameters.Color = Color.yellow;
                }
                else
                {
                    outlinable.OutlineParameters.Color = Color.red;
                }

                
            }
            else if (ping)
            {
                outlinable.enabled = true;
                outlinable.OutlineParameters.Color = Color.magenta;
            }
            else if (target)
            {
                outlinable.enabled = true;
                outlinable.OutlineParameters.Color = Color.grey;
            }
            else
            {
                outlinable.enabled = false;
            }
        }


        public void Target(bool flag)
        {
            target = flag;
            //outlinable.enabled = flag;
            //outlinable.OutlineParameters.Color = Color.white;
        }

        public void Interact(bool flag)
        {
            interact = flag;
            //outlinable.enabled = flag;
            //outlinable.OutlineParameters.Color = Color.yellow;
        }
    }
}