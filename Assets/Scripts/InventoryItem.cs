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
        [SerializeField] public AmmoType ammoType;
        [SerializeField] public float baseFireRate = 0;
        [SerializeField] public float baseDamage = 10;
        [SerializeField] public float baseHeat = 10;
        [SerializeField] public float shake = 0.5f;
        [SerializeField] public Sprite sprite;
        [SerializeField] public bool canRemove = true;
    }
}