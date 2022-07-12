using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    
    public class DeadEnemy : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private UnityEvent behaviour;
        [SerializeField] public List<InventoryItem> loot;

        public EnemyType EnemyType { get => enemyType; set => enemyType = value; }
    }
}