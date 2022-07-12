using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HackedDesign
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Referenced Game Objects")]
        [SerializeField] private Transform enemyParent;
        [Header("Settings")]
        //[SerializeField] private int xScale = 500;
        //[SerializeField] private int zScale = 500;
        // [SerializeField] private int safeX = 33;
        // [SerializeField] private int safeZ = 33;
        [SerializeField] private int minClusterSize = 2;
        [SerializeField] private int maxClusterSize = 6;
        [SerializeField] private float clusterRadius = 30;
        [SerializeField] private float deadzone = 10.0f;


        [Header("Prefabs")]
        [SerializeField] private List<Enemy> enemyPrefabs;
        [SerializeField] private List<DeadEnemy> deadEnemyPrefabs;

        private List<Enemy> enemyPool = new List<Enemy>();

        public void ClearEnemies()
        {
            for (int i = 0; i < enemyParent.transform.childCount; i++)
            {
                Destroy(enemyParent.transform.GetChild(i).gameObject);
            }
            enemyPool.Clear();
        }

        public void SpawnEnemies(int enemyCount, Settings settings)
        {
            ClearEnemies();
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
                    Debug.Log("Spawning enemy cluster of " + clusterSize, this);

                    clusterPos = new Vector3(x, 0, z);

                    if (!IsSafeLocationToCluster(clusterPos, settings)) // FIXME: This could be an infinite loop
                    {
                        Debug.Log("Not safe to cluster here", this);
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

                if (!IsSafeLocationToSpawn(spawnPos, settings)) // FIXME: This could be an infinite loop
                {
                    Debug.Log(spawnPos, this);
                    Debug.LogWarning("Unsafe position to spawn", this);
                    continue;
                }

                var idx = Random.Range(0, enemyPrefabs.Count);


                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                var go = Instantiate(enemyPrefabs[idx].gameObject, spawnPos, rotation, enemyParent);

                if (go.TryGetComponent<Enemy>(out var e))
                {
                    enemyPool.Add(e);
                }
                i++;

                infiniteLoop = 0;
            }
        }

        public void SpawnDestroyedEnemy(Enemy enemy)
        {
            var prefab = deadEnemyPrefabs.FirstOrDefault(p => p.EnemyType == enemy.EnemyType);

            if (prefab != null)
            {
                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                
                var e = Instantiate(prefab, enemy.transform.position, rotation, enemyParent);
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
            if (position.x <= settings.safeArea.x && position.z <= settings.safeArea.y) // FIXME: This could be an infinite loop
            {
                return false;
            }
            return true;
        }

        private bool IsSafeLocationToSpawn(Vector3 position, Settings settings)
        {
            if (position.x <= settings.safeArea.x && position.z <= settings.safeArea.y) // FIXME: This could be an infinite loop
            {
                return false;
            }

            // Don't spawn off the map
            if (position.x < deadzone || position.z < deadzone || position.x >= (settings.worldSize.x - deadzone) || position.z >= (settings.worldSize.y - deadzone))
            {
                return false;
            }

            return true;
        }
    }
}