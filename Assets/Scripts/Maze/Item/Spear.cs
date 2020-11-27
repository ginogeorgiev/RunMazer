using System;
using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// spear has animation and rigidbody to detect collision with walls and player
    /// once a spear is spawned, animation starts playing and shoots the spear in one direction
    /// spear gets destroyed if it hits a player, a wall, or when the animation ends
    /// </summary>
    public class Spear : MonoBehaviour
    {
        [SerializeField] private float damageToPlayer = 15.0f;
        private float animSpeed = 1.0f;
        private Animation anim;
        private AnimationState mainState;

        private void Start()
        {
            anim = GetComponent<Animation>();
            anim.wrapMode = WrapMode.Once;
            mainState = anim["Anim_SpearTrap_Play"];
            mainState.speed = animSpeed;
        }

        private void Update()
        {
            if (!anim.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Player":
                    CoreBars.HealthCore.CurrentValue -= damageToPlayer;
                    Destroy(gameObject);
                    break;
                case "Wall":
                    Destroy(gameObject);
                    break;
            }
        }
        public float AnimSpeed
        {
            get => animSpeed;
            set => animSpeed = value;
        }
    }
}