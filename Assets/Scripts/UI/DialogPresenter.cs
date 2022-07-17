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
            string line = "";
            if (Game.Instance.Settings.dialogLines != null && Game.Instance.Settings.dialogLines.Count > Game.Instance.Data.currentLevel && Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines != null && Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines.Count > Game.Instance.Data.currentDialogLine)
            {
                line = Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines[Game.Instance.Data.currentDialogLine];
            }

            dialogText.text = line;
        }

        public void NextClick()
        {
            Game.Instance.Data.currentDialogLine++;

            if (Game.Instance.Settings.dialogLines == null || Game.Instance.Settings.dialogLines.Count <= Game.Instance.Data.currentLevel || Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines == null || Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines.Count <= Game.Instance.Data.currentDialogLine)
            {
                Game.Instance.Data.currentDialogLine = 0;
                Game.Instance.SetPlaying();
            }

            else if (Game.Instance.Data.currentDialogLine >= Game.Instance.Settings.dialogLines[Game.Instance.Data.currentLevel].lines.Count)
            {
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