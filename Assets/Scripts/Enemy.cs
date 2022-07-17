using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{

    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform healthBar;
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private MechController mech;

        [SerializeField] private UnityEvent behaviour;
        [SerializeField] private LayerMask envMask;
        [Header("Settings")]
        [SerializeField] public float health;
        [SerializeField] public WeaponType nose;
        [SerializeField] public WeaponType leftArm;
        [SerializeField] public WeaponType rightArm;
        [SerializeField] public WeaponType leftShoulder;
        [SerializeField] public WeaponType rightShoulder;

        private float currentHealth;
        private float maxHealth;
        [SerializeField] private EnemyState state;

        public EnemyType EnemyType { get => enemyType; set => enemyType = value; }
        public EnemyState State { get => state; set => state = value; }

        public void Reset()
        {
            maxHealth = currentHealth = health * (float)Mathf.Exp(Game.Instance.Data.currentLevel);
            ItemLevel itemLevel = ItemLevel.Scrap;
            if (Game.Instance.Data.currentLevel <= 4)
            {
                itemLevel = (ItemLevel)Game.Instance.Data.currentLevel;
            }
            else
            {
                itemLevel = ItemLevel.Epic;
            }

            UpdateItemLevels(itemLevel);
        }

        private void UpdateItemLevels(ItemLevel itemLevel)
        {
            if (mech != null)
            {
                for (int i = 0; i < (int)MechPosition.Nose; i++)
                {
                    var item = mech.GetItem((MechPosition)i);
                    if (item is not null)
                    {
                        item = item.Minimize(itemLevel);
                        mech.SetItem((MechPosition)i, item);
                    }
                }
            }
        }

        public void UpdateBehaviour()
        {
            behaviour.Invoke();
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
            {
                var scale = healthBar.localScale;
                scale.x = currentHealth / maxHealth;
                healthBar.localScale = scale;
                healthBar.transform.LookAt(Game.Instance.MainCamera.transform);
            }
        }

        public void Damage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Game.Instance.AddConsoleMessage(this.name + " exploded");
                Explode();
            }
        }


        public void Explode()
        {
            var spawnPos = new Vector3(transform.position.x, 0.5f, transform.position.z);

            if (Physics.Raycast(transform.position, Vector3.up, out var raycastHit, 10.0f, envMask))
            {
                spawnPos = raycastHit.point;
            }

            Game.Instance.Enemies.SpawnDestroyedEnemy(enemyType, spawnPos);
            Game.Instance.Pool.SpawnExplosion(spawnPos);
            gameObject.SetActive(false);
        }
    }

    public enum EnemyType
    {
        Drone,
        Plane,
        Helicopter,
        Tank,
        Artillery,
        OrbitalDrop,
        Mech,
        Pickup
    }

    public enum EnemyState
    {
        Idle,
        Patrol,
        Alert,
        Attack,
        Adjust
    }
}