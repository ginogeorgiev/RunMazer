using System;
using System.Collections;
using System.Collections.Generic;
using Survival;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Corebars : MonoBehaviour
{
    [SerializeField] private Slider hungerBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider staminaBar;
    

    private Core hunger = Hunger.GetInstance();
    private Core health = Health.GetInstance();
    private Core stamina = Stamina.GetInstance();

    private bool isInBase;

    void Start()
    {
        hunger.Init(100f);
        health.Init(100f);
        stamina.Init(100f);
        
    }

    void Update()
    {
        hungerBar.value = hunger.CurrentValue;
        healthBar.value = health.CurrentValue;
        staminaBar.value = stamina.CurrentValue;

        
        //when in base all cores get refilled
        if (isInBase)
        {
            hunger.BuffValue(Time.deltaTime * (hunger.MaxValue - hunger.CurrentValue));
            health.BuffValue(Time.deltaTime * (health.MaxValue - health.CurrentValue));
            stamina.BuffValue(Time.deltaTime * (stamina.MaxValue - stamina.CurrentValue));

            return;
        }
        
        //hunger always gets diminished when not in base
        hunger.DebuffValue(Time.deltaTime * 3f);

        
        if (hunger.IsEmpty) health.DebuffValue(Time.deltaTime * 5f);
        
        //stamina gets diminished when running
        //or when stamina is depleted and shift is still held down
        if (PlayerStateMachine.GetInstance().getState()
            == PlayerStateMachine.State.IsRunning ||
            (PlayerStateMachine.GetInstance().getState()
            == PlayerStateMachine.State.IsWalking &&
            Input.GetKey(KeyCode.LeftShift)))
        {
            stamina.DebuffValue(Time.deltaTime * 10f);
        }
        else{ stamina.BuffValue(Time.deltaTime); }

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case  "Base":
                this.isInBase = true;
                break;
            
            //hunger is increased by given amount
            case "Food":
                hunger.BuffValue(10f);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Base":
                isInBase = false;
                break;
        }
    }
}