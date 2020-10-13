using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        SetPosition();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (player != null)
        {
            SetPosition();
            
        }
    }

    void SetPosition()
    {
        var newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
