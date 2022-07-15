using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign.UI
{
    public class DragPresenter : AbstractPresenter
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] UnityEngine.UI.Image dragImage;

        private InputAction mousePosAction;

        private RectTransform rect;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            mousePosAction = playerInput.actions["Mouse Position"];
        }

        public override void Repaint()
        {
            if (gameObject.activeInHierarchy)
            {
                var pos = mousePosAction.ReadValue<Vector2>();
                //transform.localPosition = pos;
                rect.anchoredPosition = pos;
            }
        }

        public void SetSprite(Sprite sprite)
        {
            dragImage.sprite = sprite;
        }
    }
}