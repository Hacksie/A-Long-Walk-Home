#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Referenced Game Objects")]
        [SerializeField] private Transform? enemyParent;
        [Header("Settings")]
        //[SerializeField] private int xScale = 500;
        //[SerializeField] private int zScale = 500;
        // [SerializeField] private int safeX = 33;
        // [SerializeField] private int safeZ = 33;
        [SerializeField] private int minClusterSize = 2;
        [SerializeField] private int maxClusterSize = 6;
        [SerializeField] private float clusterRadius = 30;
        [SerializeField] private float deadzone = 10.0f;
        [SerializeField] private LayerMask envMask;
        [SerializeField] private LayerMask envObsMask;
        [SerializeField] private Transform? bossPosition;

        [Header("Prefabs")]
        [SerializeField] private List<Enemy> enemyPrefabs = new List<Enemy>();
        [SerializeField] private List<DeadEnemy> deadEnemyPrefabs = new List<DeadEnemy>();
        [SerializeField] private List<Enemy> bossPrefabs = new List<Enemy>();
        [SerializeField] private List<Enemy> pickupPrefabs = new List<Enemy>();

        private List<Enemy> enemyPool = new List<Enemy>();

        void Awake()
        {
            if (enemyParent is null)
            {
                enemyParent = this.transform;
            }
        }

        public void Reset()
        {
            enemyPool.Clear();
            if (enemyParent is null)
            {
                Debug.LogWarning("enemyParent is not set", this);
                return;
            }
            for (int i = 0; i < enemyParent.transform.childCount; i++)
            {
                Destroy(enemyParent.transform.GetChild(i).gameObject);
            }

        }

        public void Spawn(int level, Settings settings)
        {
            Reset();
            Debug.Log("Spawning enemies");
            SpawnEnemies(settings.enemyCount + (level * 10), settings);
            Debug.Log("Spawning boss");
            SpawnBoss(level);
            Debug.Log("Spawning pickups");
            SpawnPickups(settings);

        }

        private void SpawnPickups(Settings settings)
        {
            int i = 0;
            int infiniteLoop = 0;
            while (i < settings.pickupCount)
            {
                infiniteLoop++;
                if (infiniteLoop > 1000)
                {
                    Debug.LogWarning("Breaking out of an infinite loop", this);
                    break;
                }

                var x = Random.Range(0, settings.worldSize.x - deadzone) + deadzone;
                var z = Random.Range(0, settings.worldSize.y - deadzone) + deadzone;

                var spawnPos = new Vector3(x, 0, z);

                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                var idx = Random.Range(0, pickupPrefabs.Count);
                
                var e = Instantiate(pickupPrefabs[idx], spawnPos, rotation, enemyParent);
                e.name = pickupPrefabs[idx].name;
                enemyPool.Add(e);
                e.Reset();

                infiniteLoop = 0;


                i++;
            }
        }

        private void SpawnBoss(int level)
        {
            var rotation = Quaternion.Euler(0, -45, 0);
            var e = Instantiate(bossPrefabs[level], bossPosition.position, rotation, enemyParent);
            e.name = "Boss";
            enemyPool.Add(e);
            e.Reset();
        }

        private void SpawnEnemies(int enemyCount, Settings settings)
        {
            //Reset();
            int i = 0;
            int j = 0;
            int infiniteLoop = 0;
            int clusterSize = 0;
            Vector3 clusterPos = Vector3.zero;
            while (i < enemyCount)
            {
                infiniteLoop++;
                if (infiniteLoop > 1000)
                {
                    Debug.LogWarning("Breaking out of an infinite loop", this);
                    break;
                }

                if (j == 0)
                {
                    var x = Random.Range(0, settings.worldSize.x - deadzone) + deadzone;
                    var z = Random.Range(0, settings.worldSize.y - deadzone) + deadzone;
                    clusterSize = Random.Range(minClusterSize, maxClusterSize);

                    clusterPos = new Vector3(x, 0, z);

                    if (!IsSafeLocationToCluster(clusterPos, settings))
                    {
                        Debug.LogWarning("Not safe to cluster here " + clusterPos, this);
                        continue;
                    }
                }

                j++;

                if (j >= clusterSize)
                {
                    j = 0;
                }


                var offset = Random.insideUnitCircle * clusterRadius;
                var spawnPos = new Vector3(clusterPos.x, 0, clusterPos.z) + new Vector3(offset.x, 0, offset.y);

                if (!IsSafeLocationToSpawn(spawnPos, settings))
                {
                    Debug.LogWarning("Unsafe position to spawn: " + spawnPos, this);
                    continue;
                }

                var idx = Random.Range(0, enemyPrefabs.Count);

                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                var e = Instantiate(enemyPrefabs[idx], spawnPos, rotation, enemyParent);
                e.name = enemyPrefabs[idx].name;

                enemyPool.Add(e);
                e.Reset();

                i++;

                infiniteLoop = 0;
            }
        }

        public void SpawnDestroyedEnemy(EnemyType enemyType, Vector3 position)
        {
            var prefab = deadEnemyPrefabs.FirstOrDefault(p => p.EnemyType == enemyType);

            if (prefab != null)
            {
                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);

                var e = Instantiate(prefab, position, rotation, enemyParent);
                e.GenerateLoot();
            }
        }

        public void UpdateBehaviour()
        {
            foreach (var enemy in enemyPool)
            {
                if (enemy.gameObject.activeInHierarchy)
                {
                    enemy.UpdateBehaviour();
                }
            }
        }

        private bool IsSafeLocationToCluster(Vector3 position, Settings settings)
        {
            if (position.x <= settings.safeArea.x && position.z <= settings.safeArea.y)
            {
                return false;
            }
            return true;
        }

        private bool IsSafeLocationToSpawn(Vector3 position, Settings settings)
        {
            if (position.x <= settings.safeArea.x && position.z <= settings.safeArea.y)
            {
                return false;
            }

            // Don't spawn off the map
            if (position.x < deadzone || position.z < deadzone || position.x >= (settings.worldSize.x - deadzone) || position.z >= (settings.worldSize.y - deadzone))
            {
                return false;
            }

            //if(Physics.SphereCast(Vector3 position, ))
            if (Physics.CheckSphere(position, 1.5f, envObsMask, QueryTriggerInteraction.Ignore))
            {
                Debug.LogWarning("Obstacle overlap");
                return false;
            }

            return true;
        }
    }
}