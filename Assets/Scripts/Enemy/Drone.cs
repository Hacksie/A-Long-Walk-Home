using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(Enemy), typeof(UnityEngine.AI.NavMeshAgent))]
    public class Drone : MonoBehaviour
    {
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
        [SerializeField] private Enemy baseEnemy;
        [SerializeField] private float alertRadius = 50.0f;
        [SerializeField] private float minExplosionDamage = 20.0f;
        [SerializeField] private float maxExplosionDamage = 50.0f;

        void Awake()
        {
            agent = agent ?? GetComponent<UnityEngine.AI.NavMeshAgent>();
            baseEnemy = baseEnemy ?? GetComponent<Enemy>();
        }

        public void Pause()
        {
            agent.isStopped = true;
        }

        public void UpdateBehaviour()
        {
            var playerPosition = Game.Instance.Player.transform.position;
            var sqrDistanceToPlayer = (playerPosition - this.transform.position).sqrMagnitude;

            if (sqrDistanceToPlayer < (alertRadius * alertRadius))
            {
                agent.isStopped = false;
                agent.SetDestination(playerPosition);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                var dmg = Random.Range(minExplosionDamage, maxExplosionDamage) * (float)Mathf.Exp(Game.Instance.Data.currentLevel);
                Game.Instance.Player.Mech.Damage(dmg, true, true);
                baseEnemy.Explode();
            }
        }
    }
}