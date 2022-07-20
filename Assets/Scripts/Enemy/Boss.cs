using System.Collections.Generic;
using UnityEngine;


namespace HackedDesign
{
    [RequireComponent(typeof(Enemy), typeof(UnityEngine.AI.NavMeshAgent))]
    public class Boss : MonoBehaviour
    {

        [SerializeField] private MechController controller;
        [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
        [SerializeField] private Animator? animator;
        [SerializeField] private Enemy baseEnemy;
        [SerializeField] private float alertRadius = 50.0f;
        [SerializeField] private float shootRadius = 17.0f;
        [SerializeField] private float meleeRadius = 5.0f;
        [SerializeField] private float adjustRadius = 1.0f;
        [SerializeField] private float rotateSpeed = 60;
        [SerializeField] private Transform turret;
        [SerializeField] private float idleTime = 10.0f;
        [SerializeField] private float patrolTime = 10.0f;
        [SerializeField] private float adjustTime = 1.0f;
        [SerializeField] private float patrolRadius = 25.0f;
        [SerializeField] private List<WeaponPosition> shootPositions = new List<WeaponPosition>();
        [SerializeField] private List<WeaponPosition> meleePositions = new List<WeaponPosition>();


        private float idleTimer = 0;
        private float patrolTimer = 0;
        private float adjustTimer = 0;
        public Vector3 patrolDestination = Vector3.zero;

        void Awake()
        {
            agent = agent ?? GetComponent<UnityEngine.AI.NavMeshAgent>();
            baseEnemy = baseEnemy ?? GetComponent<Enemy>();
            animator = animator ?? GetComponent<Animator>();
            SetIdle();
            idleTimer += (Random.value * 5.0f);
        }

        public void Pause()
        {
            SetIdle();
        }

        public void UpdateBehaviour()
        {
            var playerPosition = Game.Instance.Player.transform.position;
            var playerDirection = playerPosition - this.transform.position;
            var sqrDistanceToPlayer = playerDirection.sqrMagnitude;

            if (sqrDistanceToPlayer > 100)
            {
                SetIdle();
            }


            switch (baseEnemy.State)
            {
                case EnemyState.Idle:
                    if (sqrDistanceToPlayer < (alertRadius * alertRadius))
                    {
                        SetAlert(playerPosition);
                    }
                    else if (Time.time >= idleTimer)
                    {
                        SetPatrol();
                    }
                    break;
                case EnemyState.Patrol:
                    if (sqrDistanceToPlayer < (alertRadius * alertRadius))
                    {
                        SetAlert(playerPosition);
                    }
                    else if (Time.time >= patrolTimer)
                    {
                        SetIdle();
                    }
                    break;

                case EnemyState.Alert:
                    TurretLookAt(playerPosition);
                    if (sqrDistanceToPlayer < (shootRadius * shootRadius))
                    {
                        SetAttack(playerPosition);
                    }
                    else if (sqrDistanceToPlayer > (alertRadius * alertRadius))
                    {
                        SetAdjust();
                    }
                    break;

                case EnemyState.Attack:
                    TurretLookAt(playerPosition);
                    if (sqrDistanceToPlayer < (adjustRadius * adjustRadius))
                    {
                        SetAdjust();
                    }
                    else if (sqrDistanceToPlayer < (meleeRadius * meleeRadius))
                    {
                        if (Physics.Raycast(this.transform.position, playerDirection.normalized, out var hit, meleeRadius))
                        {
                            if (hit.collider.CompareTag("Player"))
                            {
                                var rndIdx = Random.Range(0, meleePositions.Count);
                                controller?.FireWeapon(meleePositions[rndIdx]);
                            }
                            else
                            {
                                SetAdjust();
                            }
                        }
                    }
                    else if (sqrDistanceToPlayer < (shootRadius * shootRadius))
                    {
                        if (Physics.Raycast(this.transform.position, playerDirection.normalized, out var hit, shootRadius))
                        {
                            if (hit.collider.CompareTag("Player"))
                            {
                                var rndIdx = Random.Range(0, shootPositions.Count);
                                controller?.FireWeapon(shootPositions[rndIdx]);
                            }
                            else
                            {
                                SetAdjust();
                            }
                        }
                    }
                    else
                    {
                        SetAdjust();
                    }
                    break;

                case EnemyState.Adjust:
                    if (Time.time >= adjustTimer)
                    {
                        SetIdle();
                    }
                    break;
            }

            Animate(agent.velocity.normalized);
        }

        private void SetIdle()
        {
            baseEnemy.State = EnemyState.Idle;
            idleTimer = Time.time + idleTime;
            agent.isStopped = true;
        }

        private void SetPatrol()
        {
            baseEnemy.State = EnemyState.Patrol;
            patrolTimer = Time.time + patrolTime;
            var nextPos = Random.insideUnitCircle * patrolRadius;
            patrolDestination = transform.position + new Vector3(nextPos.x, 0, nextPos.y);
            agent.isStopped = false;
            agent.SetDestination(patrolDestination);
        }

        private void SetAlert(Vector3 playerPosition)
        {
            baseEnemy.State = EnemyState.Alert;
            TurretLookAt(playerPosition);
            agent.isStopped = false;
            agent.SetDestination(playerPosition);
        }

        private void SetAttack(Vector3 playerPosition)
        {
            baseEnemy.State = EnemyState.Attack;
            TurretLookAt(playerPosition);
            agent.isStopped = true;
            agent.SetDestination(playerPosition);
        }

        private void SetAdjust()
        {
            adjustTimer = Time.time + adjustTime;
            baseEnemy.State = EnemyState.Adjust;
            agent.SetDestination(transform.position + (transform.right * (Random.value < 0.5 ? -1 : 1) * alertRadius));
            agent.isStopped = false;
        }

        public void TurretLookAt(Vector3 target)
        {
            var rotation = Quaternion.LookRotation((target - transform.position), Vector3.up);
            //var forward = .normalized;
            var targetAngle = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            //turret.transform.forward = new Vector3(forward.x, 0, forward.z);
            turret.rotation = Quaternion.Lerp(turret.rotation, targetAngle, rotateSpeed * Time.deltaTime);
        }

        private void Animate(Vector2 movement)
        {
            animator?.SetFloat("Rotation", movement.x);
            animator?.SetFloat("Speed", movement.y);
        }
    }
}