using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Survival;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CoreBars : MonoBehaviour
{
    [SerializeField] private bool godMode = false;
    [SerializeField] private bool dieMfDie = false;
    
    [SerializeField] private Slider hungerBar;
    [SerializeField] private Slider healthBar;
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

    [SerializeField] private float fillRateCoreNeedBase = 2f;

    [SerializeField] private float fillRateStamina = 1f;

    private static readonly CoreNeed hunger = new CoreNeed();
    private static readonly CoreNeed health = new CoreNeed();
    private static readonly CoreNeed stamina = new CoreNeed();
    
    

    private static bool isInBase;

    public static CoreNeed Hunger => hunger;

    public static CoreNeed Health => health;

    public static CoreNeed Stamina => stamina;

    public static bool IsInBase
    {
        get => isInBase;
        set => isInBase = value;
    }

    void Start()
    {
        hunger.Init(maxHunger);
        health.Init(maxHealth);
        stamina.Init(maxStamina);
    }

    void Update()
    {
        hungerBar.value = hunger.CurrentValue;
        healthBar.value = health.CurrentValue;
        staminaBar.value = stamina.CurrentValue;

        
        //when in base all cores get refilled
        if ((isInBase || godMode) && !dieMfDie)
        {
            hunger.BuffValue(Time.deltaTime * (hunger.MaxValue - hunger.CurrentValue + fillRateCoreNeedBase));
            health.BuffValue(Time.deltaTime * ((int) (hunger.CurrentValue/hunger.MaxValue) * (health.MaxValue - health.CurrentValue + fillRateCoreNeedBase)));
            stamina.BuffValue(Time.deltaTime * (stamina.MaxValue - stamina.CurrentValue + fillRateCoreNeedBase));
            
            return;
        }
        else if (dieMfDie)
        {
            hunger.CurrentValue = 0f;
            health.CurrentValue = 0f;
            stamina.CurrentValue = 0f;
            
            return;
        }
        
        //hunger always gets diminished when not in base
        hunger.DebuffValue(Time.deltaTime * depletingRateHunger);

        
        if (hunger.IsEmpty) health.DebuffValue(Time.deltaTime * depletingRateHealth);
        
        //stamina gets diminished when running
        //or when stamina is depleted and shift is still held down
        if (PlayerStateMachine.GetInstance().GetState()
            == PlayerStateMachine.State.IsRunning ||
            (PlayerStateMachine.GetInstance().GetState()
            == PlayerStateMachine.State.IsWalking &&
            Input.GetKey(KeyCode.LeftShift)))
        {
            stamina.DebuffValue(Time.deltaTime * depletingRateStamina);
        }
        else{ stamina.BuffValue(Time.deltaTime * fillRateStamina); }

        if (health.IsEmpty)
        {
            GameStateMachine.GetInstance().SetState(GameStateMachine.State.LostGame);
        }
    }

    public static void PlayerFoundFood(float foodBuff)
    {
        hunger.BuffValue(foodBuff);
    }
}