using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;

    [SerializeField] private int moveSpeed;

    [SerializeField] private PlayerStateMachine.State playerState;
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
            case PlayerStateMachine.State.IsIdle:
                if (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") != 0)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        playerState = PlayerStateMachine.State.IsRunning;
                        
                        break;
                    }
                    playerState = PlayerStateMachine.State.IsWalking;
                }
                break;
            
            case PlayerStateMachine.State.IsWalking:
                if (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") == 0)
                {
                    playerState = PlayerStateMachine.State.IsIdle;
                    
                    break;
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    playerState = PlayerStateMachine.State.IsRunning;
                }

                moveSpeed = 20;
                break;
            
            case PlayerStateMachine.State.IsRunning:
                if (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") == 0)
                {
                    playerState = PlayerStateMachine.State.IsIdle;
                    
                    break;
                }
                else if (!Input.GetKey(KeyCode.LeftShift))
                {
                    playerState = PlayerStateMachine.State.IsWalking;
                    
                }

                moveSpeed = 25;
                break;
            
            case PlayerStateMachine.State.IsTripping:
                moveSpeed = 30;
                break;
        }
        
        PlayerStateMachine.GetInstance().changeState(playerState);
    }

    private void FixedUpdate()
    {
        playerBody.velocity = inputVector;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
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
}