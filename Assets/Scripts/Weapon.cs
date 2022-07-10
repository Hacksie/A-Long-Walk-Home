using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] public GameObject parent;
        [SerializeField] public Transform firePoint;
        [SerializeField] public bool isPlayer = false;
        [SerializeField] public InventoryItem item;
        [SerializeField] public List<WeaponModel> models;

        private float nextFireTime = 0;

        void Awake()
        {
            isPlayer = parent.CompareTag("Player");
        }

        public void UpdateModel()
        {
            // FIXME: Check that the inv item can go here
            for (int i = 0; i < models.Count; i++)
            {
                models[i].gameObject.SetActive(item != null && models[i].weaponType == item.weaponType);
            }
        }

        public bool Fire()
        {
            if (item != null)
            {
                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + item.baseFireRate;

                    FireAmmo();

                    if (isPlayer)
                    {
                        Game.Instance.IncreaseHeat(item.baseHeat);
                    }
                    Game.Instance.CameraShake.Shake(item.shake, 0.1f);
                    PlaySFX();
                    return true;
                }
            }

            return false;
        }

        private void PlaySFX()
        {
            var sfx = GetComponentInChildren<AudioSource>();
            if (sfx != null)
            {
                sfx.Play();
            }
        }

        private void FireAmmo()
        {
            Game.Instance.Pool.FireBullet(item.ammoType, parent, firePoint.position, firePoint.forward, item.baseDamage);
        }
    }
}