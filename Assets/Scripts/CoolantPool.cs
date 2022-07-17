using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class CoolantPool : MonoBehaviour
    {
        [SerializeField] private GameObject pool;
        [SerializeField] private new Collider collider;
        [SerializeField] private float minCoolantAmount = 10.0f;
        [SerializeField] private float maxCoolantAmount = 50.0f;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var amount = Random.Range(minCoolantAmount, maxCoolantAmount);
                Game.Instance.Player.Mech.IncreaseCoolant(amount);
                Game.Instance.AddConsoleMessage("Coolant pickedup: " + amount.ToString("N0"));
                this.gameObject.SetActive(false);
            }
        }
    }
}