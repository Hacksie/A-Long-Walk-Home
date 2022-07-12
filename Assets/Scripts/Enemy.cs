using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private int minLevel = 0;
        [SerializeField] private Transform explosionPoint;
        [SerializeField] private EnemyType enemyType;
        
        [SerializeField] private UnityEvent behaviour;

        public EnemyType EnemyType { get => enemyType; set => enemyType = value; }

        public void UpdateBehaviour()
        {
            behaviour.Invoke();
        }

        public void Damage(float amount)
        {
            // currentHealth -= amount;

            // if (currentHealth <= 0)
            // {
                Debug.Log("Enemy destroyed");
                Explode();
            // }
        }


        public void Explode()
        {
            Game.Instance.Enemies.SpawnDestroyedEnemy(this);
            Game.Instance.Pool.SpawnExplosion(explosionPoint.position);
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
}