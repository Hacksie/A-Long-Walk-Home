using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "MechSettings", menuName = "State/MechSettings")]
    public class MechSettings : ScriptableObject
    {
        public float rotateSpeed = 180.0f;
        //public float orbitSpeed = 180.0f;        
        public float startingArmour = 100.0f;
        public float startingShield = 0.0f;
        public float overdriveTime = 60.0f;

        public float startingHeat = 0.0f;
        public float startingCoolant = 0.0f;
        public float ambientHeatLoss = 1.0f;
        public float startingArmourMax = 100.0f;
        public float startingHeatMax = 100.0f;
        public float startingShieldMax = 100.0f;
        public float startingCoolantMax = 100.0f;
        public int startingScrap = 0;
        public float walkSpeed = 2.5f;
    }
}