using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    // Include what positon it can go in
    [CreateAssetMenu(fileName = "InvItem", menuName = "State/InvItem")]
    public class InventoryItem : ScriptableObject
    {
        [SerializeField] public List<MechPosition> allowedMechPositions = new List<MechPosition>();
        [SerializeField] public WeaponType weaponType;
        [SerializeField] public ItemType itemType = ItemType.Scrap;
        [SerializeField] public ItemLevel itemLevel = ItemLevel.Normal;
        [SerializeField] public AmmoType ammoType;
        [SerializeField] public float baseFireRate = 0;
        [SerializeField] public InvRange[] genFireRate = new InvRange[5];
        [SerializeField] public float baseMinDamage = 10;
        [SerializeField] public InvRange[] genMinDamage = new InvRange[5];
        [SerializeField] public float baseMaxDamage = 10;
        [SerializeField] public InvRange[] genMaxDamage = new InvRange[5];
        [SerializeField] public float baseHeat = 10;
        [SerializeField] public InvRange[] genHeat = new InvRange[5];
        [SerializeField] public float baseSpeed = 0.0f;
        [SerializeField] public InvRange[] genSpeed = new InvRange[5];
        [SerializeField] public float baseOverdriveTime = 0.0f;
        [SerializeField] public InvRange[] genOverdriveTime = new InvRange[5];
        [SerializeField] public float baseOverdriveMult = 0.0f;
        [SerializeField] public InvRange[] genOverdriveMult = new InvRange[5];
        [SerializeField] public float baseRange = 10;
        [SerializeField] public InvRange[] genRange = new InvRange[5];
        [SerializeField] public float baseArmour = 0;
        [SerializeField] public InvRange[] genArmour = new InvRange[5];
        [SerializeField] public float baseArmourRegen = 0;
        [SerializeField] public InvRange[] genArmourRegen = new InvRange[5];
        [SerializeField] public float baseShield = 0;
        [SerializeField] public InvRange[] genShield = new InvRange[5];
        [SerializeField] public int scrapAmount = 1;
        [SerializeField] public int scrapMulti = 1;
        [SerializeField] public InvIntRange[] genScrap = new InvIntRange[5];
        [SerializeField] public float baseCoolant = 0;
        [SerializeField] public InvRange[] genCoolant = new InvRange[5];
        [SerializeField] public float shake = 0.5f;
        [SerializeField] public Sprite sprite;
        [SerializeField] public bool canRemove = true;
        [SerializeField] public AudioClip sfx;
        [SerializeField] public float sfxPitch = 1.0f;

        public static InventoryItem RandomScrap()
        {
            var settings = Game.Instance.Settings;
            InventoryItem item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.scrap);
            item.Randomize(true);
            return item;
        }

        public static InventoryItem RandomItem()
        {
            var roll = Mathf.FloorToInt(Random.value * 13.0f);
            var settings = Game.Instance.Settings;
            InventoryItem item;
            //FIXME: Check the roll against a loot table to allow for different chances for differen items

            switch (roll)
            {
                case 0:
                    // Scrap
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.scrap);
                    break;
                case 1:
                case 11: // Double roll armour
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.armour);
                    break;
                case 2:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.autocannon);
                    break;
                case 3:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.cannon);
                    break;
                case 4:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.gattling);
                    break;
                case 5:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.gauss);
                    break;
                case 6:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.laser);
                    break;
                case 7:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.mining);
                    break;
                case 8:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.missiles);
                    break;
                case 9:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.motor);
                    break;
                case 10:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.radar);
                    break;
                default:
                    item = ScriptableObject.CreateInstance<InventoryItem>().Copy(settings.scrap);
                    break;
                    //Scrap;
            }

            item.Randomize(true);
            return item;
        }


        public InventoryItem Copy(InventoryItem item)
        {
            name = item.name;
            weaponType = item.weaponType;
            itemType = item.itemType;
            itemLevel = item.itemLevel;
            ammoType = item.ammoType;
            baseFireRate = item.baseFireRate;
            baseMinDamage = item.baseMinDamage;
            baseMaxDamage = item.baseMaxDamage;
            baseHeat = item.baseHeat;
            baseSpeed = item.baseSpeed;
            shake = item.shake;
            canRemove = item.canRemove;
            sprite = item.sprite;
            sfx = item.sfx;
            sfxPitch = item.sfxPitch;
            scrapAmount = item.scrapAmount;
            allowedMechPositions = new List<MechPosition>(item.allowedMechPositions);
            item.genArmour.CopyTo(genArmour, 0);
            item.genArmourRegen.CopyTo(genArmourRegen, 0);
            item.genShield.CopyTo(genShield, 0);
            item.genRange.CopyTo(genRange, 0);
            item.genFireRate.CopyTo(genFireRate, 0);
            item.genMinDamage.CopyTo(genMinDamage, 0);
            item.genMaxDamage.CopyTo(genMaxDamage, 0);
            item.genHeat.CopyTo(genHeat, 0);
            item.genSpeed.CopyTo(genSpeed, 0);
            item.genOverdriveTime.CopyTo(genOverdriveTime, 0);
            item.genOverdriveMult.CopyTo(genOverdriveMult, 0);
            item.genScrap.CopyTo(genScrap, 0);
            return this;
        }




        public InventoryItem Randomize(bool roll)
        {
            itemLevel = GenerateItemLevel();
            if (roll)
            {
                return Randomize(itemLevel);
            }
            else
            {
                return Minimize(itemLevel);
            }
        }
        public InventoryItem Randomize(ItemLevel level)
        {
            baseFireRate = GetRandomFireRate(level);
            baseSpeed = GetRandomSpeed(level);
            baseArmour = GetRandomArmour(level);
            baseArmourRegen = GetRandomArmourRegen(level);
            baseShield = GetRandomShield(level);
            baseRange = GetRandomRange(level);
            baseMinDamage = GetRandomMinDamage(level);
            baseMaxDamage = GetRandomMaxDamage(level);
            baseHeat = GetRandomHeat(level);
            baseOverdriveTime = GetRandomOverdriveTime(level);
            scrapAmount = GetRandomScrap(level);

            return this;
        }

        public InventoryItem Minimize(ItemLevel level)
        {
            baseFireRate = GetMinFireRate(level);
            baseSpeed = GetMinSpeed(level);
            baseArmour = GetMinArmour(level);
            baseArmourRegen = GetMinArmourRegen(level);
            baseShield = GetMinShield(level);
            baseRange = GetMinRange(level);
            baseMinDamage = GetMinMinDamage(level);
            baseMaxDamage = GetMinMaxDamage(level);
            baseHeat = GetMinHeat(level);
            baseOverdriveTime = GetMinOverdriveTime(level);
            scrapAmount = GetMinScrap(level);

            return this;
        }

        private ItemLevel GenerateItemLevel()
        {
            var currentLevel = Game.Instance.Data.currentLevel;
            var roll = Random.value + (currentLevel / 10);
            return (ItemLevel)(Game.Instance.Settings.lootLevel.Count - Game.Instance.Settings.lootLevel.FindIndex(0, c => c <= roll) - 1);
        }

        public float GetRandomFireRate(ItemLevel level) => Random.Range(genFireRate[(int)level].min, genFireRate[(int)level].max);
        public float GetRandomSpeed(ItemLevel level) => Random.Range(genSpeed[(int)level].min, genSpeed[(int)level].max);
        public float GetRandomHeat(ItemLevel level) => Random.Range(genHeat[(int)level].min, genHeat[(int)level].max);
        public float GetRandomRange(ItemLevel level) => Random.Range(genRange[(int)level].min, genRange[(int)level].max);
        public float GetRandomArmour(ItemLevel level) => Random.Range(genArmour[(int)level].min, genArmour[(int)level].max);
        public float GetRandomArmourRegen(ItemLevel level) => Random.Range(genArmourRegen[(int)level].min, genArmourRegen[(int)level].max);
        public float GetRandomShield(ItemLevel level) => Random.Range(genShield[(int)level].min, genShield[(int)level].max);
        public float GetRandomMinDamage(ItemLevel level) => Random.Range(genMinDamage[(int)level].min, genMinDamage[(int)level].max);
        public float GetRandomMaxDamage(ItemLevel level) => Random.Range(genMaxDamage[(int)level].min, genMaxDamage[(int)level].max);
        public float GetRandomOverdriveTime(ItemLevel level) => Random.Range(genOverdriveTime[(int)level].min, genOverdriveTime[(int)level].max);
        public int GetRandomScrap(ItemLevel level) => Random.Range(genScrap[(int)level].min, genScrap[(int)level].max);

        public float GetMinFireRate(ItemLevel level) => genFireRate[(int)level].min;
        public float GetMinSpeed(ItemLevel level) => genSpeed[(int)level].min;
        public float GetMinHeat(ItemLevel level) => genHeat[(int)level].min;
        public float GetMinRange(ItemLevel level) => genRange[(int)level].min;
        public float GetMinArmour(ItemLevel level) => genArmour[(int)level].min;
        public float GetMinArmourRegen(ItemLevel level) => genArmourRegen[(int)level].min;
        public float GetMinShield(ItemLevel level) => genShield[(int)level].min;
        public float GetMinMinDamage(ItemLevel level) => genMinDamage[(int)level].min;
        public float GetMinMaxDamage(ItemLevel level) => genMaxDamage[(int)level].min;
        public float GetMinOverdriveTime(ItemLevel level) => genOverdriveTime[(int)level].min;
        public int GetMinScrap(ItemLevel level) => genScrap[(int)level].min;


    }

    [System.Serializable]
    public class InvRange
    {
        public float min;
        public float max;
    }

    [System.Serializable]
    public class InvIntRange
    {
        public int min;
        public int max;
    }

    public enum ItemType
    {
        Scrap,
        Weapon,
        Motor,
        Armour,
        Radar,
        Coolant
    }

    public enum ItemLevel
    {
        Scrap,
        Normal,
        Uncommon,
        Rare,
        Epic,
    }
}