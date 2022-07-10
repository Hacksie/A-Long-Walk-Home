using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign.UI
{
    public class DialogPresenter : AbstractPresenter
    {
        [SerializeField] UnityEngine.UI.Text dialogText;

        public override void Repaint()
        {
            dialogText.text = Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines[Game.Instance.Data.currentDialogLine];
        }

        public void NextClick()
        {
            Game.Instance.Data.currentDialogLine++;
            if (Game.Instance.Data.currentDialogLine >= Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines.Count)
            {
                Debug.Log("Start game", this);
                Game.Instance.Data.currentDialogLine = 0;
                Game.Instance.SetPlaying();
            }
            else
            {
                Repaint();
            }

        }
    }
}