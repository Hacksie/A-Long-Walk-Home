using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HackedDesign
{
    public class Pool : MonoBehaviour
    {

        [SerializeField] Transform parent;
        [SerializeField] List<Bullet> bulletPrefabs;

        [SerializeField] ParticleSystem miniExplosionPrefab = null;
        [SerializeField] ParticleSystem explosionPrefab = null;

        private List<Bullet> bulletPool = new List<Bullet>();


        private List<ParticleSystem> miniExplosionPool = new List<ParticleSystem>();
        private List<ParticleSystem> explosionPool = new List<ParticleSystem>();

        void Awake()
        {
            if (!parent)
            {
                parent = this.transform;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Destroy(parent.transform.GetChild(i));
            }
            bulletPool.Clear();
            explosionPool.Clear();
            miniExplosionPool.Clear();
        }

        public void SpawnExplosion(Vector3 position)
        {
            ParticleSystem explosion = explosionPool.FirstOrDefault(e => !e.isPlaying);

            if (explosion == null)
            {
                var go = Instantiate(explosionPrefab.gameObject, position, Quaternion.identity, parent);
                explosion = go.GetComponent<ParticleSystem>();
                explosionPool.Add(explosion);
            }

            var sfx = explosion.GetComponentInChildren<AudioSource>();
            if (sfx != null)
            {
                sfx.Play();
            }

            explosion.transform.position = position;
            explosion.Play();
        }

        public void SpawnMiniExplosion(Vector3 position)
        {
            ParticleSystem explosion = miniExplosionPool.FirstOrDefault(e => !e.isPlaying);

            if (explosion == null)
            {
                var go = Instantiate(miniExplosionPrefab.gameObject, position, Quaternion.identity, parent);
                explosion = go.GetComponent<ParticleSystem>();
                miniExplosionPool.Add(explosion);
            }

            var sfx = explosion.GetComponentInChildren<AudioSource>();
            if (sfx != null)
            {
                sfx.Play();
            }

            explosion.transform.position = position;
            explosion.Play();
        }

        public void FireBullet(AmmoType ammoType, GameObject firer, Vector3 start, Vector3 forward, float damage, float elecDamage)
        {
            Bullet bullet = GetPooledAmmo(ammoType);

            if (bullet == null)
            {
                var prefab = ChoosePrefab(ammoType);
                if (prefab == null)
                {
                    Debug.LogError("bullet prefab is null", this);
                    return;
                }

                bullet = Instantiate(prefab, start, Quaternion.identity, parent);
                // if (!go.TryGetComponent<Bullet>(out bullet))
                // {
                //     Debug.LogError("No bullet component", this);
                //     return;
                // }
                bulletPool.Add(bullet);
            }

            bullet.gameObject.SetActive(true);
            bullet.transform.position = start;
            bullet.transform.forward = forward;
            bullet.Fire(firer, damage, elecDamage);
        }

        private Bullet GetPooledAmmo(AmmoType ammoType)
        {
            return bulletPool.FirstOrDefault(b => b.ammoType == ammoType && !b.gameObject.activeInHierarchy);
        }

        private void AddToPool(Bullet b)
        {
            bulletPool.Add(b);
        }

        private Bullet ChoosePrefab(AmmoType ammoType)
        {
            var b = bulletPrefabs.FirstOrDefault(b => b.ammoType == ammoType);
            if (b != null)
            {
                return b;
            }
            return null;
        }
    }
}
