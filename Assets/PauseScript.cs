using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    void Start()
    {
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }
    }
        //if scripts still work while timescale is 0, disable and enable them here 
        //if "Escape" is pressed, then set the time scale to 0
        private void PauseGame()
        {
            if (GameStateMachine.GetInstance().GetState() == GameStateMachine.State.Playing)
            {
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                GameStateMachine.GetInstance().SetState(GameStateMachine.State.Paused);
            }
        } 
        //vice versa
        private void ContinueGame()
        {
            if (GameStateMachine.GetInstance().GetState() == GameStateMachine.State.Paused)
            { 
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                GameStateMachine.GetInstance().SetState(GameStateMachine.State.Playing);
            }
        }
}
