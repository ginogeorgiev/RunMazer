using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze.Item
{
    public class ItemGenerator : MonoBehaviour
    {
        private Maze maze;

        /// <summary>
        /// Generates all items according to their generation logic
        /// </summary>
        /// <param name="maze">to place items into</param>
        /// <param name="mazeItemPrefabList">Item Prefabs to be instantiated</param>
        public void Generate(global::Maze.Maze maze, List<MazeItem> mazeItemPrefabList)
        {
            this.maze = maze;
            foreach (MazeItem mazeItemPrefab in mazeItemPrefabList)
            {
                switch (mazeItemPrefab)
                {
                    case Fragment fragment:
                        GenerateFragments(fragment);
                        break;
                    default:
                        GenerateRandomInMaze(mazeItemPrefab);
                        break;
                }
            }
        }


        /// <summary>
        /// Places Fragments in the most outer corners of the outerMazePieces
        /// Since we have 4 outerMazePieces, 4 Fragments are placed
        /// </summary>
        /// <param name="fragment">Prefab to be instantiated</param>
        /// <exception cref="Exception">Thrown when cell is already occupied by another item</exception>
        private void GenerateFragments(Fragment fragment)
        {
            foreach (OuterMazePiece outerMazePiece in maze.OuterMazePieces)
            {
                MazeCell cornerCell = outerMazePiece.GetCell(0, 0);
                if (!cornerCell.HasItem)
                {
                    Fragment instantiatedFragment = Instantiate(fragment, cornerCell.transform, false);
                    cornerCell.HasItem = true;
                }
                else
                {
                    throw new Exception("Outer Corner Cell in outerMazePiece " + outerMazePiece.Orientation +
                                        " already has an item");
                }
            }
        }


        /// <summary>
        /// Places items at random places in inner and outer Maze
        /// Places <see cref="MazeItem.count"/> times
        /// </summary>
        /// <param name="mazeItemPrefab">Item prefab to be instantiated</param>
        private void GenerateRandomInMaze(MazeItem mazeItemPrefab)
        {
            for (int i = 0; i < mazeItemPrefab.Count; i++)
            {
                int randomCellNumber = Random.Range(0, maze.CellAmount - 1);
                MazeCell cell;
                if (randomCellNumber < maze.InnerMaze.GetCellAmount())
                {
                    cell = maze.InnerMaze.GetCell(randomCellNumber);
                }
                else
                {
                    randomCellNumber -= maze.InnerMaze.GetCellAmount();
                    int mazePieceNumber = randomCellNumber / maze.OuterMazePieces[0].GetCellAmount();
                    randomCellNumber -= mazePieceNumber * maze.OuterMazePieces[0].GetCellAmount();
                    cell = maze.OuterMazePieces[mazePieceNumber].GetCell(randomCellNumber);
                }

                try
                {
                    if (!cell.HasItem)
                    {
                        MazeItem item = Instantiate(mazeItemPrefab, cell.transform, false);
                        cell.HasItem = true;
                    }
                    else
                    {
                        i -= 1;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("RandomCellCalculation was not successful. Modify GenerateRandomInMaze");
                    throw;
                }
            }
        }
    }
}