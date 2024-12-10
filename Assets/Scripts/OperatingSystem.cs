
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "OperatingSystem", menuName = "Mouse/OperatingSystem")]
    public class OperatingSystem : ScriptableObject
    {
        [SerializeField] private Queue<string> commandBuffer = new Queue<string>();

        public void ExecuteCommand(string command, GameObject target)
        {
            commandBuffer.Enqueue(command);
        }
    }
}