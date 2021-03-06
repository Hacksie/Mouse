﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

namespace HackedDesign
{
    [RequireComponent(typeof(Collider2D))]
    public class BaseTrigger : MonoBehaviour
    {
        protected new Collider2D collider;
        protected ShadowCaster2D shadow;
        public bool triggerEnabled = true;

        [Header("Settings")]
        public bool autoInteraction = false;
        public bool allowInteraction = false;
        public bool allowBug = false;
        public bool allowHack = false;
        public bool allowOverload = false;
        public bool allowRepeatInteractions = false;
        public bool allowNPCAutoInteraction = false;

        [Header("Actions")]
        public UnityEvent entryActionEvent;
        public UnityEvent interactActionEvent;
        public UnityEvent bugActionEvent;
        public UnityEvent hackActionEvent;
        public UnityEvent overloadActionEvent;
        public UnityEvent leaveActionEvent;

        [Header("State")]
        public bool triggered = false;
        public bool npcTriggered = false;
        public bool overloaded = false;
        public bool hacked = false;
        public bool bugged = false;
        private List<GameObject> colliders = new List<GameObject>();

        private void Start()
        {
            if (!CompareTag(Tags.TRIGGER))
            {
                Logger.LogError(this ,"Trigger is not tagged");
            }
        }

        public virtual void Initialize()
        {
            triggered = false;
            npcTriggered = false;
            collider = GetComponent<Collider2D>();
            shadow = GetComponent<ShadowCaster2D>();

            colliders.Clear();

            if (triggerEnabled)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        public void Activate()
        {
            if (collider != null)
            {
                collider.enabled = true;
            }
            if(shadow != null)
            {
                shadow.enabled = true;
            }
        }

        public void Deactivate()
        {
            if (collider != null)
            {
                collider.enabled = false;
                shadow.enabled = false;
            }
        }

        // public virtual void UpdateTrigger(Input.IInputController inputController)
        // {

        //     for (int i = colliders.Count - 1; i >= 0; i--)
        //     {
        //         if (colliders[i].CompareTag(Tags.NPC) && allowNPCAutoInteraction)
        //         {
        //             Logger.Log(name, "NPC hit door", " ", colliders[i].gameObject.name);
        //             Invoke(colliders[i].gameObject);
        //             colliders.RemoveAt(i);
        //         }

        //         if (colliders[i].CompareTag(Tags.PLAYER))
        //         {
        //             if (CheckPlayerActions(colliders[i].gameObject, inputController))
        //             {
        //                 colliders.RemoveAt(i);
        //             }
        //         }
        //     }
        // }

        public virtual void Entry(GameObject source)
        {
            entryActionEvent.Invoke();
        }

        public virtual void Invoke(GameObject source)
        {
            if (allowInteraction || hacked || bugged || overloaded)
            {
                interactActionEvent.Invoke();
            }
        }

        public virtual void Overload(GameObject source)
        {
            if (!overloaded && !hacked && !bugged && GameManager.Instance.Data.Player.CanOverload&& allowOverload)
            {
                Logger.Log(this, "Overload");
                overloaded = true;

                overloadActionEvent.Invoke();
                GameManager.Instance.Data.Player.ConsumeOverload();
            }
        }

        public virtual void Hack(GameObject source)
        {
            if (!overloaded && !hacked && !bugged && GameManager.Instance.Data.Player.CanHack&& allowHack)
            {
                if (GameManager.Instance.Data.Player.ConsumeHack())
                {
                    hackActionEvent.Invoke();
                    hacked = true;
                }
            }
            else if (hacked)
            {
                interactActionEvent.Invoke();
            }
        }

        public virtual void Bug(GameObject source)
        {
            bugged = true;
            hacked = true;
            bugActionEvent.Invoke();
            GameManager.Instance.Data.Player.ConsumeBug();
        }

        public virtual void Leave(GameObject source)
        {
            leaveActionEvent.Invoke();
        }

        // protected bool CheckPlayerActions(GameObject source, Input.IInputController inputController)
        // {
        //     if (autoInteraction)
        //     {
        //         Invoke(source);
        //         return true;
        //     }

        //     if (inputController.InteractButtonUp() && allowInteraction)
        //     {
        //         Invoke(source);
        //         return true;
        //     }
        //     if (!overloaded && !hacked && !bugged && GameManager.Instance.Data.Player.CanOverload&& inputController.OverloadButtonUp() && allowOverload)
        //     {
        //         Overload(source);
        //         return true;
        //     }
        //     if (!overloaded && !hacked && !bugged && GameManager.Instance.Data.Player.CanBug&& inputController.BugButtonUp() && allowBug)
        //     {
        //         Bug(source);
        //         return true;
        //     }
        //     if (!overloaded && !hacked && !bugged && GameManager.Instance.Data.Player.CanHack&& inputController.HackButtonUp() && allowHack)
        //     {
        //         Hack(source);
        //         return true;
        //     }
        //     if ((overloaded || hacked || bugged) && inputController.InteractButtonUp())
        //     {
        //         Invoke(source);
        //         return true;
        //     }

        //     return false;
        // }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!triggerEnabled)
            {
                return;
            }

            if (other.CompareTag(Tags.PLAYER))
            {
                //FIXME: cache this
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                //playerController.RegisterTrigger(this);
                Entry(other.gameObject);
            }

            if(other.CompareTag(Tags.NPC))
            {
                if(allowNPCAutoInteraction)
                {
                    Invoke(other.gameObject);
                }
            }
        }

        //FIXME: Move input code outside of physics update
        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (!triggerEnabled)
            {
                return;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!triggerEnabled)
            {
                return;
            }

            if (other.CompareTag(Tags.PLAYER))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                //playerController.UnregisterTrigger(this);
                Leave(other.gameObject);
            }

            if (other.CompareTag(Tags.NPC))
            {
                Leave(other.gameObject);
            }
        }
    }
}