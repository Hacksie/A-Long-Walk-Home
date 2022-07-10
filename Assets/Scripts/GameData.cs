using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    [System.Serializable]
    public class GameData
    {
        public UI.MainMenuState menuState;
        public float armour = 100.0f;
        public float heat = 0.0f;
        public float coolant = 100.0f;

        public float ambientHeatLoss = 1.0f;
        public float heatDamage = 3.0f;
        public float coolantDump = 25.0f;

        public int currentLevel = 0;
        public int currentDialogLine = 0;

        public int scrap = 0;

        public InventoryItem[] inventory = new InventoryItem[6];

        public void Reset(Settings settings)
        {
            armour = settings.startingArmour;
            heat = settings.startingHeat;
            coolant = settings.startingCoolant;
            ambientHeatLoss = settings.ambientHeatLoss;
            heatDamage = 3.0f;
            coolantDump = 25.0f;
        }
    }
}