using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HackedDesign.UI
{
    public class ConsolePresenter : AbstractPresenter
    {
        [SerializeField] private UnityEngine.UI.Text consoleText;

        public override void Repaint()
        {
            var text = "";
            var console = Game.Instance.Console.ToList();
            foreach(var line in console)
            {
                text += (line + "\n");
            }

            consoleText.text = text;
        }
    }
}