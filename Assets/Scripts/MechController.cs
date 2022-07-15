using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class MechController : MonoBehaviour
    {
        [SerializeField] private MechSettings settings;
        [SerializeField] private Weapon nose;
        [SerializeField] private Weapon rightArm;
        [SerializeField] private Weapon leftArm;
        [SerializeField] private Weapon rightShoulder;
        [SerializeField] private Weapon leftShoulder;
        [SerializeField] private List<MechData> data = new List<MechData>(3);
        [SerializeField] private InventoryItem legs;
        [SerializeField] private InventoryItem body;
        [SerializeField] private InventoryItem radar;
        [SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>(8);

        [SerializeField] public WeaponPosition selectedPrimaryWeapon = 0;
        [SerializeField] public WeaponPosition selectedSecondaryWeapon = 0;

        [SerializeField] public bool linkArms = false;
        [SerializeField] public bool linkShoulders = false;

        private bool overdriven = false;
        private float overdriveTime = 0;
        private float overdriveCooldown = 0;

        public MechData Data { get { return data[Game.Instance.CurrentSlot]; } private set { data[Game.Instance.CurrentSlot] = value; } }

        public float HeatLoss { get { return Game.Instance.Settings.ambientHeatLoss; } }
        public float ArmourMax { get { return settings.startingArmourMax + (body != null ? body.baseArmour : 0); } }
        public float ShieldMax { get { return settings.startingShieldMax + (body != null ? body.baseShield : 0); } }
        public float HeatMax { get { return settings.startingHeatMax; } }
        public float CoolantMax { get { return settings.startingCoolantMax; } }
        public bool IsOverheating { get { return Data.heat >= HeatMax; } }
        public float WalkSpeed { get { return settings.walkSpeed + body.baseSpeed + (overdriven ? 1 : 0); } }
        public float RadarRange { get { return radar != null ? radar.baseRange : 0.0f; } }
        public float OverdriveMultiplier { get { return legs != null ? legs.baseOverdriveMult : 0; }}
        public float OverdriveCooldown { get { return overdriveCooldown; }}

        public void Awake()
        {
            for (int i = 0; i < 3; i++)
            {
                data[i] = new MechData();
            }
        }
        public void Start()
        {

            Reset();
        }

        public void Reset()
        {
            overdriveTime = 0;
            overdriven = false;
            Data.Reset(settings);
            UpdateModels();
        }

        public void DamageArmour(float amount, bool shake)
        {
            Data.armour = Mathf.Clamp(Data.armour - amount, 0, ArmourMax);
            if (shake)
            {
                Game.Instance.CameraShake.Shake(0.5f, 0.2f); //FIXME: Setting
            }

            // Check if player
            if (Data.armour <= 0)
            {
                Game.Instance.Pool.SpawnExplosion(Game.Instance.Player.transform.position);
                Game.Instance.SetDead();
            }
        }

        public void Overdrive()
        {
            if (!overdriven && overdriveTime <= 0.0f && Time.time >= overdriveCooldown)
            {
                Debug.Log("OVERDRIVE!", this);
                var item = GetItem(MechPosition.Motor);
                overdriveTime = item.baseOverdriveTime;
                overdriveCooldown = Time.time + settings.overdriveTime;
                overdriven = true;
            }
        }

        public void UpdateStatus(float deltaTime)
        {
            UpdateOverdrive(deltaTime);
            ArmourRegen(deltaTime);
            AmbientHeatLoss(deltaTime);
            UpdateOverHeat(deltaTime);
        }

        public void UpdateOverdrive(float deltaTime)
        {
            
            if (overdriven && overdriveTime <= 0.0f)
            {
                overdriveTime = 0;
                overdriven = false;
            }
            if (overdriven)
            {
                overdriveTime -= deltaTime;
                Debug.Log("OVERDRIVE!", this);
            }
        }



        public void MaxHeal()
        {
            float amountReq = ArmourMax - Data.armour;
            int amount = Mathf.CeilToInt(Mathf.Min(amountReq, Data.scrap));
            DamageArmour(-1 * amount, false);
            Data.scrap -= amount;
        }

        public void ArmourRegen(float deltaTime)
        {
            if (overdriven)
            {
                DamageArmour(-4 * deltaTime, false);
            }
            
            var item = GetItem(MechPosition.Armour);

            if(item != null)
            {
                DamageArmour(-1 * item.baseArmourRegen * deltaTime, false);
            }
        }

        public void AmbientHeatLoss(float deltaTime)
        {
            if (overdriven)
            {
                IncreaseHeat(1.0f * (HeatLoss) * deltaTime);
            }
            else
            {
                IncreaseHeat(-1.0f * (HeatLoss) * deltaTime);
            }

        }

        private void UpdateOverHeat(float deltaTime)
        {
            if (IsOverheating)
            {
                DamageArmour(Game.Instance.Data.overheatDamage * deltaTime, true);
                //AudioManager.Instance.PlayWarning();
            }
        }        

        public void IncreaseHeat(float amount)
        {
            Data.heat = Mathf.Max(Data.heat + amount, 0);
        }

        public void IncreaseCoolant(float amount)
        {
            Data.coolant = Mathf.Clamp(Data.coolant + amount, 0, CoolantMax);
        }

        public void CoolantDump()
        {
            var amount = Data.coolant;

            IncreaseHeat(-1 * Data.coolant);
            Data.coolant = 0;
        }

        public bool FirePrimaryWeapon() => GetWeapon(selectedPrimaryWeapon).Fire();
        public bool FireSecondaryWeapon() => GetWeapon(selectedSecondaryWeapon).Fire();

        public void FireAllWeapons()
        {
            for (int i = 0; i < 4; i++)
            {
                GetWeapon((WeaponPosition)i)?.Fire();
            }
        }

        public void NextPrimaryWeapon()
        {
            selectedPrimaryWeapon++;

            if (selectedPrimaryWeapon > WeaponPosition.Nose)
            {
                selectedPrimaryWeapon = WeaponPosition.RightArm;
            }
        }

        public void PrevPrimaryWeapon()
        {
            selectedPrimaryWeapon--;

            if (selectedPrimaryWeapon < 0)
            {
                selectedPrimaryWeapon = WeaponPosition.Nose;
            }
        }

        public void NextSecondaryWeapon()
        {
            selectedSecondaryWeapon++;

            if (selectedSecondaryWeapon > WeaponPosition.Nose)
            {
                selectedSecondaryWeapon = WeaponPosition.RightArm;
            }
        }

        public void PrevSecondaryWeapon()
        {
            selectedSecondaryWeapon--;

            if (selectedSecondaryWeapon < 0)
            {
                selectedSecondaryWeapon = WeaponPosition.Nose;
            }
        }

        public Weapon GetWeapon(WeaponPosition position)
        {
            switch (position)
            {
                case WeaponPosition.LeftShoulder:
                    return leftShoulder;
                case WeaponPosition.RightShoulder:
                    return rightShoulder;
                case WeaponPosition.LeftArm:
                    return leftArm;
                case WeaponPosition.RightArm:
                    return rightArm;
                case WeaponPosition.Nose:
                default:
                    return nose;
            }
        }

        public InventoryItem GetItem(MechPosition position)
        {
            switch (position)
            {
                case MechPosition.RightArm:
                    return rightArm.item;
                case MechPosition.LeftArm:
                    return leftArm.item;
                case MechPosition.RightShoulder:
                    return rightShoulder.item;
                case MechPosition.LeftShoulder:
                    return leftShoulder.item;
                case MechPosition.Nose:
                    return nose.item;
                case MechPosition.Motor:
                    return legs;
                case MechPosition.Armour:
                    return body;
                case MechPosition.Radar:
                    return radar;
                case MechPosition.InvSlot0:
                    return inventory[0];
                case MechPosition.InvSlot1:
                    return inventory[1];
                case MechPosition.InvSlot2:
                    return inventory[2];
                case MechPosition.InvSlot3:
                    return inventory[3];
                case MechPosition.InvSlot4:
                    return inventory[4];
                case MechPosition.InvSlot5:
                    return inventory[5];
                case MechPosition.InvSlot6:
                    return inventory[6];
                case MechPosition.InvSlot7:
                    return inventory[7];
                default:
                    return null;
            }
        }

        public void SetItem(MechPosition position, InventoryItem item)
        {
            switch (position)
            {
                case MechPosition.RightArm:
                    rightArm.item = item;
                    break;
                case MechPosition.LeftArm:
                    leftArm.item = item;
                    break;
                case MechPosition.RightShoulder:
                    rightShoulder.item = item;
                    break;
                case MechPosition.LeftShoulder:
                    leftShoulder.item = item;
                    break;
                case MechPosition.Nose:
                    nose.item = item;
                    break;
                case MechPosition.Motor:
                    legs = item;
                    break;
                case MechPosition.Armour:
                    body = item;
                    break;
                case MechPosition.Radar:
                    radar = item;
                    break;
                case MechPosition.InvSlot0:
                    inventory[0] = item;
                    break;
                case MechPosition.InvSlot1:
                    inventory[1] = item;
                    break;
                case MechPosition.InvSlot2:
                    inventory[2] = item;
                    break;
                case MechPosition.InvSlot3:
                    inventory[3] = item;
                    break;
                case MechPosition.InvSlot4:
                    inventory[4] = item;
                    break;
                case MechPosition.InvSlot5:
                    inventory[5] = item;
                    break;
                case MechPosition.InvSlot6:
                    inventory[6] = item;
                    break;
                case MechPosition.InvSlot7:
                    inventory[7] = item;
                    break;

                default:
                    nose.item = item;
                    break;
            }
            UpdateModels();
        }

        public bool SwapItemPositions(MechPosition pos1, MechPosition pos2)
        {
            var item1 = GetItem(pos1);
            var item2 = GetItem(pos2);

            if (item1 != null && !item1.canRemove)
            {
                Debug.Log("Can't remove item1");
                return false;
            }

            if (item2 != null && !item2.canRemove)
            {
                Debug.Log("Can't remove item2");
                return false;
            }

            if (item1 != null && !(item1.allowedMechPositions.Contains(pos2) || isInventoryPosition(pos2)))
            {
                Debug.Log("Can't swap item1 here");
                return false;
            }

            if (item2 != null && !(item2.allowedMechPositions.Contains(pos1) || isInventoryPosition(pos1)))
            {
                Debug.Log("Can't swap item2 here");
                return false;
            }

            SetItem(pos2, item1);
            SetItem(pos1, item2);
            UpdateModels();

            return true;
        }

        private bool isInventoryPosition(MechPosition pos)
        {
            return (pos == MechPosition.InvSlot0
                    || pos == MechPosition.InvSlot1
                    || pos == MechPosition.InvSlot2
                    || pos == MechPosition.InvSlot3
                    || pos == MechPosition.InvSlot4
                    || pos == MechPosition.InvSlot5
                    || pos == MechPosition.InvSlot6
                    || pos == MechPosition.InvSlot7);

        }

        public void UpdateModels()
        {
            nose?.UpdateModel();
            rightArm?.UpdateModel();
            leftArm?.UpdateModel();
            rightShoulder?.UpdateModel();
            leftShoulder?.UpdateModel();
        }
    }
}