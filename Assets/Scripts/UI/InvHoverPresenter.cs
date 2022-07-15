using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign.UI
{
    public class InvHoverPresenter : AbstractPresenter
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private UnityEngine.UI.Text itemName;
        [SerializeField] private UnityEngine.UI.Text itemType;
        [SerializeField] private UnityEngine.UI.Text item1Value;
        [SerializeField] private UnityEngine.UI.Text item2Value;
        [SerializeField] private UnityEngine.UI.Text item3Value;
        [SerializeField] private UnityEngine.UI.Text itemPosition;
        [SerializeField] private UnityEngine.UI.Text itemScrap;
        [SerializeField] private Color dmgColor;
        [SerializeField] private Color apsColor;
        [SerializeField] private Color heatColor;
        [SerializeField] private Color typeColor;
        [SerializeField] private Color labelColor;
        private InputAction mousePosAction;

        private RectTransform rect;
        private InventoryItem currentItem;
        private MechPosition position;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            //mousePosAction = playerInput.actions["Mouse Position"];
        }

        public void ShowItem(InventoryItem item, MechPosition position)
        {
            if (item != null)
            {
                SetCurrentItem(item, position);
                Repaint();
                Show();

            }
            else
            {
                Hide();
            }
        }

        public void SetCurrentItem(InventoryItem item, MechPosition position)
        {
            this.position = position;
            this.currentItem = item;
        }

        public void SetPosition(Vector2 position)
        {
            rect.anchoredPosition = position;
        }

        public override void Repaint()
        {
            itemName.text = currentItem != null ? currentItem.name : "";
            itemPosition.text = "(" + (currentItem != null ? this.position.ToString() : "") + ")";
            itemScrap.text = currentItem != null ? currentItem.scrapAmount + "£" : "";
            ShowItem();
        }

        public void ShowItem()
        {
            switch (currentItem.itemType)
            {
                case ItemType.Weapon:
                    ShowWeapon();
                    break;
                case ItemType.Scrap:
                    ShowScrap();
                    break;
                case ItemType.Armour:
                    ShowArmour();
                    break;
                case ItemType.Motor:
                    ShowMotor();
                    break;
                case ItemType.Radar:
                    ShowRadar();
                    break;
            }
        }

        public void ShowWeapon()
        {
            itemType.text = "<color='#" + ColorUtility.ToHtmlStringRGB(labelColor) + "'>Weapon - </color><color='#" + ColorUtility.ToHtmlStringRGB(typeColor) + "'>" + currentItem.weaponType.ToString() + "</color>";
            item1Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(dmgColor) + "'>" + currentItem.baseMinDamage.ToString("N1") + " - " + currentItem.baseMaxDamage.ToString("N1") + "</color><color='#" + ColorUtility.ToHtmlStringRGB(labelColor) + "'> DMG</color>";
            item2Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(apsColor) + "'>" + (1 / currentItem.baseFireRate).ToString("N1") + "</color><color='#" + ColorUtility.ToHtmlStringRGB(labelColor) + "'> ATK/s</color>";
            item3Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(heatColor) + "'>+" + currentItem.baseHeat.ToString("N1") + "</color><color='#" + ColorUtility.ToHtmlStringRGB(labelColor) + "'> HEAT</color>";
        }
        public void ShowScrap()
        {
            itemType.text = "Scrap";
            item1Value.text = "";
            item2Value.text = "";
            item3Value.text = "";
        }
        public void ShowArmour()
        {
            itemType.text = "Armour";
            item1Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(typeColor) + "'>+" + currentItem.baseArmour.ToString("N2") + " armour</color>";
            item2Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(typeColor) + "'>+" + currentItem.baseArmourRegen.ToString("N2") + " Arm/s</color>";
            item3Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(typeColor) + "'>+" + currentItem.baseShield.ToString("N2") + " shield</color>";;
        }
        public void ShowMotor()
        {
            itemType.text = "Motor";
            item1Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(typeColor) + "'>+" + currentItem.baseSpeed.ToString("N1") + " walk speed</color>";
            item2Value.text = "";
            item3Value.text = "";
        }
        public void ShowRadar()
        {
            itemType.text = "Radar";
            item1Value.text = "<color='#" + ColorUtility.ToHtmlStringRGB(typeColor) + "'>+" + currentItem.baseRange.ToString("N1") + " range</color>";;
            item2Value.text = "";
            item3Value.text = "";
        }


        public void UpdateHoverLocation()
        {
            var pos = mousePosAction.ReadValue<Vector2>();
            //transform.localPosition = pos;
            rect.anchoredPosition = pos;

        }


    }
}