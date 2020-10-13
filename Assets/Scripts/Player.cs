using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private int moveSpeed = 20;    
    // [SerializeField] private Game game;
    
    private Vector3 _inputVector;

    void Update()
    {
        // Game is not playing, nothing to do
        if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;
        
        _inputVector = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, playerBody.velocity.y, Input.GetAxis("Vertical") * moveSpeed);
        transform.LookAt(transform.position + new Vector3(_inputVector.x, 0, _inputVector.z));
    }

    private void FixedUpdate()
    {
        playerBody.velocity = _inputVector;
    }
}
