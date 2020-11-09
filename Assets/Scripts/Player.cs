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

    [SerializeField] private float foodBuff;


    //for time sensitive states (until now: IsIdle and CaffeineRush)
    [SerializeField] private float timeInState = 0;
    // [SerializeField] private Game game;

    [SerializeField] private int moveSpeedWalking = 15;
    [SerializeField] private int moveSpeedRunning = 20;
    [SerializeField] private int moveSpeedCaffeine = 25;

    [SerializeField] private float stateChangeDelay = 5f;

    private Vector3 inputVector;
    

    private void Update()
    {
        // Game is not playing, nothing to do
        if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;
        
        inputVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, playerBody.velocity.y,
            Input.GetAxis("Vertical") * moveSpeed);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
        
        
        playerState = PlayerStateMachine.GetInstance().GetState();

        switch (playerState)
        {
            case PlayerStateMachine.State.CaffeineRush:
                timeInState += Time.deltaTime;

                moveSpeed = moveSpeedCaffeine;
                
                //Delays change into basic states (Idle, Walking, Running) by given time in s
                this.StateChangeDelay(stateChangeDelay);
                break;

            default:
                this.DetermineState();
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
            case  "Base":
                CoreBars.IsInBase = true;
                
                break;

            case "Caffein":
                PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.CaffeineRush);

                Destroy(other.gameObject);
                break;
            
            case  "Food":
                CoreBars.PlayerFoundFood(foodBuff);
                
                Destroy(other.gameObject);
                break;
                
            case "Fragment":
                Destroy(other.gameObject);
                ScoreManager.Instance.AddFragmentScore();
                break;
            
            case "Exit":
                if (ScoreManager.Instance.GetFragmentScore() == ScoreManager.Instance.GetMaxFragments())
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
                CoreBars.IsInBase = false;
                
                break;
        }
    }

    private void PlayerWonGame()
    {
        GameStateMachine.GetInstance().SetState(GameStateMachine.State.FinishedGame);
    }

    private void DetermineState()
    {
        //player isn't moving
        if (inputVector == Vector3.zero)
        {
            PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsIdle);
            return;
        }

        //player is moving but shift isn't held down
        //or stamina is depleted
        if (!Input.GetKey(KeyCode.LeftShift) || CoreBars.Stamina.IsEmpty)
        {
            PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsWalking);

            moveSpeed = moveSpeedWalking;
            return;
        }

        //player is moving, shift is held down and stamina isn't depleted 
        if (!CoreBars.Stamina.IsEmpty)
        {
            PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsRunning);

            moveSpeed = moveSpeedRunning;
        }
    }

    private void StateChangeDelay(float delay)
    {
        if (timeInState > delay)
        {
            timeInState = 0;

            this.DetermineState();
        }
    }
}