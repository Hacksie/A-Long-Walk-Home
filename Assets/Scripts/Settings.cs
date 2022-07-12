using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Settings", menuName = "State/Settings")]
    public class Settings : ScriptableObject
    {
        [Header("Player settings")]
        public float walkSpeed = 2.5f;
        public float rotateSpeed = 180.0f;
        public float orbitSpeed = 180.0f;

        public float clawRange = 0.7f;

        [Header("Game settings")]
        public int maxLevels = 25;
        public bool skipIntro = true;
        public Vector2 worldSize = new Vector2(400, 400);
        public Vector2 safeArea = new Vector2(35, 35);
        public float deadzone = 20.0f;
        public int obstacleCount = 300;
        public int enemyCount = 100;
        public int coolantCount = 20;


        [Header("Starting values")]
        public float startingArmour = 100.0f;
        public float startingHeat = 0.0f;
        public float startingCoolant = 0.0f;
        public float ambientHeatLoss = 1.0f;
        public float startingArmourMax = 100.0f;
        public float startingHeatMax = 100.0f;
        public float startingCoolantMax = 100.0f;

        public WeaponType startingLeftArm = WeaponType.Cannon;
        public WeaponType startingRightArm = WeaponType.Claw;
        public WeaponType startingLeftShoulder = WeaponType.Cannon;
        public WeaponType startingRightShoulder = WeaponType.Cannon;
        public WeaponType startingNose = WeaponType.Mining;

        public WeaponPosition startingPrimary = WeaponPosition.LeftArm;
        public WeaponPosition startingSecondary = WeaponPosition.Nose;

        [Header("Dialog")]
        public List<Dialog> dialogLines;

        public Vector3 startPosition = new Vector3(20, 0, 20);

    }
}