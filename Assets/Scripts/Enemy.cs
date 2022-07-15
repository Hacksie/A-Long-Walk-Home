using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{

    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform healthBar;
        [SerializeField] private EnemyType enemyType;

        [SerializeField] private UnityEvent behaviour;
        [SerializeField] private LayerMask envMask;
        [Header("Settings")]
        [SerializeField] public float health;

        private float currentHealth;
        private EnemyState state;

        public EnemyType EnemyType { get => enemyType; set => enemyType = value; }
        public EnemyState State { get => state; set => state = value; }

        public void Reset()
        {
            currentHealth = health * (float)Math.Exp(Game.Instance.Data.currentLevel);
        }

        public void UpdateBehaviour()
        {
            behaviour.Invoke();
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            var scale = healthBar.localScale;
            scale.x = currentHealth / health;
            if (healthBar != null)
            {
                healthBar.localScale = scale;
                healthBar.transform.LookAt(Game.Instance.MainCamera.transform);
            }
            else
            {
                Debug.LogWarning("Healthbar not set", this);
            }
        }

        public void Damage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
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
        Mech
    }

    public enum EnemyState
    {
        Idle,
        Patrol,
        Attack,
        Adjust
    }
}