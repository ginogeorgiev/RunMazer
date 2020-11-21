using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private String usedScene;
    //Singleton stuff from last semester (GTAT; Zier)
    private static GameManager instance;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Initialize();
            }

            return instance;
        }
        private set { instance = value; }
    }

    private static void Initialize()
    {
        var gameObjects = FindObjectsOfType<GameManager>();
        if (gameObjects.Length < 1)
        {
            CreateInstance();
        }
        else if(gameObjects.Length == 1)
        {
            instance = gameObjects[0];
        }
        else
        {
            Debug.LogWarning("more than one Instance! Assuming first");
            instance = gameObjects[0];
        }
    }

    private static void CreateInstance()
    {
        var host = new GameObject();
        Instance = host.AddComponent<GameManager>();
        // prevent accidental scene saving
        host.hideFlags = HideFlags.DontSaveInEditor;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        instance = this;
        //DontDestroyOnLoad(gameObject);
        
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        GameStateMachine.GetInstance().SetState(GameStateMachine.State.Playing);
        SceneManager.LoadScene(usedScene);
    }

    public void MainMenuButton()
    {
        GameStateMachine.GetInstance().SetState(GameStateMachine.State.MainMenu);
        SceneManager.LoadScene("MainMenu");
    }
}
