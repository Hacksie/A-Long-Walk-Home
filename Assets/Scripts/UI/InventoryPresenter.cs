using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class InventoryPresenter : AbstractPresenter
    {
        [SerializeField] private List<UnityEngine.UI.Image> icons;
        [SerializeField] private Sprite noItemIcon;

        private MechPosition dragSlot;

        public override void Repaint()
        {
            UpdateWeaponIcons(Game.Instance.Player.Weapons);
        }

        public void CloseClick()
        {
            Game.Instance.SetPlaying();
        }

        private void UpdateWeaponIcons(MechController weapons)
        {
            for (int i = 0; i < icons.Count; i++)
            {
                var item = weapons.GetItem((MechPosition)i);
                icons[i].sprite = item != null ? item.sprite : noItemIcon;
            }
        }

        public void RightArmDrag()
        {
            dragSlot = MechPosition.RightArm;
        }

        public void LeftArmDrag()
        {
            dragSlot = MechPosition.LeftArm;
        }        

        public void RightArmDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.RightArm);
            Game.Instance.Player.Weapons.UpdateWeapons();

            //dragSlot = null;
        }           

        public void LeftArmDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.LeftArm);
            Game.Instance.Player.Weapons.UpdateWeapons();
            //dragSlot = null;
        }

     
    }
}