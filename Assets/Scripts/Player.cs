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

    [SerializeField] private float runBuff = 0;
    [SerializeField] private float foodBuff = 0;
    

    private void Update()
    {
        // Game is not playing, nothing to do
        if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;

        inputVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, playerBody.velocity.y,
            Input.GetAxis("Vertical") * moveSpeed);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
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
                
                Invoke(nameof(PlayerIsHungry), foodBuff); //got to find better solution, multiple pick up doesn't accumulate
                break;
            
            case "Caffein":
                this.PlayerIsRunning();
                this.PlayerIsEating();

                foodBuff += 1f;
                runBuff += 10f;
                
                Invoke(nameof(PlayerIsHungry),  foodBuff);
                Invoke(nameof(PlayerCalmsDown), runBuff);
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

    private void PlayerCalmsDown()
    {
        moveSpeed = 20;
    }
}