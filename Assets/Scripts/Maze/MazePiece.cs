using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public abstract class MazePiece : MonoBehaviour
    {
        [SerializeField] protected Vector2Int size = Vector2Int.zero;
        [SerializeField] protected float sizeCells = 0;
        [SerializeField] protected float wallScale = 0;
        [SerializeField] protected MazeCell cellPrefab = null;
        [SerializeField] protected MazeCellWall wallPrefab = null;
        [SerializeField] protected GameObject buttressPrefab = null;

        protected MazeCell[,] cells;
        protected MazeCell currentCell;

        public abstract void Generate();

        public abstract void CreateCell(int x, int y);

        /// <summary>
        /// Instantiates, Initializes and Transfroms wall to fit between <param name="cell"></param> and <param name="otherCell"></param>
        /// </summary>
        /// <param name="direction">of the wall</param>
        public abstract void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction);

        public abstract void HuntAndKill();

        /// <summary>
        /// Walk algorithm for the Hunt-and-Kill maze generation
        /// Walks a path from <see cref="currentCell"/>
        /// Destroys all walls encountered during walk
        /// </summary>
        /// <param name="path">to associate with current walk</param>
        protected void Walk(int path)
        {
            List<MazeDirection> unvisitedNeighbors = FindUnvisitedNeighbors();
            while (unvisitedNeighbors.Count > 0)
            {
                MazeDirection chosenDirection = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                MazeCell chosenCell = GetCell(currentCell.GetX() + MazeDirections.ToIntVector2(chosenDirection).x,
                    currentCell.GetY() + MazeDirections.ToIntVector2(chosenDirection).y);
                DestroyWall(currentCell.GetEdge(chosenDirection));
                DestroyWall(chosenCell.GetEdge(chosenDirection.GetOpposite()));
                currentCell = chosenCell;
                currentCell.SetVisited(true);
                currentCell.SetPath(path);
                unvisitedNeighbors = FindUnvisitedNeighbors();
            }

            // connects to other path if no univisited neighbors found
            // allows to find paths between base exits
            List<MazeDirection> otherPathNeighbors = Find0TherPathNeighbors();
            if (otherPathNeighbors.Count > 0)
            {
                MazeDirection chosenDirection = otherPathNeighbors[Random.Range(0, otherPathNeighbors.Count)];
                MazeCell chosenCell = GetCell(currentCell.GetX() + MazeDirections.ToIntVector2(chosenDirection).x,
                    currentCell.GetY() + MazeDirections.ToIntVector2(chosenDirection).y);
                DestroyWall(currentCell.GetEdge(chosenDirection));
                DestroyWall(chosenCell.GetEdge(chosenDirection.GetOpposite()));
            }
        }

        /// <summary>
        /// Checks all neighbors of <see cref="currentCell"/>
        /// </summary>
        /// <returns>Existent neighbors that have been visited from other paths</returns>
        private List<MazeDirection> Find0TherPathNeighbors()
        {
            int x = currentCell.GetX();
            int y = currentCell.GetY();
            int path = currentCell.GetPath();
            List<MazeDirection> otherPathNeighbors = new List<MazeDirection>();
            if (GetCell(x - 1, y) != null && GetCell(x - 1, y).GetPath() != path && GetCell(x - 1, y).GetPath() != 0)
            {
                otherPathNeighbors.Add(MazeDirection.South);
            }

            if (GetCell(x, y + 1) != null && GetCell(x, y + 1).GetPath() != path && GetCell(x, y + 1).GetPath() != 0)
            {
                otherPathNeighbors.Add(MazeDirection.West);
            }

            if (GetCell(x + 1, y) != null && GetCell(x + 1, y).GetPath() != path && GetCell(x + 1, y).GetPath() != 0)
            {
                otherPathNeighbors.Add(MazeDirection.North);
            }

            if (GetCell(x, y - 1) != null && GetCell(x, y - 1).GetPath() != path && GetCell(x, y - 1).GetPath() != 0)
            {
                otherPathNeighbors.Add(MazeDirection.East);
            }

            return otherPathNeighbors;
        }

        /// <summary>
        /// Checks all neighbors of <see cref="currentCell"/>
        /// </summary>
        /// <returns>unvisited & existent neighbors</returns>
        private List<MazeDirection> FindUnvisitedNeighbors()
        {
            int x = currentCell.GetX();
            int y = currentCell.GetY();
            List<MazeDirection> unvisitedNeighbors = new List<MazeDirection>();
            if (GetCell(x - 1, y) != null && !GetCell(x - 1, y).IsVisited())
            {
                unvisitedNeighbors.Add(MazeDirection.South);
            }

            if (GetCell(x, y + 1) != null && !GetCell(x, y + 1).IsVisited())
            {
                unvisitedNeighbors.Add(MazeDirection.West);
            }

            if (GetCell(x + 1, y) != null && !GetCell(x + 1, y).IsVisited())
            {
                unvisitedNeighbors.Add(MazeDirection.North);
            }

            if (GetCell(x, y - 1) != null && !GetCell(x, y - 1).IsVisited())
            {
                unvisitedNeighbors.Add(MazeDirection.East);
            }

            return unvisitedNeighbors;
        }

        /// <summary>
        /// Removes <param name="wall"></param> from scene
        /// </summary>
        protected void DestroyWall(MazeCellEdge wall)
        {
            if (wall != null && !Application.isEditor)
            {
                wall.cell.SetEdge(wall.direction, null);
                GameObject.Destroy(wall.gameObject);
            }
            else if (wall != null && Application.isEditor)
            {
                GameObject.DestroyImmediate(wall.gameObject);
            }
        }

        public MazeCell GetCell(int x, int y)
        {
            if (x >= size.x || x < 0 || y >= size.y || y < 0)
            {
                return null;
            }

            return cells[x, y];
        }


        protected void AddButtresses(MazeCell cell)
        {
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                GameObject buttress = Instantiate(buttressPrefab, cell.transform, false);
                buttress.transform.GetChild(0).localPosition =
                    new Vector3(sizeCells / 2 - 0.25f, 1.5f, sizeCells / 2 - 0.25f);
                buttress.transform.localScale = new Vector3(1, wallScale, 1);
                buttress.transform.localRotation = MazeDirections.ToRotation((MazeDirection) i);
                cell.buttresses.Add(buttress);
            }
        }

        protected void RemoveButtresses()
        {
            for (int x = 0; x < size.x - 1; x++)
            {
                for (int y = 0; y < size.y - 1; y++)
                {
                    if (IsIsolatedButtress(x, y))
                    {
                        Destroy(GetCell(x, y).buttresses[0]);
                        GetCell(x, y).buttresses[0] = null;
                        Destroy(GetCell(x, y + 1).buttresses[1]);
                        GetCell(x, y + 1).buttresses[1] = null;
                        Destroy(GetCell(x + 1, y + 1).buttresses[2]);
                        GetCell(x + 1, y + 1).buttresses[2] = null;
                        Destroy(GetCell(x + 1, y).buttresses[3]);
                        GetCell(x + 1, y).buttresses[3] = null;
                    }
                }
            }
        }


        private bool IsIsolatedButtress(int x, int y)
        {
            if (GetCell(x, y).GetEdge(MazeDirection.North) == null &&
                GetCell(x, y).GetEdge(MazeDirection.West) == null &&
                GetCell(x + 1, y + 1).GetEdge(MazeDirection.South) == null &&
                GetCell(x + 1, y + 1).GetEdge(MazeDirection.East) == null)
            {
                return true;
            }

            return false;
        }

        public abstract int GetCellAmount();

        /// <summary>
        /// acts as if each cell has an id
        /// calculates x and y coordinates of cell with id cellNumber
        /// </summary>
        /// <param name="cellNumber">cell id</param>
        /// <returns>MazeCell placed at calculated x and y coords</returns>
        public abstract MazeCell GetCell(int cellNumber);

        public Vector2Int Size => size;
    }
}