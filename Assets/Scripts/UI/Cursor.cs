using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HackedDesign.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HackedDesign
{
    public class Cursor : MonoBehaviour
    {
        //[SerializeField] private HoverPresenter hoverPresenter;
        [SerializeField] UnityEngine.UI.Text nametagLabel;
        [SerializeField] private CanvasScaler canvas;
        [SerializeField] private Camera uiCamera;
        [SerializeField] private PlayerInput playerInput = null;
        [SerializeField] private RectTransform uiCrosshair = null;
        [SerializeField] private int screenWidth = 320;
        [SerializeField] private int screenHeight = 180;

        private InputAction mousePosAction;

        //private TilemapHighlight currentTile;

        private Interactable currentHighlightable;

        void Awake()
        {
            canvas = GetComponentInParent<CanvasScaler>();
            uiCrosshair = GetComponent<RectTransform>();
            mousePosAction = playerInput.actions["Mouse Position"];
            //UnityEngine.Cursor.visible = false;
            screenWidth = (int)canvas.referenceResolution.x;
            screenHeight = (int)canvas.referenceResolution.y;
            nametagLabel.text = "";

        }

        void OnApplicationQuit()
        {
            UnityEngine.Cursor.visible = true;
        }

        void Update()
        {
            var mousePos = mousePosAction.ReadValue<Vector2>();
            PositionCrosshair(mousePos);
            //UpdateHover(mousePos);
        }

        private void UpdateHover(Vector2 mousePos)
        {
            

            var worldPos = uiCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

            var collider = Physics2D.OverlapPoint(new Vector2(worldPos.x, worldPos.y));

            if (currentHighlightable)
            {
                currentHighlightable.Show(false);
                nametagLabel.text = "";
            }


            if (collider != null && (collider.CompareTag("Interactable") || collider.CompareTag("Player") || collider.CompareTag("Enemy")))
            {
                nametagLabel.text = collider.name.ToString();
                var highlight = collider.gameObject.GetComponent<Interactable>();
                if (highlight != null)
                {
                    currentHighlightable = highlight;
                    currentHighlightable.Show(true);
                }
            }
        }

        private void PositionCrosshair(Vector2 mousePos)
        {
            uiCrosshair.anchoredPosition = new Vector2(Mathf.FloorToInt(screenWidth * (mousePos.x / Screen.width)), Mathf.FloorToInt(screenHeight * (mousePos.y / Screen.height)));
        }        
    }
}