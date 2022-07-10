using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private float alertRadius = 50.0f;
        [SerializeField] private int minLevel = 0;
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;

        void Awake()
        {
            agent = agent ?? GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        public void UpdateBehaviour()
        {
            var playerPosition = Game.Instance.Player.transform.position;
            var sqrDistanceToPlayer = (playerPosition - this.transform.position).sqrMagnitude;

            if(sqrDistanceToPlayer < (alertRadius * alertRadius))
            {
                Debug.Log("Enemy is attacking player", this);
                agent.SetDestination(playerPosition);
            }

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


        private void Explode()
        {
            //Game.Instance.Enemies.SpawnDestroyedEnemy(this);
            Game.Instance.Pool.SpawnExplosion(this.transform.position);
            gameObject.SetActive(false);
        }        
    }
}