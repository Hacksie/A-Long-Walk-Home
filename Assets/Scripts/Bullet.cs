using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] public AmmoType ammoType;
        [SerializeField] public float speed;
        [SerializeField] public float baseTimeout = 1.5f;
        [SerializeField] public bool addForce = true;
        [SerializeField] public bool explode = true;
        private Rigidbody rb;
        private GameObject firer;
        private float timeout = 1.5f;
        private float damage = 0;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Fire(GameObject firer, float damage)
        {
            timeout = baseTimeout;
            this.firer = firer;
            this.damage = damage;
            rb.velocity = Vector3.zero;
            if (addForce)
            {
                rb.AddForce(transform.forward * speed, ForceMode.Impulse);
            }
        }

        private void Update()
        {
            timeout -= Time.deltaTime;
            if (timeout <= 0)
            {
                Explode(this.transform.position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // if (!other.isTrigger && other.gameObject != this.firer)
            // {
            //     if (other != null && other.CompareTag("Enemy"))
            //     {
            //         var e = other.GetComponentInParent<Enemy>();
            //         if (e != null)
            //         {
            //             e.Damage(this.damage);
            //         }
            //         else
            //         {
            //             Debug.LogError("untagged enemy error");
            //         }

            //     }
            //     if (other != null && this.firer != null && other.CompareTag("Player") && !this.firer.CompareTag("Player"))
            //     {
            //         GameManager.Instance.DamageArmour(damage);
            //     }
            //     if (other.gameObject != null && this.firer.gameObject != null && other.CompareTag("Base") && !this.firer.CompareTag("Player"))
            //     {
            //         GameManager.Instance.DamageBase(damage);
            //     }

            //     Explode(this.transform.position);
            // }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.isTrigger && other.collider.gameObject != this.firer)
            {
                if (other.collider.CompareTag("Enemy"))
                {
                    if (other.collider.TryGetComponent<Enemy>(out var e))
                    {
                        e.Damage(this.damage);
                    }
                    else
                    {
                        Debug.LogError("Enemy doesn't have enemy component", this);
                    }
                }
                if (other.collider.CompareTag("Player") && !this.firer.CompareTag("Player"))
                {
                    //GameManager.Instance.DamageArmour(damage);
                }
                Explode(this.transform.position);
            }
        }

        private void Explode(Vector3 position)
        {
            if (explode)
            {
                Game.Instance.Pool.SpawnMiniExplosion(position);
            }
            this.gameObject.SetActive(false);
        }
    }
}