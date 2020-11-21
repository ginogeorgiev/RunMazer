using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseAndEndScript : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endPanel;
    private TextMeshProUGUI endText;
    private Image bgImg;
    void Start()
    {
        pausePanel.SetActive(false);
        endPanel.SetActive(false);
        endText = endPanel.GetComponentInChildren<TextMeshProUGUI>();
        bgImg = endPanel.GetComponent<Image>();
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

        if (GameStateMachine.GetInstance().GetState() == GameStateMachine.State.GameOver)
        {
            YouLose();
        }
        else if (GameStateMachine.GetInstance().GetState() == GameStateMachine.State.GameWon)
        {
            YouWin();
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

        private void YouLose()
        {
            if (!endPanel.activeInHierarchy)
                
            {
                Time.timeScale = 0;
                endText.text = "You Lose!";
                bgImg.color = new Color(0, 0, 1, 0.5f);
                endPanel.SetActive(true);
            }
        }

        private void YouWin()
        {
            if (!endPanel.activeInHierarchy)
            {
                Time.timeScale = 0;
                endText.text = "You Win!";
                bgImg.color = new Color(1, 0, 0, 0.5f);
                endPanel.SetActive(true);
            }
        }
}
