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
        [SerializeField] private UnityEngine.UI.Text scrapText;
        [SerializeField] private Sprite noWeaponIcon;

        public override void Repaint()
        {
            var mech = Game.Instance.Player.Mech;
            linkArms.SetActive(mech.linkArms);
            linkShoulders.SetActive(mech.linkShoulders);
            var cooldown = mech.OverdriveCooldown - Time.time;
            overdriveText.text = cooldown <= 0 ? "" : cooldown.ToString("N0");
            scrapText.text = mech.Scrap.ToString("N0");
            UpdateWeaponFrames(mech);
            UpdateWeaponIcons(mech);
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