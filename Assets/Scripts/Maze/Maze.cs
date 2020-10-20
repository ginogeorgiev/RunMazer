using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    [SerializeField] private Vector2Int sizeBase;
    [SerializeField] private float sizeCell;
    [SerializeField] private float sizeWall;
    [SerializeField] private MazeCell cellPrefab;
    [SerializeField] private MazeCellBase baseCellPrefab;
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private MazeCellWall wallPrefab;

    private MazeCell[,] cells;
    private MazeCell currentCell;

    /// <summary>
    /// Generate maze including all its game parts
    /// Generation-algorithm used is a variant of the hunt-and-kill algorithm
    /// </summary>
    public void Generate()
    {
        InstantiateBase();


        // Initialize all Cells with Walls on every side
        cells = new MazeCell[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                CreateCell(x, y);
            }
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (!(getCell(x, y) is MazeCellBase))
                {
                    CreateWall(getCell(x, y), getCell(x + 1, y), MazeDirection.North);
                    CreateWall(getCell(x, y), getCell(x - 1, y), MazeDirection.South);
                    CreateWall(getCell(x, y), getCell(x, y - 1), MazeDirection.East);
                    CreateWall(getCell(x, y), getCell(x + 1, y + 1), MazeDirection.West);
                }
            }
        }

        HuntAndKill();
    }

    /// <summary>
    /// Creates Base depending on given sizes
    /// </summary>
    private void InstantiateBase()
    {
        GameObject baseObject = Instantiate(basePrefab) as GameObject;
        baseObject.transform.localScale = new Vector3(sizeCell * sizeBase.x, 3, sizeCell * sizeBase.y);
    }

    /// <summary>
    /// Instantiates a maze cell and puts it in its spot in the scene
    /// If x and y is in Base create a Base Cell 
    /// </summary>
    /// <param name="x">coordinate of the cell</param>
    /// <param name="y">coordinate of the cell</param>
    private void CreateCell(int x, int y)
    {
        MazeCell cell;
        if (isInBase(x, y))
        {
            cell = Instantiate(baseCellPrefab) as MazeCellBase;
            cell.SetVisited(true);
        }
        else
        {
            cell = Instantiate(cellPrefab) as MazeCell;
            cell.SetVisited(false);
        }

        cell.SetCoordinates(x,y);
        cell.SetPath(0);
        cell.name = "Maze Cell" + x + "," + y;

        cell.transform.parent = transform;
        cell.transform.GetChild(0).localScale = new Vector3(sizeCell * 0.1f, 1, sizeCell * 0.1f);
        cell.transform.localPosition =
            new Vector3(sizeCell * (x - size.x / 2), -1f, sizeCell * (y - size.y / 2));
        cells[x, y] = cell;
    }

    /// <summary>
    /// Checks whether <param name="x"></param> and <param name="y"></param> coordinate is inside base of maze
    /// </summary>
    private bool isInBase(int x, int y)
    {
        return x >= (size.x - sizeBase.x) / 2 && x < (size.x - sizeBase.x) / 2 + sizeBase.x &&
               y >= (size.y - sizeBase.y) / 2 && y < (size.y - sizeBase.y) / 2 + sizeBase.y;
    }

    /// <summary>
    /// Instantiates, Initializes and Transfroms wall to fit between <param name="cell"></param> and <param name="otherCell"></param>
    /// </summary>
    /// <param name="direction">of the wall</param>
    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeCellWall wall = Instantiate(wallPrefab) as MazeCellWall;
        wall.Initialize(cell, otherCell, direction);
        wall.transform.localScale = new Vector3(1, sizeWall, sizeCell + 1);
        wall.transform.GetChild(0).localPosition = new Vector3(sizeCell * 0.5f, 0, 0);
    }
    
    /// <summary>
    /// Variant of Hunt-and-Kill algorithm
    /// </summary>
    private void HuntAndKill()
    {
        // Walk from all 4 Base-Exits first
        // Sequence could also be randomised
        currentCell = getCell(size.x / 2, (size.y - sizeBase.y) / 2);
        Walk(2);
        currentCell = getCell(size.x / 2, (size.y - sizeBase.y) / 2 + sizeBase.y - 1);
        Walk(4);
        currentCell = getCell((size.x - sizeBase.x) / 2, size.y / 2);
        Walk(3);
        currentCell = getCell((size.x - sizeBase.x) / 2 + sizeBase.x - 1, size.y / 2);
        Walk(1);

        // Walk from random Cells that have been visited
        for (int i = 0; i < size.x * size.y / 2; i++)
        {
            currentCell = cells[Random.Range(0, size.x - 1), Random.Range(0, size.y - 1)];
            if (currentCell.IsVisited() && !(currentCell is MazeCellBase))
            {
                Walk(currentCell.getPath());
            }
        }

        // Walk from and connect remaining unconnected cells
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                currentCell = cells[x, y];
                if (!currentCell.IsVisited())
                {
                    currentCell.SetVisited(true);
                    Walk(5);
                }
            }
        }
    }

    /// <summary>
    /// Walk algorithm for the Hunt-and-Kill maze generation
    /// Walks a path from <see cref="currentCell"/>
    /// Destroys all walls encountered during walk
    /// </summary>
    /// <param name="path">to associate with current walk</param>
    private void Walk(int path)
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
    private void DestroyWall(MazeCellEdge wall)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall.gameObject);
        }
    }

    private MazeCell getCell(int x, int y)
    {
        if (x >= size.x || x < 0 || y >= size.y || y < 0)
        {
            return null;
        }

        return cells[x, y];
    }
}