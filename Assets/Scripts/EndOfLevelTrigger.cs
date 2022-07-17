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
            if (other.CompareTag("Player"))
            {
                Game.Instance.Data.currentLevel += 1;
                if (Game.Instance.Data.currentLevel < Game.Instance.Settings.maxLevels)
                {
                    Game.Instance.SetLoading();
                }
                else
                {
                    Game.Instance.SetGameoverState();
                }
            }

        }
    }
}