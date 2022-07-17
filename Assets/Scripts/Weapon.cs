using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] public GameObject parent;
        [SerializeField] public Transform firePoint;
        [SerializeField] public AudioSource sfx;
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
                    

                    FireAmmo();

                    if (isPlayer)
                    {
                        nextFireTime = Time.time + (item.baseFireRate * (1 + (Game.Instance.Player.Mech.Overdriven ? Game.Instance.Player.Mech.OverdriveMultiplier : 0))); // FIXME: 
                        Game.Instance.Player.Mech.IncreaseHeat(item.baseHeat);
                    }
                    else
                    {
                        nextFireTime = Time.time + item.baseFireRate;
                    }
                    Game.Instance.CameraShake.Shake(item.shake, 0.1f);
                    PlaySFX(item);
                    return true;
                }
            }

            return false;
        }

        private void PlaySFX(AudioClip clip, float pitch)
        {
            if (sfx != null && clip != null)
            {
                sfx.pitch = pitch;
                sfx.clip = clip;
                sfx.Play();
            }
        }

        private void PlaySFX(InventoryItem item)
        {
            PlaySFX(item.sfx, item.sfxPitch);
        }

        private void FireAmmo()
        {
            var dmg = Random.Range(item.baseMinDamage, item.baseMaxDamage);
            if(!isPlayer)
            {
                dmg = dmg * (0.5f + (0.25f * PlayerPreferences.Instance.difficulty));
            }
            // FIXME: Elec damage
            Game.Instance.Pool.FireBullet(item.ammoType, parent, firePoint.position, firePoint.forward, dmg, 0);
        }
    }
}