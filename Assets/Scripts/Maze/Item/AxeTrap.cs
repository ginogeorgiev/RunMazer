using System;
using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// we need this script to spawn the whole saw blade trap and to handle the animation if needed.
    /// animSpeed speeds up or slows down the animation
    /// </summary>
    public class AxeTrap : MazeItem
    {
        [SerializeField] private float animSpeed = 1.0f;
        private Animation anim;
        private AnimationState mainState;

        private void Start()
        {
            if (transform.parent.parent.GetComponent<OuterMazePiece>())
            {
                transform.localScale *= 2;
            }
            
            transform.localRotation = MazeDirections.ToRotation(MazeDirections.RandomValue);
            anim = GetComponent<Animation>();
            mainState = anim["Anim_AxeTrap_Play"];
            anim.wrapMode = WrapMode.Loop;
            mainState.speed = animSpeed;
            
            
        }
 
        protected override void EnterEffect()
        {
        }

        protected override void ExitEffect()
        {
        }

        protected override void StayEffect()
        {
        }
    }
}