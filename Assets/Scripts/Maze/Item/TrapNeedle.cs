using System;
using System.Collections;
using System.Collections.Generic;
using Maze.Item;
using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// if player steps on the needle trap, the needles come out and player gets damaged.
    /// if player steps away from the trap, the needles go back to the default hiding position
    /// </summary>
    public class TrapNeedle : MazeItem
    {
        [SerializeField] private float damageToPlayer = 30.0f;
        private Animation anim;
        private void Start()
        {
            anim = GetComponent<Animation>();
            anim.wrapMode = WrapMode.Once;
            
        }
        
        protected override void EnterEffect()
        {
            anim.Play("Anim_TrapNeedle_Show");
            CoreBars.HealthCore.CurrentValue -= damageToPlayer;
        }

        protected override void ExitEffect()
        {
            anim.Play("Anim_TrapNeedle_Hide");
        }

        protected override void StayEffect()
        {
            
        }
    }
}
