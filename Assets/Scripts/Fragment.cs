using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);

            if (gameObject.CompareTag("Item"))
            {
                ScoreManager.Instance.AddFragmentScore();
            }
        }
    }
}