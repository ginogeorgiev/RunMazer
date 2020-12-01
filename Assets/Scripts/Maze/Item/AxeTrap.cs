using System;
using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// we need this script to spawn the whole axe trap and to handle the animation if needed.
    /// animSpeed speeds up or slows down the animation
    /// </summary>
    public class AxeTrap : MazeItem
    {
        [SerializeField] private float animSpeed = 0.5f;
        private Animation anim;
        private AnimationState mainState;
        
        protected override void EnterEffect()
        {
            anim = GetComponent<Animation>();
            mainState = anim["Anim_AxeTrap_Play"];
            anim.wrapMode = WrapMode.Loop;
            mainState.speed = animSpeed;
        }

        protected override void ExitEffect()
        {
        }

        protected override void StayEffect()
        {
        }
    }
}