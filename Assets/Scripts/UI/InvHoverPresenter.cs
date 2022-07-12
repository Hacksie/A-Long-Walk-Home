using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign.UI
{
    public class InvHoverPresenter : AbstractPresenter
    {
        [SerializeField] private PlayerInput? playerInput;
        private InputAction mousePosAction;

        private RectTransform rect;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            mousePosAction = playerInput.actions["Mouse Position"];
        }

        public override void Repaint()
        {
            UpdateHoverLocation();
        }

        public void UpdateHoverLocation()
        {
            var pos = mousePosAction.ReadValue<Vector2>();
            //transform.localPosition = pos;
            rect.anchoredPosition = pos;

        }        


    }
}