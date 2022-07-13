using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    // FIXME:L Make this a generic inventory item settings
    // Include what positon it can go in
    [CreateAssetMenu(fileName = "InvItem", menuName = "State/InvItem")]
    public class InventoryItem : ScriptableObject
    {
        [SerializeField] public List<MechPosition> allowedMechPositions;
        [SerializeField] public WeaponType weaponType;
        [SerializeField] public ItemType itemType = ItemType.Scrap;
        [SerializeField] public ItemLevel itemLevel = ItemLevel.Normal;
        [SerializeField] public AmmoType ammoType;
        [SerializeField] public float baseFireRate = 0;
        [SerializeField] public float baseMinDamage = 10;
        [SerializeField] public float baseMaxDamage = 10;
        [SerializeField] public float baseHeat = 10;
        [SerializeField] public float shake = 0.5f;
        [SerializeField] public Sprite sprite;
        [SerializeField] public bool canRemove = true;
        [SerializeField] public AudioClip sfx;
        [SerializeField] public float sfxPitch = 1.0f;
        [SerializeField] public int scrapAmount = 1;
    }

    public enum ItemType
    {
        Scrap,
        Weapon,
        Legs,
        Body,
        Radar
    }

    public enum ItemLevel
    {
        Normal,
        Uncommon,
        Rare,
        Epic
    }    
}