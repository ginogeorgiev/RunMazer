using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// increases stamina currentValue by given value
    /// </summary>
    public class Banana : MazeItem
    {
        [SerializeField] private float staminaValueEffect = 20f;

        protected override void EnterEffect()
        {
            CoreBars.StaminaCore.CurrentValue += staminaValueEffect;

            ReplaceItem();
        }

        protected override void ExitEffect()
        {
        }

        protected override void StayEffect()
        {
        }
    }
}