﻿using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// sets stamina depleting rate to given percentage of initial value
    /// UNUSED
    /// </summary>
    public class Coffee : MazeItem
    {
        [SerializeField] private float staminaDepletingEffect = 0.8f;

        protected override void EnterEffect()
        {
            CoreBars.StaminaCore.DepletingRate *= staminaDepletingEffect;

            Debug.Log("Stamina depleting rate down");

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