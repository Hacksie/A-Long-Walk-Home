using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class HudPresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Slider healthBar;
        [SerializeField] private UnityEngine.UI.Slider heatBar;
        [SerializeField] private UnityEngine.UI.Slider coolantBar;

        public override void Repaint()
        {
            var data = Game.Instance.Data;

            healthBar.value = data.armour;
            heatBar.value = data.heat;
            coolantBar.value = data.coolant;

            //heatText.color = data.heat > 100 ? Color.red : Color.white;
        }
    }
}