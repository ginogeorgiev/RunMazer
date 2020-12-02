using Survival;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze.Item
{
    public class Bread : MazeItem
    {
        [SerializeField] private float healthValueEffect = 20f;
        private bool isRotten = false;
        
        //bread has a 30% chance to be rotten
        private void Awake()
        {
            if (Random.value < 0.3f)
            {
                GetComponent<Renderer>().material.color = new Color(0.24f,0.37f,0.0f);
                isRotten = true;
            }
            
        }
        /// <summary>
        /// if its not rotten your hunger bar goes up by 20
        /// if its rotten and hunger bar is not 0 then you lose 10 hunger
        /// if its rotten and hunger bar is 0 then you lose 10 hp
        /// </summary>
        protected override void EnterEffect()
        {
            if (!isRotten)
                CoreBars.HungerCore.CurrentValue += healthValueEffect;
            else
            {
                if (CoreBars.HungerCore.CurrentValue > 0.0f)
                    CoreBars.HungerCore.CurrentValue -= healthValueEffect / 2;
                else
                    CoreBars.HealthCore.CurrentValue -= healthValueEffect / 2;
                
            }
            
            Destroy(gameObject);
        }
        //we need to implement these functions even if we dont use them
        protected override void ExitEffect()
        {
            
        }

        protected override void StayEffect()
        {
            
        }
    }
}