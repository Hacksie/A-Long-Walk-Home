using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Settings", menuName = "State/Settings")]
    public class Settings : ScriptableObject
    {
        [Header("Game settings")]
        public int maxLevels = 25;
        public bool skipIntro = true;
        public Vector2 worldSize = new Vector2(400, 400);
        public Vector2 safeArea = new Vector2(35, 35);
        public float deadzone = 20.0f;
        public int obstacleCount = 300;
        public int enemyCount = 100;
        public int coolantCount = 20;
        public int pickupCount = 20;
        public float sqPickupRadius = 4.0f;
        //public float ambientHeatLoss = 1.0f;
        public float lootChance = 0.5f;
        public float scrapChance = 0.5f;
        public List<float> lootLevel = new List<float>();
        public float damageShakeAmount = 0.5f;
        public float damageShakeLength = 0.2f;
        


        [Header("Inventory Item Templates")]
        public InventoryItem scrap;
        public InventoryItem motor;
        public InventoryItem armour;
        public InventoryItem radar;
        public InventoryItem claw;
        public InventoryItem mining;
        public InventoryItem cannon;
        public InventoryItem gattling;
        public InventoryItem gauss;
        public InventoryItem laser;
        public InventoryItem autocannon;
        public InventoryItem missiles;
        public InventoryItem coolant;

        public WeaponPosition startingPrimary = WeaponPosition.LeftArm;
        public WeaponPosition startingSecondary = WeaponPosition.Nose;

        [Header("Dialog")]
        public List<Dialog> dialogLines = new List<Dialog>();

        public Vector3 startPosition = new Vector3(20, 0, 20);

    }
}