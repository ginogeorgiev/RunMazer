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
    [SerializeField] private int moveSpeedRunning = 2;
    
    private static bool isInBase;

    private Vector3 inputVector;


    public static bool IsInBase => isInBase;

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
            case PlayerStateMachine.State.IsWalking:

                moveSpeed = moveSpeedWalking;

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
                isInBase = true;
                
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
                isInBase = false;
                
                break;
        }
    }

    private void PlayerWonGame()
    {
        GameStateMachine.GetInstance().SetState(GameStateMachine.State.GameWon);
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
        //or shift is held down but stamina is depleted
        if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && !CoreBars.PlayerCanRun))
        {
            PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsWalking);
            
            return;
        }

        //player is moving, shift is held down and stamina isn't depleted
        PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsRunning);
        
    }
}