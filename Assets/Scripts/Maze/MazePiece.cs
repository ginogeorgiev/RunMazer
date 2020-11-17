using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MazePiece : MonoBehaviour
{
    [SerializeField] protected Vector2Int size = Vector2Int.zero;
    [SerializeField] protected float sizeCells = 0;
    [SerializeField] private float sizeWall = 0;
    [SerializeField] protected MazeCell cellPrefab = null;
    [SerializeField] private MazeCellWall wallPrefab = null;

    protected MazeCell[,] cells;
    protected MazeCell currentCell;
    
    public abstract void Generate();
    
    public abstract void CreateCell(int x, int y);

    /// <summary>
    /// Instantiates, Initializes and Transfroms wall to fit between <param name="cell"></param> and <param name="otherCell"></param>
    /// </summary>
    /// <param name="direction">of the wall</param>
    public void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeCellWall wall = Instantiate(wallPrefab) as MazeCellWall;
        wall.Initialize(cell, otherCell, direction);
        wall.transform.localScale = new Vector3(1, sizeWall, sizeCells + 1);
        wall.transform.GetChild(0).localPosition = new Vector3(sizeCells * 0.5f, 0, 0);
    }

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
            MazeCell chosenCell = getCell(currentCell.GetX() + MazeDirections.ToIntVector2(chosenDirection).x,
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
            MazeCell chosenCell = getCell(currentCell.GetX() + MazeDirections.ToIntVector2(chosenDirection).x,
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
        int path = currentCell.getPath();
        List<MazeDirection> otherPathNeighbors = new List<MazeDirection>();
        if (getCell(x - 1, y) != null && getCell(x - 1, y).getPath() != path && getCell(x - 1, y).getPath() != 0)
        {
            otherPathNeighbors.Add(MazeDirection.South);
        }

        if (getCell(x, y + 1) != null && getCell(x, y + 1).getPath() != path && getCell(x, y + 1).getPath() != 0)
        {
            otherPathNeighbors.Add(MazeDirection.West);
        }

        if (getCell(x + 1, y) != null && getCell(x + 1, y).getPath() != path && getCell(x + 1, y).getPath() != 0)
        {
            otherPathNeighbors.Add(MazeDirection.North);
        }

        if (getCell(x, y - 1) != null && getCell(x, y - 1).getPath() != path && getCell(x, y - 1).getPath() != 0)
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
        if (getCell(x - 1, y) != null && !getCell(x - 1, y).IsVisited())
        {
            unvisitedNeighbors.Add(MazeDirection.South);
        }

        if (getCell(x, y + 1) != null && !getCell(x, y + 1).IsVisited())
        {
            unvisitedNeighbors.Add(MazeDirection.West);
        }

        if (getCell(x + 1, y) != null && !getCell(x + 1, y).IsVisited())
        {
            unvisitedNeighbors.Add(MazeDirection.North);
        }

        if (getCell(x, y - 1) != null && !getCell(x, y - 1).IsVisited())
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
        if (wall != null)
        {
            GameObject.Destroy(wall.gameObject);
        }
    }
    
    protected MazeCell getCell(int x, int y)
    {
        if (x >= size.x || x < 0 || y >= size.y || y < 0)
        {
            return null;
        }

        return cells[x, y];
    }
}