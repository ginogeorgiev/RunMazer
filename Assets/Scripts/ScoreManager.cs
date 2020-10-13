using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    //Singleton stuff from last semester (GTAT; Zier)
    private static ScoreManager instance;
    private int _fragmentScore;
    [SerializeField] private TextMeshProUGUI fragmentText;
    
    public static ScoreManager Instance
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
        var gameObjects = FindObjectsOfType<ScoreManager>();
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
        Instance = host.AddComponent<ScoreManager>();
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
        _fragmentScore = 0;
        fragmentText.text = "Fragment: " + _fragmentScore + "/4";
    }
    
    //adds fragment
    public void AddFragmentScore()
    {
        if (_fragmentScore >= 4)
        {
            return;
        }
        _fragmentScore++;
        fragmentText.text = "Fragment: " + _fragmentScore + "/4";
    }
}
