using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    [System.Serializable]
    public class GameData
    {
        public UI.MainMenuState menuState;
        public float overheatDamage = 3.0f;
        public float coolantDump = 25.0f;

        public int currentLevel = 0;
        public int currentDialogLine = 0;
        public bool skipIntro = false;

        //public int scrap = 0;

        public InventoryItem hoverItem;


        public void Reset(Settings settings)
        {
            overheatDamage = 3.0f;
            coolantDump = 25.0f;
            currentLevel = 1;
        }
    }
}