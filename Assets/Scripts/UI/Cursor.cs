using System;
using System.Collections;
using System.Collections.Generic;
using HackedDesign.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign
{
    public class Cursor : MonoBehaviour
    {
        //[SerializeField] private HoverPresenter hoverPresenter;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private PlayerInput playerInput = null;
        [SerializeField] private RectTransform uiCrosshair = null;
        [SerializeField] private int screenWidth = 320;
        [SerializeField] private int screenHeight = 180;

        private InputAction mousePosAction;

        //private TilemapHighlight currentTile;

        void Awake()
        {
            uiCrosshair = GetComponent<RectTransform>();
            mousePosAction = playerInput.actions["Mouse Position"];
            UnityEngine.Cursor.visible = false;
        }

        void OnApplicationQuit()
        {
            UnityEngine.Cursor.visible = true;
        }

        void Update()
        {
            var mousePos = mousePosAction.ReadValue<Vector2>();
            PositionCrosshair(mousePos);
            UpdateHover(mousePos);
        }

        private void UpdateHover(Vector2 mousePos)
        {
            /*
            var worldPos = uiCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

            var collider = Physics2D.OverlapPoint(new Vector2(worldPos.x, worldPos.y));
            if(collider != null && (collider.CompareTag("Interactable") || collider.CompareTag("Player") || collider.CompareTag("Enemy")))
            {
                var tile = collider.GetComponent<TilemapHighlight>();
                if(currentTile != null)
                {
                    currentTile.Reset();
                    currentTile = null;
                }
                
                if(tile != null)
                {
                    tile.Highlight();
                    currentTile = tile;
                }
                hoverPresenter.SetHoverLabel(collider.name);
                //Debug.Log(collider.name + "mouse over");
            }
            else 
            {
                if(currentTile != null)
                {
                    currentTile.Reset();
                    currentTile = null;
                }                
            }*/


        }

        private void PositionCrosshair(Vector2 mousePos)
        {
            
            uiCrosshair.anchoredPosition = new Vector2(Mathf.FloorToInt(screenWidth * (mousePos.x / Screen.width)), Mathf.FloorToInt(screenHeight * (mousePos.y / Screen.height)));
        }        
    }
}