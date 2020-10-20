using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public Maze mazePrefab;

    private Maze mazeInstance;
    
    private void Start () {
        BeginGame();
    }

    private void BeginGame () {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
    }
    
}
