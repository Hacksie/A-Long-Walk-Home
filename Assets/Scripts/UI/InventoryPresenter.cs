using System.Collections.Generic;
using System.Linq;
using UnityEngine;



namespace HackedDesign.UI
{
    public class InventoryPresenter : AbstractPresenter
    {
        
        [SerializeField] private InvHoverPresenter hoverPanel;
        [SerializeField] private List<UnityEngine.UI.Image> icons;
        [SerializeField] private Sprite noItemIcon;

        

        private MechPosition dragSlot;


 
        public override void Repaint()
        {
            UpdateWeaponIcons(Game.Instance.Player.Weapons);
            
        }

        public void ScrapClick()
        {
            Debug.Log("Scrap click");
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

        public void RightArmMouseOver()
        {
            hoverPanel.Show();
            hoverPanel.Repaint();
        }

        public void LeftArmMouseOver()
        {
            hoverPanel.Show();
            hoverPanel.Repaint();
        }

        public void RightShoulderMouseOver()
        {
            hoverPanel.Show();
            hoverPanel.Repaint();
        }

        public void LeftShoulderMouseOver()
        {
            hoverPanel.Show();
            hoverPanel.Repaint();
        }        

        public void NoseMouseOver()
        {
            hoverPanel.Show();
            hoverPanel.Repaint();
        }

        public void RightArmMouseOut()
        {
            hoverPanel.Hide();
        }

        public void LeftArmMouseOut()
        {
            hoverPanel.Hide();
        }        

        public void RightShoulderMouseOut()
        {
            hoverPanel.Hide();
        }           

        public void LeftShoulderMouseOut()
        {
            hoverPanel.Hide();
        }

        public void NoseMouseOut()
        {
            hoverPanel.Hide();
        }        

        public void RightArmDrag()
        {
            dragSlot = MechPosition.RightArm;
        }

        public void LeftArmDrag()
        {
            dragSlot = MechPosition.LeftArm;
        }

        public void RightShoulderDrag()
        {
            dragSlot = MechPosition.RightShoulder;
        }        

        public void LeftShoulderDrag()
        {
            dragSlot = MechPosition.LeftShoulder;
        }

        public void NoseDrag()
        {
            dragSlot = MechPosition.Nose;
        }        

        public void LegsDrag()
        {
            dragSlot = MechPosition.Legs;
        }

        public void BodyDrag()
        {
            dragSlot = MechPosition.Body;
        }        

        public void RadarDrag()
        {
            dragSlot = MechPosition.Radar;
        }        

        public void InvSlot0Drag()
        {
            dragSlot = MechPosition.InvSlot0;
        }        

        public void InvSlot1Drag()
        {
            dragSlot = MechPosition.InvSlot1;
        }        

        public void InvSlot2Drag()
        {
            dragSlot = MechPosition.InvSlot2;
        }        

        public void InvSlot3Drag()
        {
            dragSlot = MechPosition.InvSlot3;
        }      


        public void InvSlot4Drag()
        {
            dragSlot = MechPosition.InvSlot4;
        }        

        public void InvSlot5Drag()
        {
            dragSlot = MechPosition.InvSlot5;
        }        

        public void InvSlot6Drag()
        {
            dragSlot = MechPosition.InvSlot6;
        }        

        public void InvSlot7Drag()
        {
            dragSlot = MechPosition.InvSlot7;
        }                                

        public void RightArmDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.RightArm);
            Game.Instance.Player.Weapons.UpdateModels();
        }

        public void LeftArmDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.LeftArm);
            Game.Instance.Player.Weapons.UpdateModels();
        }

        public void RightShoulderDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.RightShoulder);
            Game.Instance.Player.Weapons.UpdateModels();
        }

        public void LeftShoulderDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.LeftShoulder);
            Game.Instance.Player.Weapons.UpdateModels();
        }

        public void NoseDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.Nose);
            Game.Instance.Player.Weapons.UpdateModels();
        }        

        public void LegsDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.Legs);
            Game.Instance.Player.Weapons.UpdateModels();
        }        

        public void BodyDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.Body);
            Game.Instance.Player.Weapons.UpdateModels();
        }        

        public void RadarDrop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.Radar);
            Game.Instance.Player.Weapons.UpdateModels();
        }        

        public void InvSlot0Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot0);
            Game.Instance.Player.Weapons.UpdateModels();
        }

        public void InvSlot1Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot1);
            Game.Instance.Player.Weapons.UpdateModels();
        }

        public void InvSlot2Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot2);
            Game.Instance.Player.Weapons.UpdateModels();
        }
     
        public void InvSlot3Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot3);
            Game.Instance.Player.Weapons.UpdateModels();
        }

        public void InvSlot4Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot4);
            Game.Instance.Player.Weapons.UpdateModels();
        }             

        public void InvSlot5Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot5);
            Game.Instance.Player.Weapons.UpdateModels();
        }        

        public void InvSlot6Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot6);
            Game.Instance.Player.Weapons.UpdateModels();
        }        

        public void InvSlot7Drop()
        {
            Debug.Log("Drop " + dragSlot);
            Game.Instance.Player.Weapons.SwapItemPositions(dragSlot, MechPosition.InvSlot7);
            Game.Instance.Player.Weapons.UpdateModels();
        }



    }
}