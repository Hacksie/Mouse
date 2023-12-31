﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign.Story
{
    class ActionInvoker : MonoBehaviour
    {
        public string action = "";

        private void Start()
        {
            if (string.IsNullOrEmpty(action))
            {
                Logger.LogWarning(name, "Action is empty");
            }
        }

        public void Invoke()
        {
            Logger.Log(name, "Invoke ", action);
            SceneManager.Instance.Invoke(action);
        }
    }
}
