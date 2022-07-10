using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "State/Dialog")]
    public class Dialog : ScriptableObject
    {
        public List<string> lines;
    }
}