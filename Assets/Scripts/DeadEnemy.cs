using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{

    public class DeadEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] public List<InventoryItem> loot = new List<InventoryItem>();
        [SerializeField] public GameObject pickupIndicator;
        [SerializeField] public GameObject radarIndicator;

        private float cleanupTimer = Mathf.Infinity;

        public EnemyType EnemyType { get => enemyType; set => enemyType = value; }

        public bool HasLoot { get => loot.Count > 0; }

        public void GenerateLoot()
        {
            var settings = Game.Instance.Settings;

            if (Random.value < settings.lootChance)
            {
                Debug.Log("No loot", this);
                UpdatePickupIndicator();
                return;
            }
            loot = new List<InventoryItem>();
            InventoryItem item;

            // FIXME: Generate multiple loot
            if (Random.value >= settings.scrapChance)
            {
                // Generate scrap
                item = InventoryItem.RandomScrap();
                loot.Add(item);
                UpdatePickupIndicator();
                return;
            }

            item = InventoryItem.RandomItem();
            Debug.Log("Generated " + item.name + " " + item.itemLevel);
            loot = new List<InventoryItem>();
            loot.Add(item);
            UpdatePickupIndicator();
        }

        void Update()
        {
            if(Time.time >= cleanupTimer)
            {
                gameObject.SetActive(false);
            }
        }

        private void UpdatePickupIndicator()
        {
            pickupIndicator.SetActive(loot.Count > 0);
            radarIndicator.SetActive(loot.Count > 0);
            if(loot.Count == 0)
            {
                cleanupTimer = Time.time + 5f;
            }
        }

        public void PickupLoot(InventoryItem item)
        {
            loot.Remove(item);
            UpdatePickupIndicator();
        }
    }
}