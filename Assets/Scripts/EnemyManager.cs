using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("Referenced Game Objects")]
        [SerializeField] private Transform enemyParent;
        [Header("Settings")]
        [SerializeField] private int xScale = 500;
        [SerializeField] private int zScale = 500;
        [SerializeField] private int safeX = 33;
        [SerializeField] private int safeZ = 33;
        [SerializeField] private int minClusterSize = 2;
        [SerializeField] private int maxClusterSize = 6;
        [SerializeField] private float clusterRadius = 30;


        [Header("Prefabs")]
        [SerializeField] private List<Enemy> enemyPrefabs;

        private List<Enemy> enemyPool = new List<Enemy>();

        public void ClearEnemies()
        {
            for (int i = 0; i < enemyParent.transform.childCount; i++)
            {
                Destroy(enemyParent.transform.GetChild(i).gameObject);
            }
            enemyPool.Clear();
        }

        public void SpawnEnemies(int enemyCount)
        {
            ClearEnemies();
            int i = 0;
            while (i < enemyCount)
            {

                var clusterSize = Random.Range(minClusterSize, maxClusterSize);
                var x = Random.Range(0, xScale - 5) + 5;
                var z = Random.Range(0, zScale - 5) + 5;
                Debug.Log("Spawning enemy cluster of " + clusterSize, this);

                var clusterPos = new Vector3(x, 0, z);

                if (!IsSafeLocationToCluster(clusterPos)) // FIXME: This could be an infinite loop
                {
                    Debug.Log("Not safe to cluster here", this);
                    continue;
                }


                for (int j = 0; j < clusterSize; j++)
                {
                    var offset = Random.insideUnitCircle * clusterRadius;
                    var spawnPos = new Vector3(clusterPos.x, 0, clusterPos.z) + new Vector3(offset.x, 0, offset.y);

                    if (!IsSafeLocationToSpawn(spawnPos)) // FIXME: This could be an infinite loop
                    {
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
                }
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

        private bool IsSafeLocationToCluster(Vector3 position)
        {
            if (position.x <= safeX && position.z <= safeZ) // FIXME: This could be an infinite loop
            {
                return false;
            }
            return true;
        }

        private bool IsSafeLocationToSpawn(Vector3 position)
        {
            if (position.x <= safeX && position.z <= safeZ) // FIXME: This could be an infinite loop
            {
                return false;
            }

            // Don't spawn off the map
            if (position.x >= xScale || position.z >= zScale)
            {
                return false;
            }

            return true;
        }
    }
}