using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Item
{
    public class Bread : Item
    {
        private bool isRotten = false;
        /// <summary>
        /// bread has 30% chance to be rotten
        /// </summary>
        private void Awake()
        {
            if (Random.value < 0.3f)
            {
                GetComponent<Renderer>().material.color = new Color(0.24f,0.37f,0.0f);
                isRotten = true;
            }
        }

        protected override void EnterEffect()
        {
            //TODO fill up hunger bar if it isnt rotten, if it is then deplete hunger faster
            if (!isRotten)
                Debug.Log("Yum");
            else
            {
                Debug.Log("not so yum");
            }
            Destroy(gameObject);
        }
        //i need to implement these functions even if we dont use them
        protected override void ExitEffect()
        {
            
        }

        protected override void StayEffect()
        {
            
        }
    }
}