using UnityEngine;

namespace Maze.Item
{
    public abstract class MazeItem : MonoBehaviour
    {
        
        [SerializeField]private int count;

        protected abstract void EnterEffect();
        protected abstract void ExitEffect();
        
        protected abstract void StayEffect();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EnterEffect();
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ExitEffect();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StayEffect();
            }
        }

        //************** GETTERS & SETTERS ************//
        
        public int Count => count;
    }
}