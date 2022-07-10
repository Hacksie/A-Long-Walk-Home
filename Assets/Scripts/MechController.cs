using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class MechController : MonoBehaviour
    {

        [SerializeField] private Weapon nose;
        [SerializeField] private Weapon rightArm;
        [SerializeField] private Weapon leftArm;
        [SerializeField] private Weapon rightShoulder;
        [SerializeField] private Weapon leftShoulder;
        [SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>(8);

        [SerializeField] public WeaponPosition selectedPrimaryWeapon = 0;
        [SerializeField] public WeaponPosition selectedSecondaryWeapon = 0;
        //[SerializeField] public WeaponPosition currentWeapon;

        // [SerializeField] public WeaponType noseWeapon;
        // [SerializeField] public WeaponType leftArmWeapon;
        // [SerializeField] public WeaponType rightArmWeapon;
        // [SerializeField] public WeaponType leftShoulderWeapon;
        // [SerializeField] public WeaponType rightShoulderWeapon;

        [SerializeField] public bool linkArms = false;
        [SerializeField] public bool linkShoulders = false;

        // [SerializeField] public WeaponType noseWeaponTemp;
        // [SerializeField] public WeaponType leftArmWeaponTemp;
        // [SerializeField] public WeaponType rightArmWeaponTemp;
        // [SerializeField] public WeaponType leftShoulderWeaponTemp;
        // [SerializeField] public WeaponType rightShoulderWeaponTemp;

        public void Start()
        {
            UpdateWeapons();
        }

        public bool FirePrimaryWeapon()
        {
            return GetWeapon(selectedPrimaryWeapon).Fire();
            //return currentWeapon ? currentWeapon.type != WeaponType.None && currentWeapon.Fire() : false;
        }

        public bool FireSecondaryWeapon()
        {
            return GetWeapon(selectedSecondaryWeapon).Fire();
        }

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
            switch(position)
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
                default:
                    return null;
            }
        }

        public void SetItem(MechPosition position, InventoryItem item)
        {
            switch(position)
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
                default:
                    nose.item = item;
                    break;
            }
        }        

        public void SwapItemPositions(MechPosition pos1, MechPosition pos2)
        {
            // FIXME: Probably have to clone these
            var item1 = GetItem(pos1);
            var item2 = GetItem(pos2);
            SetItem(pos2, item1);
            SetItem(pos1, item2);
        }

        // public void UpgradeWeapon(WeaponType newWeapon)
        // {
        //     switch (selectedPrimaryWeapon)
        //     {
        //         case WeaponPosition.LeftArm:
        //             leftArmWeapon = newWeapon;
        //             break;
        //         case WeaponPosition.RightShoulder:
        //             rightShoulderWeapon = newWeapon;
        //             break;
        //         case WeaponPosition.LeftShoulder:
        //             leftShoulderWeapon = newWeapon;
        //             break;
        //     }
        // }

        // public void ClearTempWeapons()
        // {
        //     leftShoulderWeaponTemp = WeaponType.None;
        //     rightShoulderWeaponTemp = WeaponType.None;
        //     leftArmWeaponTemp = WeaponType.None;
        //     noseWeaponTemp = WeaponType.None;
        //     rightArmWeaponTemp = WeaponType.None;
        //     UpdateWeapons();
        // }

        // public void SetTempWeapon(WeaponType type)
        // {
        //     switch (selectedPrimaryWeapon)
        //     {
        //         case WeaponPosition.LeftShoulder:
        //             leftShoulderWeaponTemp = type;
        //             break;
        //         case WeaponPosition.RightShoulder:
        //             rightShoulderWeaponTemp = type;
        //             break;
        //         case WeaponPosition.LeftArm:
        //             leftArmWeaponTemp = type;
        //             break;
        //         case WeaponPosition.Nose:
        //             noseWeaponTemp = type;
        //             break;
        //     }

        //     UpdateWeapons();
        // }

        public void UpdateWeapons()
        {
            nose.UpdateModel();
            rightArm.UpdateModel();
            leftArm.UpdateModel();
            rightShoulder.UpdateModel();
            leftShoulder.UpdateModel();
        }
    }
}