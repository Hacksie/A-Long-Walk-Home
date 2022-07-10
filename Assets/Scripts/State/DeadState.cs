using UnityEngine;

namespace HackedDesign
{
    public class DeadState : IState
    {
        public DeadState()
        {
        }

        public bool Playing => false;

        public void Begin()
        {
            Debug.Log("Dead");
        }

        public void End()
        {
            
        }

        public void FixedUpdate()
        {
            
        }

        public void Start()
        {
            
        }

        public void Select()
        {
            
        }

        public void Update()
        {
            
        }
    }

}