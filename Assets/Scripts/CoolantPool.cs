using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class CoolantPool : MonoBehaviour
    {
        [SerializeField] private GameObject pool;
        [SerializeField] private new Collider collider;
        [SerializeField] private float coolantAmount = 50.0f;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Game.Instance.Player.Mech.IncreaseCoolant(coolantAmount);
                pool.SetActive(false);
                collider.enabled = false;
            }
        }
    }
}