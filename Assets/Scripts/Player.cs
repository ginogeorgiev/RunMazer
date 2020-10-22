using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Survival;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;

    [SerializeField] private int moveSpeed;

    [SerializeField] private PlayerStateMachine.State playerState;

    //for time sensitive states (until now: IsIdle and CaffeineRush)
    [SerializeField] float timeInState = 0;
    // [SerializeField] private Game game;

    private Vector3 inputVector;
    

    private void Update()
    {
        // Game is not playing, nothing to do
        if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;
        
        inputVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, playerBody.velocity.y,
            Input.GetAxis("Vertical") * moveSpeed);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
        
        
        playerState = PlayerStateMachine.GetInstance().getState();

        switch (playerState)
        {
            case PlayerStateMachine.State.CaffeineRush:
                timeInState += Time.deltaTime;

                moveSpeed = 25;
                
                //Delays change into basic states (Idle, Walking, Running) by given time in s
                this.stateChangeDelay(5f);
                break;
            
            case PlayerStateMachine.State.IsIdle:
                
                //changes into basic state if player is moving
                if (inputVector != Vector3.zero)
                {
                    timeInState = 0;
                    
                    //determines which basic state player is in
                    this.determineState();
                    
                    break;
                }
                
                timeInState += Time.deltaTime;
                
                this.stateChangeDelay(3f);
                break;
            
            case PlayerStateMachine.State.IsJiggiling:
                
                
                if (inputVector != Vector3.zero)
                {
                    
                    //determines which basic state player is in
                    this.determineState();
                    
                    break;
                }
                
                //TODO fancy jiggiling
                break;

            default:
                this.determineState();
                break;
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
            case "Caffein":
                PlayerStateMachine.GetInstance().changeState(PlayerStateMachine.State.CaffeineRush);

                Destroy(other.gameObject);
                break;
            
            case  "Food":
                Destroy(other.gameObject);
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
        
    }

    private void PlayerWonGame()
    {
        GameStateMachine.GetInstance().SetState(GameStateMachine.State.FinishedGame);
    }

    private void determineState()
    {
        //player isn't moving
        if (inputVector == Vector3.zero)
        {
            PlayerStateMachine.GetInstance().changeState(PlayerStateMachine.State.IsIdle);
            return;
        }

        //player is moving but shift isn't held down
        //or stamina is depleted
        if (!Input.GetKey(KeyCode.LeftShift) || Stamina.GetInstance().IsEmpty)
        {
            PlayerStateMachine.GetInstance().changeState(PlayerStateMachine.State.IsWalking);

            moveSpeed = 15;
            return;
        }

        //player is moving, shift is held down and stamina isn't depleted 
        if (!Stamina.GetInstance().IsEmpty)
        {
            PlayerStateMachine.GetInstance().changeState(PlayerStateMachine.State.IsRunning);

            moveSpeed = 20;
        }
    }

    private void stateChangeDelay(float delay)
    {
        if (timeInState > delay)
        {
            timeInState = 0;

            if (playerState == PlayerStateMachine.State.IsIdle)
            {
                PlayerStateMachine.GetInstance().changeState(PlayerStateMachine.State.IsJiggiling);
                return;
            }
            
            this.determineState();
        }
    }
}