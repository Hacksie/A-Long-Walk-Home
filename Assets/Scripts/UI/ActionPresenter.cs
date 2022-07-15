using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class ActionPresenter : AbstractPresenter
    {
        [SerializeField] private GameObject linkArms;
        [SerializeField] private GameObject linkShoulders;
        [SerializeField] private List<UnityEngine.UI.Image> frames;
        [SerializeField] private List<UnityEngine.UI.Image> icons;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color primaryColor = Color.cyan;
        [SerializeField] private Color secondaryColor = Color.magenta;
        [SerializeField] private UnityEngine.UI.Text overdriveText;
        [SerializeField] private Sprite noWeaponIcon;

        public override void Repaint()
        {
            var weapons = Game.Instance.Player.Mech;
            linkArms.SetActive(weapons.linkArms);
            linkShoulders.SetActive(weapons.linkShoulders);
            //var data = Game.Instance.Data;
            var cooldown = Game.Instance.Player.Mech.OverdriveCooldown;
            overdriveText.text = cooldown <= 0 ? "" : (cooldown - Time.time).ToString("N0");
            UpdateWeaponFrames(weapons);
            UpdateWeaponIcons(weapons);
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