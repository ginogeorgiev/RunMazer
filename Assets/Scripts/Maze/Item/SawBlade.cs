using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// Saw blade that damages the player once the collider gets triggered
    /// </summary>
    public class SawBlade: MazeItem
    {
        [SerializeField] private float damageToPlayer = 20.0f;
        
        private void Start()
        {
            //to prevent that someone spawns these. they should only be spawned by the trap itself
            Count = 0;
        }
        
        protected override void EnterEffect()
        {
            CoreBars.HealthCore.CurrentValue -= damageToPlayer;
        }

        protected override void ExitEffect()
        {
        }

        protected override void StayEffect()
        {
        }
    }
}