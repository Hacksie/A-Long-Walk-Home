using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class DeadPresenter : AbstractPresenter
    {
        
        public override void Repaint()
        {
        }

        public void QuitClick()
        {
            Game.Instance.SetMenu();
        }
    }
}