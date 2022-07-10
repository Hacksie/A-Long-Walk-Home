using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class InventoryPresenter : AbstractPresenter
    {


        public override void Repaint()
        {
            
        }

        public void CloseClick()
        {
            Game.Instance.SetPlaying();
        }
    }
}