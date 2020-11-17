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

    [SerializeField] private float restWhenIdle = 5f;
    [SerializeField] private float restWhenWalking = 2f;

    //for time sensitive states (until now: CaffeineRush)
    [SerializeField] private float timeInState = 0;
    // [SerializeField] private Game game;

    [SerializeField] private int moveSpeedWalking = 15;
    [SerializeField] private int moveSpeedRunning = 20;
    [SerializeField] private int moveSpeedCaffeine = 25;

    //time spent in caffeineRush state in s
    [SerializeField] private float stateChangeDelay = 5f;

    private bool isInOtherState = false;

    private Vector3 inputVector;
    

    private void Update()
    {
        // Game is not playing, nothing to do
        if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;
        
        inputVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, playerBody.velocity.y,
            Input.GetAxis("Vertical") * moveSpeed);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
        
        DetermineState();
        
        playerState = PlayerStateMachine.GetInstance().GetState();

        switch (playerState)
        {
            case PlayerStateMachine.State.CaffeineRush:
                timeInState += Time.deltaTime;

                moveSpeed = moveSpeedCaffeine;

                //Delays change into basic states (Idle, Walking, Running) by given time in s
                this.StateChangeDelay(stateChangeDelay);
                
                break;
            
            
            case PlayerStateMachine.State.IsIdle:
                
                CoreBars.PlayerRests(restWhenIdle);
                
                break;
            
            
            case PlayerStateMachine.State.IsWalking:

                moveSpeed = moveSpeedWalking;
                
                CoreBars.PlayerRests(restWhenWalking);
                
                break;
            
            
            case PlayerStateMachine.State.IsRunning:

                moveSpeed = moveSpeedRunning;
                
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

                isInOtherState = true;

                Destroy(other.gameObject);
                break;
            
            case  "Food":
                CoreBars.PlayerFoundItem("food");
                
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
        GameStateMachine.GetInstance().SetState(GameStateMachine.State.GameWon);
    }

    private void DetermineState()
    {
        if (isInOtherState) return;
        
        //player isn't moving
        if (inputVector == Vector3.zero)
        {
            PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsIdle);
            return;
        }

        //player is moving but shift isn't held down
        //or shift is held down but stamina is depleted
        if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && !CoreBars.PlayerCanRun()))
        {
            PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsWalking);
            
            return;
        }

        //player is moving, shift is held down and stamina isn't depleted
        PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsRunning);
        
    }

    private void StateChangeDelay(float delay)
    {
        if (timeInState > delay)
        {
            timeInState = 0;

            isInOtherState = false;

            this.DetermineState();
        }
    }
}