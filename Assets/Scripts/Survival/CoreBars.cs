using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Survival;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.UI;


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

        [SerializeField] private static float foodBuff = 5f;

        private static float deltaTime;
        
        private static bool isInBase;
        

        public static bool IsInBase
        {
            get => isInBase;
            set => isInBase = value;
        }

        void Start()
        {
            healthCore = new Core(maxHealth, depletingRateHealth);
            hungerCore = new Core(maxHunger, depletingRateHunger);
            staminaCore = new Core(maxStamina, depletingRateStamina);

            healthCore.Bar = healthBar;
            hungerCore.Bar = hungerBar;
            staminaCore.Bar = staminaBar;
        }

        void Update()
        {
            deltaTime = Time.deltaTime;
            
            currentHealth = healthCore.UpdateCore();
            currentHunger = hungerCore.UpdateCore();
            currentStamina = staminaCore.UpdateCore();

            //when in base hunger an stamina get refilled
            if ((isInBase || godMode) && !dieMfDie)
            {
                hungerCore.FillInBase(deltaTime);
                staminaCore.FillInBase(deltaTime);
                
                return;
            }
            
            //all cores get emptied
            if (dieMfDie)
            {
                healthCore.EmptyCore();
                hungerCore.EmptyCore();
                staminaCore.EmptyCore();

                return;
            }

            //hunger always gets diminished when not in base
            if (!hungerCore.DepleteCore(deltaTime) && !healthCore.DepleteCore(deltaTime))
            {
               GameStateMachine.GetInstance().SetState(GameStateMachine.State.LostGame);
            }
        }

        public static void PlayerFoundItem(String item)
        {
            switch (item)
            {
                case "food":
                    hungerCore.IncreaseCore(foodBuff);
                    
                    break;
            }
        }

        public static bool PlayerCanRun()
        {
            if (!isInBase) return staminaCore.DepleteCore(deltaTime);

            return true;
        }

        public static void PlayerRests(float restRate)
        {
            staminaCore.IncreaseCore(deltaTime * restRate);
        }
    }
}