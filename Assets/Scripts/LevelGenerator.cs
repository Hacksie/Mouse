
using DunGen;
using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] RuntimeDungeon runtimeDungeon;

        void Start()
        {
            Reset();
            Generate(0);
        
            
        }

        public void Reset()
        {
            runtimeDungeon.Generator.Clear(true);
            
        }

        public void Generate(int level)
        {
            runtimeDungeon.Generate();
        }
    }
}