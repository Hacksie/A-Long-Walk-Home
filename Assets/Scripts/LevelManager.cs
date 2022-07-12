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
        [SerializeField] private CoolantPool coolantPoolPrefab;
        //[SerializeField] private int safeX = 33;
        //[SerializeField] private int safeZ = 33;
        

        [SerializeField] private Unity.AI.Navigation.NavMeshSurface surface;
        //[SerializeField] private Unity.AI.Navigation.
        [SerializeField] private LineRenderer path;

        [SerializeField] private Transform start;
        [SerializeField] private Transform target;

        public void SpawnLevel(Settings settings)
        {
            UpdateTerrain(settings);
            SpawnObstacles(settings);
            SpawnCoolant(settings);
        }

        private void UpdateTerrain(Settings settings)
        {
            //terrain.
        }

        private void SpawnCoolant(Settings settings)
        {
            for(int i=0;i<settings.coolantCount;i++)
            {
                var x = Random.Range(settings.deadzone, settings.worldSize.x - settings.deadzone);
                var z = Random.Range(settings.deadzone, settings.worldSize.y - settings.deadzone);
                var spawnPos = new Vector3(x,0,z);                
                var go = Instantiate(coolantPoolPrefab, spawnPos, Quaternion.identity, obstacleParent);
            }
        }        

        private void SpawnObstacles(Settings settings)
        {
            DestroyObstacles();

            UnityEngine.AI.NavMeshPath navpath = new NavMeshPath();

            int i = 0;
            int infiniteLoop = 0;
            List<GameObject> lastObjects = new List<GameObject>();

            bool canPath = true;

            while (i < settings.obstacleCount)
            {
                infiniteLoop++;
                if(infiniteLoop > 1000)
                {
                    Debug.LogWarning("Infinite loop, breaking");
                    return;
                }
                var x = Random.Range(settings.deadzone, settings.worldSize.x - settings.deadzone);
                var z = Random.Range(settings.deadzone, settings.worldSize.y - settings.deadzone);
                var spawnPos = new Vector3(x,0,z);
                if(!IsSafeLocationToSpawn(spawnPos, settings))
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

        private bool IsSafeLocationToSpawn(Vector3 position, Settings settings)
        {
            if (position.x <= settings.safeArea.x && position.z <= settings.safeArea.y) // FIXME: This could be an infinite loop
            {
                return false;
            }

            // Don't spawn off the map
            if(position.x >= settings.worldSize.x || position.z >= settings.worldSize.y)
            {
                return false;
            }

            return true;
        }        
    }
}