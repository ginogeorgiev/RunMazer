using Survival;
using UnityEngine;

namespace Maze.Item
{
    
    /// <summary>
    /// increases health currentValue by given value
    /// </summary>
    public class MedKit : MazeItem
    {
        [SerializeField] private float healthValueEffect = 20f;
        
        protected override void EnterEffect()
        {
            CoreBars.HealthCore.CurrentValue += healthValueEffect;
            
            Destroy(gameObject);
        }

        protected override void ExitEffect()
        {
            
        }

        protected override void StayEffect()
        {
            
        }
    }
}