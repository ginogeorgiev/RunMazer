using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hungerbar : MonoBehaviour
{
    [SerializeField] private Slider hungerBar;
    [SerializeField] private float hunger;
    [SerializeField] private float maxHunger = 100f;

    void Start()
    {
        hunger = maxHunger;
    }

    void Update()
    {
        if (hunger > 100)
        {
            hunger = 100;
        }

        if (hunger < 0)
        {
            hunger = 0;
        }
        
        hungerBar.value = hunger;

        if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.HasHunger)
        {
            hunger -= 2f * Time.deltaTime;
            Debug.Log("hasHunger");
        }
        if (PlayerStateMachine.GetInstance().GetState() == PlayerStateMachine.State.IsEating)
        {
            hunger += 20f * Time.deltaTime;
            Debug.Log("isEating");
        }
    }
}