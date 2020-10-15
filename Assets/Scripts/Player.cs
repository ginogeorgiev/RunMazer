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
                Invoke(nameof(PlayerIsHungry), 3f); //got to find better solution, multiple pick up doesn't accumulate
                break;
            
            case "Caffein":
                this.PlayerIsTrippin();
                this.PlayerIsEating();
                
                Invoke(nameof(PlayerIsHungry), 1f);
                Invoke(nameof(PlayerCalmsDown), 9f);
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

    private void PlayerIsTrippin()
    {
        moveSpeed = 40;
    }

    private void PlayerCalmsDown()
    {
        moveSpeed = 20;
    }
}