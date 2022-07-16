using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HackedDesign
{
    public class Radar : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;

        void Update()
        {
            if (Game.Instance.State.Playing)
            {
                var h = (Game.Instance.Player.Mech.RadarRange * 2) * (Mathf.Sqrt(3) / 2);
                transform.position = followTarget.position + new Vector3(0, h, 0);
            }
        }
    }
}