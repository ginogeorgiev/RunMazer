using System;
using Survival;
using UnityEngine;

namespace Maze.Item
{
    //if it hits the player, deal damage to him
    public class Axe : MazeItem
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