using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class WeaponsController : MonoBehaviour
    {
        [SerializeField] private List<Weapon> nose;
        [SerializeField] private List<Weapon> rightArm;
        [SerializeField] private List<Weapon> leftArm;
        [SerializeField] private List<Weapon> rightShoulder;
        [SerializeField] private List<Weapon> leftShoulder;

        [SerializeField] public WeaponPosition selectedPrimaryWeapon = 0;
        [SerializeField] public WeaponPosition selectedSecondaryWeapon = 0;
        //[SerializeField] public WeaponPosition currentWeapon;

        [SerializeField] public WeaponType noseWeapon;
        [SerializeField] public WeaponType leftArmWeapon;
        [SerializeField] public WeaponType rightArmWeapon;
        [SerializeField] public WeaponType leftShoulderWeapon;
        [SerializeField] public WeaponType rightShoulderWeapon;

        [SerializeField] public bool linkArms = false;
        [SerializeField] public bool linkShoulders = false;

        [SerializeField] public WeaponType noseWeaponTemp;
        [SerializeField] public WeaponType leftArmWeaponTemp;
        [SerializeField] public WeaponType rightArmWeaponTemp;
        [SerializeField] public WeaponType leftShoulderWeaponTemp;
        [SerializeField] public WeaponType rightShoulderWeaponTemp;

        public void Start()
        {
            UpdateWeapons();
        }

        public bool FirePrimaryWeapon()
        {
            var currentWeapon = GetWeapon(selectedPrimaryWeapon);
            return currentWeapon ? currentWeapon.type != WeaponType.None && currentWeapon.Fire() : false;
        }

        public bool FireSecondaryWeapon()
        {
            var currentWeapon = GetWeapon(selectedSecondaryWeapon);
            Debug.Log("Fire secondary " + (currentWeapon != null));
            return currentWeapon ? currentWeapon.type != WeaponType.None && currentWeapon.Fire() : false;
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

        // public Weapon GetCurrentWeapon()
        // {
        //     Weapon weapon;
        //     switch (selectedPrimaryWeapon)
        //     {
        //         case WeaponPosition.LeftShoulder:
        //             weapon = leftShoulder.FirstOrDefault(w => w.type == leftShoulderWeapon);
        //             break;
        //         case WeaponPosition.RightShoulder:
        //             weapon = rightShoulder.FirstOrDefault(w => w.type == rightShoulderWeapon);
        //             break;
        //         case WeaponPosition.LeftArm:
        //             weapon = leftArm.FirstOrDefault(w => w.type == leftArmWeapon);
        //             break;
        //         case WeaponPosition.RightArm:
        //         default:
        //             weapon = rightArm.FirstOrDefault(w => w.type == rightArmWeapon);
        //             break;
        //     }

        //     return weapon;
        // }

        public Weapon GetWeapon(WeaponPosition position)
        {
            switch (position)
            {

                case WeaponPosition.LeftShoulder:
                    return leftShoulder.FirstOrDefault(w => w.type == leftShoulderWeapon);
                case WeaponPosition.RightShoulder:
                    return rightShoulder.FirstOrDefault(w => w.type == rightShoulderWeapon);
                case WeaponPosition.LeftArm:
                    return leftArm.FirstOrDefault(w => w.type == leftArmWeapon);
                case WeaponPosition.RightArm:
                    return rightArm.FirstOrDefault(w => w.type == rightArmWeapon);
                case WeaponPosition.Nose:
                default:
                    return nose.FirstOrDefault(w => w.type == noseWeapon);
            }

        }

        public void UpgradeWeapon(WeaponType newWeapon)
        {
            switch (selectedPrimaryWeapon)
            {
                case WeaponPosition.LeftArm:
                    leftArmWeapon = newWeapon;
                    break;
                case WeaponPosition.RightShoulder:
                    rightShoulderWeapon = newWeapon;
                    break;
                case WeaponPosition.LeftShoulder:
                    leftShoulderWeapon = newWeapon;
                    break;
            }
        }

        public void ClearTempWeapons()
        {
            leftShoulderWeaponTemp = WeaponType.None;
            rightShoulderWeaponTemp = WeaponType.None;
            leftArmWeaponTemp = WeaponType.None;
            noseWeaponTemp = WeaponType.None;
            rightArmWeaponTemp = WeaponType.None;
            UpdateWeapons();
        }

        public void SetTempWeapon(WeaponType type)
        {
            switch (selectedPrimaryWeapon)
            {
                case WeaponPosition.LeftShoulder:
                    leftShoulderWeaponTemp = type;
                    break;
                case WeaponPosition.RightShoulder:
                    rightShoulderWeaponTemp = type;
                    break;
                case WeaponPosition.LeftArm:
                    leftArmWeaponTemp = type;
                    break;
                case WeaponPosition.Nose:
                    noseWeaponTemp = type;
                    break;
            }

            UpdateWeapons();
        }

        public void UpdateWeapons()
        {
            if (nose != null && nose.Count > 0)
            {
                nose.ForEach((weapon) => { weapon.gameObject.SetActive(noseWeaponTemp != WeaponType.None ? weapon.type == noseWeaponTemp : weapon.type == noseWeapon); });
            }
            if (leftArm != null && leftArm.Count > 0)
            {
                leftArm.ForEach((weapon) => { weapon.gameObject.SetActive(leftArmWeaponTemp != WeaponType.None ? weapon.type == leftArmWeaponTemp : weapon.type == leftArmWeapon); });
            }
            if (rightArm != null && rightArm.Count > 0)
            {
                rightArm.ForEach((weapon) => { weapon.gameObject.SetActive(weapon.type == rightArmWeapon); });
            }
            if (leftShoulder != null && leftShoulder.Count > 0)
            {
                leftShoulder.ForEach((weapon) => { weapon.gameObject.SetActive(leftShoulderWeaponTemp != WeaponType.None ? weapon.type == leftShoulderWeaponTemp : weapon.type == leftShoulderWeapon); });
            }
            if (rightShoulder != null && rightShoulder.Count > 0)
            {
                rightShoulder.ForEach((weapon) => { weapon.gameObject.SetActive(rightShoulderWeaponTemp != WeaponType.None ? weapon.type == rightShoulderWeaponTemp : weapon.type == rightShoulderWeapon); });
            }
        }
    }
}