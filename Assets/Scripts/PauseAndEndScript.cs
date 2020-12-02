using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class PauseAndEndScript : MonoBehaviour
{
    [FormerlySerializedAs("endPanel")] [SerializeField] private GameObject pauseAndEndPanel;
    [SerializeField] private TextMeshProUGUI playAgainText;
    [SerializeField] private GameObject miniMap;
    private GameObject child;
    private TextMeshProUGUI endText;
    private Image bgImg;
    void Start()
    {
        pauseAndEndPanel.SetActive(false);
        endText = pauseAndEndPanel.GetComponentInChildren<TextMeshProUGUI>();
        bgImg = pauseAndEndPanel.GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseAndEndPanel.activeInHierarchy)
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
                pauseAndEndPanel.SetActive(true);
                endText.text = "Pause";
                playAgainText.text = "Restart";
                bgImg.color = new Color(1, 1, 1, 0.3f);
                //disables minimap so you cant maximise it after pausing
                miniMap.GetComponent<MiniMapUIScript>().enabled = false;
                GameStateMachine.GetInstance().SetState(GameStateMachine.State.Paused);
            }
        } 
        //vice versa
        private void ContinueGame()
        {
            if (GameStateMachine.GetInstance().GetState() == GameStateMachine.State.Paused)
            { 
                Time.timeScale = 1;
                pauseAndEndPanel.SetActive(false);
                miniMap.GetComponent<MiniMapUIScript>().enabled = true;
                GameStateMachine.GetInstance().SetState(GameStateMachine.State.Playing);
            }
        }

        private void YouLose()
        {
            if (!pauseAndEndPanel.activeInHierarchy)
                
            {
                Time.timeScale = 0;
                endText.text = "You Lose!";
                playAgainText.text = "Play Again";
                bgImg.color = new Color(0, 0, 1, 0.5f);
                miniMap.GetComponent<MiniMapUIScript>().enabled = false;
                pauseAndEndPanel.SetActive(true);
            }
        }

        private void YouWin()
        {
            if (!pauseAndEndPanel.activeInHierarchy)
            {
                Time.timeScale = 0;
                endText.text = "You Win!";
                playAgainText.text = "Play Again";
                bgImg.color = new Color(1, 0, 0, 0.5f);
                miniMap.GetComponent<MiniMapUIScript>().enabled = false;
                pauseAndEndPanel.SetActive(true);
            }
        }
}
