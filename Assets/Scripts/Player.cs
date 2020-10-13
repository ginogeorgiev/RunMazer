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
    

    void Update()
    {
        // Game is not playing, nothing to do
        if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;
        
        inputVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, playerBody.velocity.y, Input.GetAxis("Vertical") * moveSpeed);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
    }

    private void FixedUpdate()
    {
        playerBody.velocity = inputVector;
    }
}
