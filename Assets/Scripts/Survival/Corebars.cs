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
    private Core stamina = Stamina.getInstance();

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

        if (isInBase)
        {
            hunger.BuffValue(Time.deltaTime);
            health.BuffValue(Time.deltaTime);
            stamina.BuffValue(Time.deltaTime);

            return;
        }
        
        hunger.DebuffValue(Time.deltaTime);

        if (hunger.IsEmpty) health.DebuffValue(Time.deltaTime);
        if (PlayerStateMachine.GetInstance().getState()
            == PlayerStateMachine.State.IsRunning)
        {
            stamina.DebuffValue(Time.deltaTime);
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
            
            case "Food":
                hunger.BuffValue(1f);
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