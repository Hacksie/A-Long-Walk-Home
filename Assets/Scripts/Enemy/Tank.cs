using UnityEngine;


namespace HackedDesign
{
    [RequireComponent(typeof(Enemy), typeof(UnityEngine.AI.NavMeshAgent))]
    public class Tank : MonoBehaviour
    {
        
        [SerializeField] private MechController controller;
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
        [SerializeField] private Enemy baseEnemy;
        [SerializeField] private float alertRadius = 50.0f;
        [SerializeField] private float attackRadius = 15.0f;
        [SerializeField] private float rotateSpeed = 60;
        [SerializeField] private Transform turret;

        void Awake()
        {
            agent = agent ?? GetComponent<UnityEngine.AI.NavMeshAgent>();
            baseEnemy = baseEnemy ?? GetComponent<Enemy>();
            baseEnemy.State = EnemyState.Patrol;
        }

        public void UpdateBehaviour()
        {
            var playerPosition = Game.Instance.Player.transform.position;
            var sqrDistanceToPlayer = (playerPosition - this.transform.position).sqrMagnitude;
        
            switch(baseEnemy.State)
            {
                case EnemyState.Idle:
                break;
                case EnemyState.Patrol:
                break;
                case EnemyState.Attack:
                break;
                case EnemyState.Adjust:
                break;
            }

            if(sqrDistanceToPlayer < (attackRadius * attackRadius))
            {
                TurretLookAt(playerPosition);
                //turret.LookAt(playerPosition);
                controller?.FirePrimaryWeapon();
            }
            else if(sqrDistanceToPlayer < (alertRadius * alertRadius))
            {
                TurretLookAt(playerPosition);
                agent.SetDestination(playerPosition);
            }

            
        }

        public void TurretLookAt(Vector3 target)
        {
            var rotation = Quaternion.LookRotation((target - transform.position), Vector3.up);
            //var forward = .normalized;
            var targetAngle = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            //turret.transform.forward = new Vector3(forward.x, 0, forward.z);
            turret.rotation = Quaternion.Lerp(turret.rotation, targetAngle, rotateSpeed * Time.deltaTime);        
        }



    }
}