using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Survival;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Math = System.Math;


namespace Survival
{
    public class CoreBars : MonoBehaviour
    {
        private static Core healthCore;
        private static Core hungerCore;
        private static Core staminaCore;
        
        [SerializeField] private bool godMode = false;
        [SerializeField] private bool dieMfDie = false;

        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider hungerBar;
        [SerializeField] private Slider staminaBar;
        
        /// <summary>
        /// for better overview only
        /// fast access over inspector
        /// </summary>
        [SerializeField] private float maxHunger = 100f;
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxStamina = 100f;
        
        [SerializeField] private float currentHealth;
        [SerializeField] private float currentHunger;
        [SerializeField] private float currentStamina;

        [SerializeField] private float depletingRateHealth = 5f;
        [SerializeField] private float depletingRateHunger = 3f;
        [SerializeField] private float depletingRateStamina = 10f;

        [SerializeField] private float fillRateBase = 20f;

        [SerializeField] private float restWhenIdle = 4f;
        [SerializeField] private float restWhenWalking = 2f;

        private static float deltaTime;


        public static Core HealthCore
        {
            get => healthCore;
            set => healthCore = value;
        }

        public static Core HungerCore
        {
            get => hungerCore;
            set => hungerCore = value;
        }

        public static Core StaminaCore
        {
            get => staminaCore;
            set => staminaCore = value;
        }


        void Start()
        {
            healthCore = new Core(healthBar, maxHealth, depletingRateHealth);
            hungerCore = new Core(hungerBar, maxHunger, depletingRateHunger);
            staminaCore = new Core(staminaBar, maxStamina, depletingRateStamina);
        }

        void Update()
        {
            deltaTime = Time.deltaTime;
            
            //Game Over if health depleted
            if (healthCore.CurrentValue == 0)
            {
                GameStateMachine.GetInstance().SetState(GameStateMachine.State.GameOver);
                
                return;
            }

            healthBar.value = healthCore.CurrentValue;
            hungerBar.value = hungerCore.CurrentValue;
            staminaBar.value = staminaCore.CurrentValue;
            
            currentHealth = healthCore.CurrentValue;
            currentHunger = hungerCore.CurrentValue;
            currentStamina = staminaCore.CurrentValue;

            //increase hunger and stamina to maxVal when in base or god mode enabled
            if (Player.IsInBase || godMode)
            {
                hungerCore.CurrentValue = Math.Min(hungerCore.MaxValue, hungerCore.CurrentValue + deltaTime * fillRateBase);
                staminaCore.CurrentValue = Math.Min(staminaCore.MaxValue, staminaCore.CurrentValue + deltaTime * fillRateBase);
                
                return;
            }

            //instant death by depleting cores
            if (dieMfDie)
            {
                healthCore.CurrentValue = 0;
                hungerCore.CurrentValue = 0;
                staminaCore.CurrentValue = 0;
                
                return;
            }
            
            /* checks for player state
             * running: stamina is decreased over time
             * walking: stamina is increased slowly
             * idle: stamina is increased fast
             */
            if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.IsRunning)
            {
                staminaCore.CurrentValue = Math.Max(0, staminaCore.CurrentValue - deltaTime * staminaCore.DepletingRate);
            }
            else if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.IsWalking)
            {
                staminaCore.CurrentValue = Math.Min(staminaCore.MaxValue, staminaCore.CurrentValue + deltaTime * restWhenWalking);
            }
            else if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.IsIdle)
            {
                staminaCore.CurrentValue = Math.Min(staminaCore.MaxValue, staminaCore.CurrentValue + deltaTime * restWhenIdle);
            }

            //hunger is decreased over time
            if (hungerCore.CurrentValue > 0)
            {
                hungerCore.CurrentValue = Math.Max(0, hungerCore.CurrentValue - deltaTime * hungerCore.DepletingRate);
                
                return;
            }

            //health is decreased over time if hunger is depleted
            healthCore.CurrentValue = Math.Max(0, healthCore.CurrentValue - deltaTime * healthCore.DepletingRate);
        }

        
        /// <summary>
        /// prevents constantly switching between running and walking when stamina is depleted
        /// </summary>
        public static bool PlayerCanRun()
        {
            return !(staminaCore.CurrentValue - deltaTime * staminaCore.DepletingRate < 0);
        }
    }
}