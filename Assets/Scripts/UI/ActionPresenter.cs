using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class ActionPresenter : AbstractPresenter
    {
        [SerializeField] private List<UnityEngine.UI.Image> frames;
        [SerializeField] private List<UnityEngine.UI.Image> icons;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color primaryColor = Color.cyan;
        [SerializeField] private Color secondaryColor = Color.magenta;
        [SerializeField] private UnityEngine.UI.Button overdriveButton;
        [SerializeField] private UnityEngine.UI.Button coolantButton;
        [SerializeField] private UnityEngine.UI.Button repairButton;
        [SerializeField] private UnityEngine.UI.Text overdriveText;
        [SerializeField] private UnityEngine.UI.Text scrapText;
        [SerializeField] private Sprite noWeaponIcon;

        public override void Repaint()
        {
            var mech = Game.Instance.Player.Mech;

            var cooldown = mech.OverdriveCooldown - Time.time;
            overdriveText.text = cooldown <= 0 ? "" : cooldown.ToString("N0");
            scrapText.text = mech.Scrap.ToString("N0");
            overdriveButton.interactable = mech.CanOverdrive;
            repairButton.interactable = mech.Scrap > 0;
            coolantButton.interactable = mech.Coolant > 0;
            UpdateWeaponFrames(mech);
            UpdateWeaponIcons(mech);
        }

        public void LeftArmClick()
        {
            Game.Instance.Player.Mech?.FireWeapon(WeaponPosition.LeftArm);
        }

        public void RightArmClick()
        {
            Game.Instance.Player.Mech?.FireWeapon(WeaponPosition.RightArm);
        }

        public void LeftShoulderClick()
        {
            Game.Instance.Player.Mech?.FireWeapon(WeaponPosition.LeftShoulder);
        }

        public void RightShoulderClick()
        {
            Game.Instance.Player.Mech?.FireWeapon(WeaponPosition.RightShoulder);
        }

        public void NoseClick()
        {
            Game.Instance.Player.Mech?.FireWeapon(WeaponPosition.Nose);
        }

        public void CoolantDumpClick()
        {
            Game.Instance.Player.Mech?.CoolantDump();
        }

        public void OverdriveClick()
        {
            Game.Instance.Player.Mech?.Overdrive();
        }

        public void RepairClick()
        {
            Game.Instance.Player.Mech?.MaxHeal();
        }

        private void UpdateWeaponFrames(MechController weapons)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                frames[i].color = (int)weapons.selectedPrimaryWeapon == i ? primaryColor : ((int)weapons.selectedSecondaryWeapon == i ? secondaryColor : defaultColor);
            }
        }

        private void UpdateWeaponIcons(MechController weapons)
        {
            for (int i = 0; i < icons.Count; i++)
            {
                var weapon = weapons.GetWeapon((WeaponPosition)i);
                icons[i].sprite = weapon != null && weapon.item != null ? weapon.item.sprite : noWeaponIcon;
            }
        }
    }
}