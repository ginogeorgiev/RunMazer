using System;
using UnityEditor.UI;
using UnityEngine;

namespace Maze.Item
{
    public abstract class MazeItem : MonoBehaviour
    {
        [SerializeField] private int count = 0;

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

        //public int Count => count;
        public int Count
        {
            get => count;
            set => count = value;
        }

        public void ReplaceItem()
        {
            MazeCell cell = ItemGenerator.GetRandomEmptyCell();
            transform.parent = cell.transform;
            transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        }
    }
}