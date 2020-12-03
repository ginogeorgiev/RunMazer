﻿using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// sets health depleting rate to given percentage of initial value
    /// sets stamina depleting rate -*-
    /// makes you feel like Sophie Scholl
    /// UNUSED
    /// </summary>
    public class FfpMask : MazeItem
    {
        [SerializeField] private float healthDepletingEffect = 0.5f;
        [SerializeField] private float staminaDepletingEffect = 1.01f;

        protected override void EnterEffect()
        {
            CoreBars.HealthCore.DepletingRate *= healthDepletingEffect;
            CoreBars.StaminaCore.DepletingRate *= staminaDepletingEffect;

            Debug.Log("health depleting rate down");
            Debug.Log("stamina depleting rate up");

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