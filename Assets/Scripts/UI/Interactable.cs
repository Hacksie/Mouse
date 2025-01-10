using EPOOutline;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HackedDesign
{
    [RequireComponent(typeof(EventTrigger))]
    public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Outlinable outlinable;

        private float pingTimer = 0;
        private EventTrigger trigger;

        private void Awake()
        {
            outlinable = GetComponent<Outlinable>();
            trigger = GetComponent<EventTrigger>();
            var onEnterTrigger = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerEnter
            };

            onEnterTrigger.callback.AddListener(x => { Show(true); });

            var onExitTrigger = new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerExit
            };

            onExitTrigger.callback.AddListener(x => { Show(false); });


            trigger.triggers.Add(onEnterTrigger);
            trigger.triggers.Add(onExitTrigger);
            

            Show(false);
        }

        public void Ping()
        {
            pingTimer = Time.time;
            Show(true);

        }

        private void Update()
        {
            /*
            if (Time.time - 3 > pingTimer)
            {
                Show(false);
            }*/
        }

        public void Show(bool flag)
        {
            outlinable.enabled = flag;
            outlinable.OutlineParameters.Color = Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Show(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Show(false);
        }
    }
}