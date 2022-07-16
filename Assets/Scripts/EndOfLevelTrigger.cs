using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    public class EndOfLevelTrigger : MonoBehaviour
    {
        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Debug.Log("end of level trigger");
                Game.Instance.Data.currentLevel += 1;
                Game.Instance.SetLoading();
            }
            
        }
    }
}