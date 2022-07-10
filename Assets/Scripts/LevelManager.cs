using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

namespace HackedDesign
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform terrain;
        //[SerializeField] private Transform terrainBoundary
        [SerializeField] private Transform obstacleParent;
        [SerializeField] private List<GameObject> ObstaclePrefabs;
        [SerializeField] private int obstacleCount = 100;
        //[SerializeField] private int xScale = 500;
        //[SerializeField] private int zScale = 500;
        [SerializeField] private int safeX = 33;
        [SerializeField] private int safeZ = 33;
        

        [SerializeField] private Unity.AI.Navigation.NavMeshSurface surface;
        //[SerializeField] private Unity.AI.Navigation.
        [SerializeField] private LineRenderer path;

        [SerializeField] private Transform start;
        [SerializeField] private Transform target;

        public void SpawnLevel(Settings settings)
        {
            UpdateTerrain(settings);
            SpawnObstacles(settings);
        }

        private void UpdateTerrain(Settings settings)
        {
            //terrain.
        }

        private void SpawnObstacles(Settings settings)
        {
            DestroyObstacles();

            UnityEngine.AI.NavMeshPath navpath = new NavMeshPath();

            int i = 0;
            List<GameObject> lastObjects = new List<GameObject>();

            bool canPath = true;

            while (i < (settings.worldSize.x + settings.worldSize.y))
            {
                var x = Random.Range(0, settings.worldSize.x);
                var z = Random.Range(0, settings.worldSize.x);
                var spawnPos = new Vector3(x,0,z);
                if(!IsSafeLocationToSpawn(spawnPos, settings.worldSize))
                {
                    continue;
                }
                var idx = Random.Range(0, ObstaclePrefabs.Count);
                var rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                var go = Instantiate(ObstaclePrefabs[idx], spawnPos, rotation, obstacleParent);
                lastObjects.Add(go);
                i++;

                if (i % 50 == 0)
                {
                    surface.BuildNavMesh();
                    canPath = NavMesh.CalculatePath(Game.Instance.Player.transform.position, target.position, 1, navpath);
                    if (!canPath)
                    {
                        Debug.Log("Can't path, reverting", this);
                        for (int j = 0; j < lastObjects.Count; j++)
                        {
                            Destroy(lastObjects[j]);
                        }
                        i -= 50;
                    }
                    lastObjects.Clear();
                }
            }

            surface.BuildNavMesh();
            canPath = NavMesh.CalculatePath(Game.Instance.Player.transform.position, target.position, 1, navpath);
            if (!canPath)
            {
                Debug.Log("Can't path, reverting", this);
                for (int j = 0; j < lastObjects.Count; j++)
                {
                    Destroy(lastObjects[j]);
                }
            }

            lastObjects.Clear();
            GeneratePath(navpath);
        }

        private void DestroyObstacles()
        {
            for (int i = 0; i < obstacleParent.transform.childCount; i++)
            {
                Destroy(obstacleParent.transform.GetChild(i).gameObject);
            }
        }

        private void GeneratePath(UnityEngine.AI.NavMeshPath navpath)
        {
            path.positionCount = navpath.corners.Length;
            path.SetPositions(navpath.corners);
        }

        private bool IsSafeLocationToSpawn(Vector3 position, Vector2 worldSize)
        {
            if (position.x <= safeX && position.z <= safeZ) // FIXME: This could be an infinite loop
            {
                return false;
            }

            // Don't spawn off the map
            if(position.x >= worldSize.x || position.z >= worldSize.y)
            {
                return false;
            }

            return true;
        }        
    }
}