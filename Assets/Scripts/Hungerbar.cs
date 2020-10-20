using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Hungerbar : MonoBehaviour
{
    [SerializeField] private Slider hungerBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider sleepBar;
    [SerializeField] private float hunger;
    [SerializeField] private float health;
    [SerializeField] private float sleep;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxSleep = 100f;

    void Start()
    {
        hunger = maxHunger;
        health = maxHealth;
        sleep = maxSleep;
    }

    void Update()
    {


        if (hunger > 100) //TODO is there min(),max() equivalent in C#?
        {
            hunger = 100;
        }

        if (hunger < 0)
        {
            hunger = 0;
        }
        
        if (health > 100)
        {
            health = 100;
        }

        if (health < 0)
        {
            health = 0;
        }
        
        if (sleep > 100)
        {
            sleep = 100;
        }

        if (sleep < 0)
        {
            sleep = 0;
        }

        hungerBar.value = hunger;
        healthBar.value = health;
        sleepBar.value = sleep;

        var PlayerIsInBase = PlayerStateMachine.State.IsInBase;
        var PlayerIsEating = PlayerStateMachine.State.IsEating;
        var PlayerIsStarving = PlayerStateMachine.State.IsStarving;
        var PlayerIsTired = PlayerStateMachine.State.IsTired;



        if (PlayerStateMachine.GetInstance().CheckForState(PlayerIsInBase))
        {
            hunger += 20f * Time.deltaTime;
            Debug.Log("isInBase");

            health += 10f * Time.deltaTime;
            Debug.Log("isHealing");

            sleep += 20f * Time.deltaTime;
            Debug.Log("IsSleepy");
        }


        if (!(PlayerStateMachine.GetInstance().CheckForState(PlayerIsInBase) ||
            PlayerStateMachine.GetInstance().CheckForState(PlayerIsEating)))
        {
            hunger -= 2f * Time.deltaTime;
            Debug.Log("isHungry");

            if (hunger < 0)
            { 
                PlayerStateMachine.GetInstance().AddState(PlayerStateMachine.State.IsStarving);
            }

            sleep -= Time.deltaTime;

            if (sleep < 20)
            { 
                PlayerStateMachine.GetInstance().AddState(PlayerStateMachine.State.IsTired); 
            }
        }

        if (PlayerStateMachine.GetInstance().CheckForState(PlayerIsEating))
        {
            hunger += 2f * Time.deltaTime;
            Debug.Log("isEating");
        }


        if (PlayerStateMachine.GetInstance().CheckForState(PlayerIsStarving))
        {
            health -= 3f * Time.deltaTime;
            Debug.Log("isStarving");
        }
    }
}