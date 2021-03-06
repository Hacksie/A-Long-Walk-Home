using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class PausePresenter : AbstractPresenter
    {
        
        public override void Repaint()
        {
        }

        public void ResumeClick()
        {
            Game.Instance.SetPlaying();
        }

        public void QuitClick()
        {
            Game.Instance.SetMenu();
        }
    }
}