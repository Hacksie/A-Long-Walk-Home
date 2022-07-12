using UnityEngine;


namespace HackedDesign
{
    [RequireComponent(typeof(Enemy), typeof(UnityEngine.AI.NavMeshAgent))]
    public class Tank : MonoBehaviour
    {
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
        [SerializeField] private Enemy baseEnemy;
        [SerializeField] private float alertRadius = 50.0f;
        [SerializeField] private float explosionDamage = 40.0f;

        void Awake()
        {
            agent = agent ?? GetComponent<UnityEngine.AI.NavMeshAgent>();
            baseEnemy = baseEnemy ?? GetComponent<Enemy>();
        }

        public void UpdateBehaviour()
        {
            var playerPosition = Game.Instance.Player.transform.position;
            var sqrDistanceToPlayer = (playerPosition - this.transform.position).sqrMagnitude;

            if(sqrDistanceToPlayer < (alertRadius * alertRadius))
            {
                agent.SetDestination(playerPosition);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.CompareTag("Player"))
            {
                Game.Instance.DamageArmour(explosionDamage);
                Debug.Log("Drone explode", this);
                baseEnemy.Explode();
            }
        }

        // void OnTriggerEnter(Collider other)
        // {
        //     if(other.CompareTag("Player"))
        //     {
        //         Game.Instance.DamageArmour(explosionDamage);
        //         Debug.Log("Drone explode", this);
        //         baseEnemy.Explode();
        //     }
        // }        
    }
}