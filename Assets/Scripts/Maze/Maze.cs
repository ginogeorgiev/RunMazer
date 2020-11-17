using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    
    // Prefabs for inner and outer maze pieces
    // all size Settings can be done in those prefabs
    // recommended: uneven x,y size for each, outer cells double the size of inner cells
    [SerializeField] private InnerMaze innerMazePrefab = null;
    [SerializeField] private OuterMazePiece outerMazePiecePrefab = null;

    /// <summary>
    /// Generate whole maze with base, inner part and 4 outer parts
    /// </summary>
    public void Generate()
    {
        InnerMaze innerMaze = Instantiate(innerMazePrefab, transform, true) as InnerMaze;
        innerMaze.Generate();

        foreach (MazeDirection direction in (MazeDirection[]) Enum.GetValues(typeof(MazeDirection)))
        {
            OuterMazePiece outerMazePiece = Instantiate(outerMazePiecePrefab, transform, true) as OuterMazePiece;
            outerMazePiece.SetInnerSize(innerMaze.GetSize());
            outerMazePiece.name = "Outer Maze Piece " + direction;
            outerMazePiece.Generate();
            outerMazePiece.SetOrientation(direction);
        }
        
    }
}