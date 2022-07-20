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
                var newx = (pos.x / Screen.currentResolution.width) * 1920;
                var newy = (pos.y / Screen.currentResolution.height) * 1080;
                //transform.localPosition = pos;
                rect.anchoredPosition = new Vector2(newx, newy);
                //transform.localPosition = pos;
                //rect.anchoredPosition = pos;
            }
        }

        public void SetSprite(Sprite sprite)
        {
            dragImage.sprite = sprite;
        }
    }
}