using UnityEngine;

namespace HackedDesign
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] public GameObject parent;
        [SerializeField] public WeaponType type;
        [SerializeField] public Transform firePoint;
        [SerializeField] public int ammo = 0;
        [SerializeField] public int maxAmmo = 0;
        [SerializeField] public AmmoType ammoType;
        [SerializeField] public float fireRate = 0;
        [SerializeField] public float damage = 10;
        [SerializeField] public float heat = 10;
        //[SerializeField] public Sprite sprite;
        [SerializeField] public bool isPlayer = false;
        [SerializeField] public WeaponSettings settings;
        [SerializeField] public InventoryItem item;

        private float nextFireTime = 0;

        public bool Fire()
        {
            if (Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                // if (ammoType == AmmoType.Claw)
                // {
                //     if (isPlayer)
                //     {
                //         ClawAttack();
                //     }
                // }
                // else
                // {
                //     FireAmmo();
                // }

                FireAmmo();

                if (isPlayer)
                {
                    Game.Instance.IncreaseHeat(heat);
                }
                Game.Instance.CameraShake.Shake(settings.shake, 0.1f);
                PlaySFX();
                return true;
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
            Game.Instance.Pool.FireBullet(ammoType, parent, firePoint.position, firePoint.forward, damage);
        }

        private void ClawAttack()
        {

            var colliders = Physics.OverlapSphere(firePoint.position, Game.Instance.Settings.clawRange);
            foreach (Collider c in colliders)
            {
                // if (c.gameObject.CompareTag("Enemy"))
                // {
                //     var e = c.GetComponentInParent<Enemy>();
                //     if (e != null)
                //     {
                //         e.Damage(this.damage);
                //     }
                //     else
                //     {
                //         Debug.LogError("untagged enemy error");
                //     }
                // }

            }
        }

    }
}