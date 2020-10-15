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
                this.PlayerIsWalking();

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
                Destroy(other.gameObject);
                this.PlayerIsEating();
                foodBuff += 3f;
                break;
            
            case "Caffein":
                Destroy(other.gameObject);
                this.PlayerIsRunning();
                this.PlayerIsEating();

                foodBuff += 1f;
                runBuff += 10f;
                break;
            
            case "Fragment":
                Destroy(other.gameObject);
                ScoreManager.Instance.AddFragmentScore();
                break;
            
            case "Exit":
                if (ScoreManager.Instance.GetFragmentScore() == 4)
                {
                    this.PlayerWonGame();
                    //destroy is temporary
                    Destroy(gameObject);
                    Debug.Log("you won");
                    
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Base":
                this.PlayerLeavesBase();
                break;
        }
    }
    
    private void PlayerLeavesBase()
    {
        this.PlayerIsHungry();
        this.PlayerIsAwake();
        
        PlayerStateMachine.GetInstance().RemoveState(PlayerStateMachine.State.IsInBase);
    }

    private void PlayerIsHungry()
    {
        PlayerStateMachine.GetInstance().RemoveState(PlayerStateMachine.State.IsEating);
        
        PlayerStateMachine.GetInstance().AddState(PlayerStateMachine.State.IsHungry);
    }

    private void PlayerIsEating()
    {
        PlayerStateMachine.GetInstance().RemoveState(PlayerStateMachine.State.IsHungry);
        
        PlayerStateMachine.GetInstance().AddState(PlayerStateMachine.State.IsEating);
    }

    private void PlayerIsInBase()
    {
        PlayerStateMachine.GetInstance().AddState(PlayerStateMachine.State.IsInBase);
    }

    private void PlayerIsAwake()
    {
        PlayerStateMachine.GetInstance().AddState(PlayerStateMachine.State.IsAwake);
    }

    private void PlayerIsSleepy()
    {
        PlayerStateMachine.GetInstance().AddState(PlayerStateMachine.State.IsSleepy);
    }

    private void PlayerWonGame()
    {
        GameStateMachine.GetInstance().SetState(GameStateMachine.State.FinishedGame);
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