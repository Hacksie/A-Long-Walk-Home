using UnityEngine;


namespace HackedDesign
{
    [RequireComponent(typeof(Enemy), typeof(UnityEngine.AI.NavMeshAgent))]
    public class Helicopter : MonoBehaviour
    {
        [SerializeField] private MechController controller;
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
        [SerializeField] private Enemy baseEnemy;
        [SerializeField] private float alertRadius = 25.0f;
        [SerializeField] private float attackRadius = 10.0f;
        [SerializeField] private float explosionDamage = 40.0f;
        [SerializeField] private float readjustTime = 5.0f;
        [SerializeField] private float turretrotateSpeed = 90;
        [SerializeField] private Transform turret;

        private bool attacking;

        void Awake()
        {
            agent = agent ?? GetComponent<UnityEngine.AI.NavMeshAgent>();
            baseEnemy = baseEnemy ?? GetComponent<Enemy>();
        }

        public void UpdateBehaviour()
        {
            var playerPosition = Game.Instance.Player.transform.position;
            var sqrDistanceToPlayer = (playerPosition - this.transform.position).sqrMagnitude;


            if (sqrDistanceToPlayer < (attackRadius * attackRadius))
            {
                TurretLookAt(playerPosition);
                //turret.LookAt(playerPosition);
                controller?.FirePrimaryWeapon();
            }
            else if (sqrDistanceToPlayer < (alertRadius * alertRadius))
            {
                if (attacking == false)
                {
                    TurretLookAt(playerPosition);
                    agent.SetDestination(playerPosition);
                }
            }
        }

        public void TurretLookAt(Vector3 target)
        {
            var rotation = Quaternion.LookRotation((target - transform.position), Vector3.up);
            //var forward = .normalized;
            var targetAngle = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            //turret.transform.forward = new Vector3(forward.x, 0, forward.z);
            turret.rotation = Quaternion.Lerp(turret.rotation, targetAngle, turretrotateSpeed * Time.deltaTime);
        }

        void OnCollisionEnter(Collision collision)
        {
            // if(collision.collider.CompareTag("Player"))
            // {
            //     Game.Instance.Player.Mech.DamageArmour(explosionDamage, true);
            //     Debug.Log("Drone explode", this);
            //     baseEnemy.Explode();
            // }
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