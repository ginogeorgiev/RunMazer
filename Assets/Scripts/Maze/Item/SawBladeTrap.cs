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
            transform.localRotation = MazeDirections.ToRotation(MazeDirections.RandomValue);
            anim = GetComponent<Animation>();
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