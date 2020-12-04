using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// </summary>
    public class SawBladeTrap : MazeItem
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
            anim = GetComponentInChildren<Animation>();
            mainState = anim["Anim_SawTrap02_Play"];
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