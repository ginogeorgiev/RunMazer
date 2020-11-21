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
        

        void Start()
        {
            healthCore = new Core(healthBar, maxHealth, depletingRateHealth);
            hungerCore = new Core(hungerBar, maxHunger, depletingRateHunger);
            staminaCore = new Core(staminaBar, maxStamina, depletingRateStamina);
        }

        void Update()
        {
            deltaTime = Time.deltaTime;
            
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

            if ((Player.IsInBase || godMode) && !dieMfDie)
            {
                hungerCore.CurrentValue = Math.Min(maxHunger, hungerCore.CurrentValue + deltaTime * fillRateBase);
                staminaCore.CurrentValue = Math.Min(maxStamina, staminaCore.CurrentValue + deltaTime * fillRateBase);
                
                return;
            }

            if (dieMfDie)
            {
                healthCore.CurrentValue = 0;
                hungerCore.CurrentValue = 0;
                staminaCore.CurrentValue = 0;
                
                return;
            }
            
            if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.IsRunning)
            {
                staminaCore.CurrentValue = Math.Max(0, staminaCore.CurrentValue - deltaTime * depletingRateStamina);
            }
            else if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.IsWalking)
            {
                staminaCore.CurrentValue = Math.Min(maxStamina, staminaCore.CurrentValue + deltaTime * restWhenWalking);
            }
            else if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.IsIdle)
            {
                staminaCore.CurrentValue = Math.Min(maxStamina, staminaCore.CurrentValue + restWhenIdle * deltaTime);
            }

            if (hungerCore.CurrentValue > 0)
            {
                hungerCore.CurrentValue = Math.Max(0, hungerCore.CurrentValue - deltaTime * depletingRateHunger);
                
                return;
            }

            healthCore.CurrentValue = Math.Max(0, healthCore.CurrentValue - deltaTime * depletingRateHealth);
        }

        public static bool PlayerCanRun()
        {
            return !(staminaCore.CurrentValue - deltaTime * staminaCore.DepletingRate < 0);
        }
    }
}