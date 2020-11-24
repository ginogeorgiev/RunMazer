using System;
using System.Collections.Generic;
using Maze.Item;
using UnityEngine;

namespace Maze
{
    public class Maze : MonoBehaviour
    {
    
        // Prefabs for inner and outer maze pieces
        // all size Settings can be done in those prefabs
        // recommended: uneven x,y size for each, outer cells double the size of inner cells
        [SerializeField] private InnerMaze innerMazePrefab = null;
        [SerializeField] private OuterMazePiece outerMazePiecePrefab = null;

        private InnerMaze innerMaze;
        private List<OuterMazePiece> outerMazePieces;
        private int cellAmount;

        /// <summary>
        /// Generate whole maze with base, inner part and 4 outer parts
        /// </summary>
        public void Generate()
        {
            innerMaze = Instantiate(innerMazePrefab, transform, true) as InnerMaze;
            innerMaze.Generate();

            outerMazePieces = new List<OuterMazePiece>();
            foreach (MazeDirection direction in (MazeDirection[]) Enum.GetValues(typeof(MazeDirection)))
            {
                OuterMazePiece outerMazePiece = Instantiate(outerMazePiecePrefab, transform, true) as OuterMazePiece;
                outerMazePiece.SetInnerSize(innerMaze.GetUnitSize());
                outerMazePiece.name = "Outer Maze Piece " + direction;
                outerMazePiece.Generate();
                outerMazePiece.SetOrientation(direction);
                outerMazePieces.Add(outerMazePiece);
            }

            cellAmount = innerMaze.GetCellAmount() + outerMazePieces[0].GetCellAmount()*4;

        }
        
        //************** GETTERS & SETTERS ************//
        public InnerMaze InnerMaze => innerMaze;

        public List<OuterMazePiece> OuterMazePieces => outerMazePieces;

        public int CellAmount => cellAmount;
    }
}