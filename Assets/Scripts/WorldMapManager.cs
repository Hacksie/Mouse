﻿#nullable enable
using UnityEngine;


//FIXME: Combine this with SceneManager
namespace HackedDesign {
	public class WorldMapManager : MonoBehaviour 
	{
        [SerializeField] private string defaultLocation = "AisanaContractorTower2";
        public string selectedLocation = "";
        public string selectedFloor = "";

        public void Initialize() {
            selectedLocation = defaultLocation;
        }

        public void NextLevel()
        {
            // This should just set a new story state, and that stage should load the level accordingly;
            Story.SceneManager.Instance.CurrentScene.Next();
        }
    }
}

