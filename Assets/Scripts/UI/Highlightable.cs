using EPOOutline;
using UnityEngine;

namespace HackedDesign
{
    public class Highlightable : MonoBehaviour
    {
        [SerializeField] private Outlinable outlinable;

        private float pingTimer = 0;

        private void Awake()
        {
            outlinable = GetComponent<Outlinable>();
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
    }
}