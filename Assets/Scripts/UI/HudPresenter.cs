using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class HudPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Slider healthBar;
        [SerializeField] private UnityEngine.UI.Slider shieldBar;
        [SerializeField] private UnityEngine.UI.Slider heatBar;
        [SerializeField] private UnityEngine.UI.Slider coolantBar;
        [SerializeField] private UnityEngine.UI.Text armourText;
        [SerializeField] private UnityEngine.UI.Text shieldText;
        [SerializeField] private UnityEngine.UI.Text heatText;
        [SerializeField] private UnityEngine.UI.Text coolantText;
        [SerializeField] private UnityEngine.UI.RawImage radarImage;

        public override void Repaint()
        {
            var data = Game.Instance.Data;
            var mech = Game.Instance.Player.Mech;

            healthBar.maxValue = mech.ArmourMax;
            shieldBar.maxValue = mech.ShieldMax;
            heatBar.maxValue = mech.HeatMax;
            coolantBar.maxValue = mech.CoolantMax;
            
            healthBar.value = mech.Data.armour;
            shieldBar.value = mech.Data.shield;
            heatBar.value = mech.Data.heat;
            coolantBar.value = mech.Data.coolant;
            armourText.text = mech.Data.armour.ToString("N0");
            shieldText.text = mech.Data.shield.ToString("N0");
            heatText.text = mech.Data.heat.ToString("N0");
            coolantText.text = mech.Data.coolant.ToString("N0");
            radarImage.gameObject.SetActive(mech.RadarRange > 0 );
        }
    }
}