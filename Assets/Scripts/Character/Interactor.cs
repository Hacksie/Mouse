

using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private new Collider2D collider2D;

        private readonly List<Interactable> currentInteractables = new();


        private void Awake()
        {
            this.AutoBind(ref collider2D);
        }

        public void TriggerInteract()
        {
            Debug.Log("trigger interact", this);
            if(currentInteractables.Count > 0)
            {
                currentInteractables[0].TriggerInteract();
            }
        }

        public void UpdateInteractors()
        {
            foreach(var interactable in currentInteractables)
            {
                interactable.Interact(false);
            }
            currentInteractables.Clear();

            List<Collider2D> contacts = new List<Collider2D>();

            collider2D.GetContacts(contacts);

            foreach (var contact in contacts)
            {
                if(contact.CompareTag("Player"))
                {
                    continue;
                }

                if(contact.TryGetComponent<Interactable>(out var interactable))
                {
                    interactable.Interact(true);
                    currentInteractables.Add(interactable);
                }
            }
        }
    }
}
