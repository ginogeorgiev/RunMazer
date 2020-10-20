using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class Hungerbar : MonoBehaviour
{
    [SerializeField] private Slider hungerBar;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float hunger;
    [SerializeField] private float health;
    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxHealth = 100f;

    void Start()
    {
        hunger = maxHunger;
        health = maxHealth;
    }

    void Update()
    {
        var playerState = PlayerStateMachine.GetInstance().GetState();
        
        
        if (hunger > 100) //is there min(),max() in C#?
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

        hungerBar.value = hunger;
        healthBar.value = health;

        switch (playerState)
        {
            case PlayerStateMachine.State.IsInBase:
                    
                hunger += 20f * Time.deltaTime;
                //Debug.Log("isInBase");

                health += 10f * Time.deltaTime;
                //Debug.Log("isHealing");
                break;
            
            
            case PlayerStateMachine.State.IsEating:

                hunger += 2f * Time.deltaTime;
                //Debug.Log("isEating");
                
                break;
                    
            
            case PlayerStateMachine.State.HasHunger:
                    
                hunger -= 2f * Time.deltaTime;
                //Debug.Log("hasHunger");

                if (hunger < 0)
                {
                    PlayerStateMachine.GetInstance().SetState(PlayerStateMachine.State.IsStarving);
                }
                break;
            
                
            case PlayerStateMachine.State.IsStarving:
                
                health -= 3f * Time.deltaTime;
                //Debug.Log("isStarving");
                break;
        }

    }
}