using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace HackedDesign.UI
{
    public class InventoryPresenter : AbstractPresenter
    {

        [SerializeField] private InvHoverPresenter hoverPanel;
        [SerializeField] private DragPresenter dragPanel;
        [SerializeField] private List<UnityEngine.UI.Image> icons;
        [SerializeField] private Sprite noItemIcon;
        [SerializeField] private UnityEngine.UI.Text scrapAmountText;

        private MechPosition selectedSlot;
        private MechPosition dragSlot;


        public override void Repaint()
        {
            scrapAmountText.text = Game.Instance.Player.Mech.Data.scrap.ToString() + "£";
            UpdateWeaponIcons(Game.Instance.Player.Mech);
        }

        public void HealClick()
        {
            Debug.Log("Heal");
            Game.Instance.Player.Mech.MaxHeal();
        }

        public void ScrapClick()
        {
            Debug.Log("Scrap click");
            if (selectedSlot != MechPosition.Nothing)
            {
                var item = Game.Instance.Player.Mech.GetItem(selectedSlot);
                Debug.Log("Scrap amount: " + item.scrapAmount);
            }
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

        public void RightArmSelected()
        {
            selectedSlot = MechPosition.RightArm;
            ShowItem(selectedSlot);
        }
        public void LeftArmSelected()
        {
            selectedSlot = MechPosition.LeftArm;
            ShowItem(selectedSlot);
        }
        public void RightShoulderSelected()
        {
            selectedSlot = MechPosition.RightShoulder;
            ShowItem(selectedSlot);
        }
        public void LeftShoulderSelected()
        {
            selectedSlot = MechPosition.LeftShoulder;
            ShowItem(selectedSlot);
        }
        public void NoseSelected()
        {
            selectedSlot = MechPosition.Nose;
            ShowItem(selectedSlot);
        }
        public void MotorSelected()
        {
            selectedSlot = MechPosition.Motor;
            ShowItem(selectedSlot);
        }
        public void ArmourSelected()
        {
            selectedSlot = MechPosition.Armour;
            ShowItem(selectedSlot);
        }

        public void RadarSelected()
        {
            selectedSlot = MechPosition.Radar;
            ShowItem(selectedSlot);
        }

        public void InvSlot0Selected()
        {
            selectedSlot = MechPosition.InvSlot0;
            ShowItem(selectedSlot);
        }

        public void InvSlot1Selected()
        {
            selectedSlot = MechPosition.InvSlot1;
            ShowItem(selectedSlot);
        }

        public void InvSlot2Selected()
        {
            selectedSlot = MechPosition.InvSlot2;
            ShowItem(selectedSlot);
        }

        public void InvSlot3Selected()
        {
            selectedSlot = MechPosition.InvSlot3;
            ShowItem(selectedSlot);
        }

        public void InvSlot4Selected()
        {
            selectedSlot = MechPosition.InvSlot4;
            ShowItem(selectedSlot);
        }

        public void InvSlot5Selected()
        {
            selectedSlot = MechPosition.InvSlot5;
            ShowItem(selectedSlot);
        }

        public void InvSlot6Selected()
        {
            selectedSlot = MechPosition.InvSlot6;
            ShowItem(selectedSlot);
        }

        public void InvSlot7Selected()
        {
            selectedSlot = MechPosition.InvSlot7;
            ShowItem(selectedSlot);
        }                                                        

        private void ShowItem(MechPosition position)
        {
            hoverPanel.ShowItem(Game.Instance.Player.Mech.GetItem(position), position);

        }


        public void RightArmMouseOver() => ShowItem(MechPosition.RightArm);
        public void LeftArmMouseOver() => ShowItem(MechPosition.LeftArm);
        public void RightShoulderMouseOver() => ShowItem(MechPosition.RightShoulder);
        public void LeftShoulderMouseOver() => ShowItem(MechPosition.LeftShoulder);
        public void NoseMouseOver() => ShowItem(MechPosition.Nose);
        public void MotorMouseOver() => ShowItem(MechPosition.Motor);
        public void ArmourMouseOver() => ShowItem(MechPosition.Armour);
        public void RadarMouseOver() => ShowItem(MechPosition.Radar);
        public void InvSlot0MouseOver() => ShowItem(MechPosition.InvSlot0);
        public void InvSlot1MouseOver() => ShowItem(MechPosition.InvSlot1);
        public void InvSlot2MouseOver() => ShowItem(MechPosition.InvSlot2);
        public void InvSlot3MouseOver() => ShowItem(MechPosition.InvSlot3);
        public void InvSlot4MouseOver() => ShowItem(MechPosition.InvSlot4);
        public void InvSlot5MouseOver() => ShowItem(MechPosition.InvSlot5);
        public void InvSlot6MouseOver() => ShowItem(MechPosition.InvSlot6);
        public void InvSlot7MouseOver() => ShowItem(MechPosition.InvSlot7);


        public void RightArmMouseOut() => hoverPanel.Hide();
        public void LeftArmMouseOut() => hoverPanel.Hide();
        public void RightShoulderMouseOut() => hoverPanel.Hide();
        public void LeftShoulderMouseOut() => hoverPanel.Hide();
        public void NoseMouseOut() => hoverPanel.Hide();
        public void MotorMouseOut() => hoverPanel.Hide();
        public void ArmourMouseOut() => hoverPanel.Hide();
        public void RadarMouseOut() => hoverPanel.Hide();
        public void InvSlot0MouseOut() => hoverPanel.Hide();
        public void InvSlot1MouseOut() => hoverPanel.Hide();
        public void InvSlot2MouseOut() => hoverPanel.Hide();
        public void InvSlot3MouseOut() => hoverPanel.Hide();
        public void InvSlot4MouseOut() => hoverPanel.Hide();
        public void InvSlot5MouseOut() => hoverPanel.Hide();
        public void InvSlot6MouseOut() => hoverPanel.Hide();
        public void InvSlot7MouseOut() => hoverPanel.Hide();

        public void RightArmDrag() => StartDrag(MechPosition.RightArm);
        public void LeftArmDrag() => StartDrag(MechPosition.LeftArm);
        public void RightShoulderDrag() => StartDrag(MechPosition.RightShoulder);
        public void LeftShoulderDrag() => StartDrag(MechPosition.LeftShoulder);
        public void NoseDrag() => StartDrag(MechPosition.Nose);
        public void MotorDrag() => StartDrag(MechPosition.Motor);
        public void ArmourDrag() => StartDrag(MechPosition.Armour);
        public void RadarDrag() => StartDrag(MechPosition.Radar);
        public void InvSlot0Drag() => StartDrag(MechPosition.InvSlot0);
        public void InvSlot1Drag() => StartDrag(MechPosition.InvSlot1);
        public void InvSlot2Drag() => StartDrag(MechPosition.InvSlot2);
        public void InvSlot3Drag() => StartDrag(MechPosition.InvSlot3);
        public void InvSlot4Drag() => StartDrag(MechPosition.InvSlot4);
        public void InvSlot5Drag() => StartDrag(MechPosition.InvSlot5);
        public void InvSlot6Drag() => StartDrag(MechPosition.InvSlot6);
        public void InvSlot7Drag() => StartDrag(MechPosition.InvSlot7);

        public void StartDrag(MechPosition position)
        {
            dragSlot = position;
            // FIXME: Check for nulls
            var item = Game.Instance.Player.Mech.GetItem(dragSlot);
            if (item != null && item.sprite != null)
            {
                dragPanel.SetSprite(item.sprite);
                dragPanel.Show();
                dragPanel.Repaint();
            }

        }

        public void EndDrag()
        {
            dragPanel.Hide();
        }

        public void Drop(MechPosition destination)
        {
            Game.Instance.Player.Mech.SwapItemPositions(dragSlot, destination);
            hoverPanel.Hide();
            EndDrag();
        }

        public void RightArmDrop() => Drop(MechPosition.RightArm);
        public void LeftArmDrop() => Drop(MechPosition.LeftArm);
        public void RightShoulderDrop() => Drop(MechPosition.RightShoulder);
        public void LeftShoulderDrop() => Drop(MechPosition.LeftShoulder);
        public void NoseDrop() => Drop(MechPosition.Nose);
        public void MotorDrop() => Drop(MechPosition.Motor);
        public void ArmourDrop() => Drop(MechPosition.Armour);
        public void RadarDrop() => Drop(MechPosition.Radar);
        public void InvSlot0Drop() => Drop(MechPosition.InvSlot0);
        public void InvSlot1Drop() => Drop(MechPosition.InvSlot1);
        public void InvSlot2Drop() => Drop(MechPosition.InvSlot2);
        public void InvSlot3Drop() => Drop(MechPosition.InvSlot3);
        public void InvSlot4Drop() => Drop(MechPosition.InvSlot4);
        public void InvSlot5Drop() => Drop(MechPosition.InvSlot5);
        public void InvSlot6Drop() => Drop(MechPosition.InvSlot6);
        public void InvSlot7Drop() => Drop(MechPosition.InvSlot7);

        public void ScrapDrop()
        {
            Debug.Log("Scrapdrop " + dragSlot);
            var item = Game.Instance.Player.Mech.GetItem(dragSlot);
            if (item != null)
            {
                Debug.Log("Scrap amount: " + item.scrapAmount);
                Game.Instance.Player.Mech.Data.scrap += item.scrapAmount;
                Game.Instance.Player.Mech.SetItem(dragSlot, null);

            }
            hoverPanel.Hide();
            EndDrag();
        }
    }
}