using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;

    [SerializeField] private int moveSpeed = 20;
    // [SerializeField] private Game game;

    private Vector3 inputVector;

    [SerializeField] private float foodBuff = 0f;
    [SerializeField] private float runBuff = 0f;


    private void Update()
    {
        // Game is not playing, nothing to do
        if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;

        inputVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, playerBody.velocity.y,
            Input.GetAxis("Vertical") * moveSpeed);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));

        if (foodBuff > 0)
        {
            foodBuff -= Time.deltaTime;

            if (foodBuff < 0)
            {
                this.PlayerIsHungry();

                foodBuff = 0;
            }
        }
        
        if (runBuff > 0)
        {
            runBuff -= Time.deltaTime;

            if (runBuff < 0)
            {
                this.PlayerIsHungry();

                runBuff = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        playerBody.velocity = inputVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Base":
                this.PlayerIsInBase();
                break;
            
            case "Food":
                this.PlayerIsEating();

                foodBuff += 3f;
                
                break;
            
            case "Caffein":
                
                this.PlayerIsRunning();
                this.PlayerIsEating();

                foodBuff += 1f;
                runBuff += 10f;
                
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Base":
                this.PlayerIsHungry();
                break;
        }
    }

    private void PlayerIsHungry()
    {
        PlayerStateMachine.GetInstance().SetState(PlayerStateMachine.State.HasHunger);
    }

    private void PlayerIsEating()
    {
        PlayerStateMachine.GetInstance().SetState(PlayerStateMachine.State.IsEating);
    }

    private void PlayerIsInBase()
    {
        PlayerStateMachine.GetInstance().SetState(PlayerStateMachine.State.IsInBase);
    }

    private void PlayerIsRunning()
    {
        moveSpeed = 40;
    }

    private void PlayerIsWalking()
    {
        moveSpeed = 20;
    }
}