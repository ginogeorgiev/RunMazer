using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public Vector2Int size;
    public Vector2Int sizeBase;
    public MazeCell cellPrefab;
    public MazeCellBase baseCellPrefab;
    public MazeCellPassage passagePrefab;
    public MazeCellWall wallPrefab;

    private MazeCell[,] cells;
    private MazeCell currentCell;

    private MazeCell getCell(int x, int y)
    {
        if (x >= size.x || x < 0 || y >= size.y || y < 0)
        {
            return null;
        }

        return cells[x, y];
    }

    public void Generate()
    {
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

        currentCell = getCell(size.x / 2, (size.y - sizeBase.y) / 2);
        Walk(2);
        currentCell = getCell(size.x / 2, (size.y - sizeBase.y) / 2 + sizeBase.y - 1);
        Walk(4);
        currentCell = getCell((size.x - sizeBase.x) / 2, size.y / 2);
        Walk(3);
        currentCell = getCell((size.x - sizeBase.x) / 2 + sizeBase.x - 1, size.y / 2);
        Walk(1);

        for (int i = 0; i < size.x * size.y / 2; i++)
        {
            currentCell = cells[Random.Range(0, size.x - 1), Random.Range(0, size.y - 1)];
            if (currentCell.IsVisited() && !(currentCell is MazeCellBase))
            {
                Walk(currentCell.getPath());
            }
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                currentCell = cells[x,y];
                if (!currentCell.IsVisited())
                {
                    currentCell.SetVisited(true);
                    Walk(5);
                }
            }
        }
    }

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

        cell.coordinates = new Vector2Int(x, y);
        cell.SetPath(0);
        cell.name = "Maze Cell" + x + "," + y;

        cell.transform.parent = transform;
        //TODO: create size modifier for cells (atm 5x5 big)
        cell.transform.localPosition =
            new Vector3((x - size.x * 0.5f) * 5f, -1f, (y - size.y * 0.5f) * 5f);
        cells[x, y] = cell;
    }

    private bool isInBase(int x, int z)
    {
        return x >= (size.x - sizeBase.x) / 2 && x < (size.x - sizeBase.x) / 2 + sizeBase.x &&
               z >= (size.y - sizeBase.y) / 2 && z < (size.y - sizeBase.y) / 2 + sizeBase.y;
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeCellWall wall = Instantiate(wallPrefab) as MazeCellWall;
        wall.Initialize(cell, otherCell, direction);
    }

    private void Walk(int path)
    {
        List<MazeDirection> unvisitedNeighbors = findUnvisitedNeighbors();
        while (unvisitedNeighbors.Count > 0)
        {
            MazeDirection chosenDirection = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
            MazeCell chosenCell = getCell(currentCell.coordinates.x + MazeDirections.ToIntVector2(chosenDirection).x,
                currentCell.coordinates.y + MazeDirections.ToIntVector2(chosenDirection).y);
            DestroyWall(currentCell.GetEdge(chosenDirection));
            DestroyWall(chosenCell.GetEdge(chosenDirection.GetOpposite()));
            currentCell = chosenCell;
            currentCell.SetVisited(true);
            currentCell.SetPath(path);
            unvisitedNeighbors = findUnvisitedNeighbors();
        }

        List<MazeDirection> otherPathNeighbors = find0therPathNeighbors();
        if (otherPathNeighbors.Count > 0)
        {
            MazeDirection chosenDirection = otherPathNeighbors[Random.Range(0, otherPathNeighbors.Count)];
            MazeCell chosenCell = getCell(currentCell.coordinates.x + MazeDirections.ToIntVector2(chosenDirection).x,
                currentCell.coordinates.y + MazeDirections.ToIntVector2(chosenDirection).y);
            DestroyWall(currentCell.GetEdge(chosenDirection));
            DestroyWall(chosenCell.GetEdge(chosenDirection.GetOpposite()));
        }
    }

    private List<MazeDirection> find0therPathNeighbors()
    {
        int x = currentCell.coordinates.x;
        int y = currentCell.coordinates.y;
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

    private List<MazeDirection> findUnvisitedNeighbors()
    {
        int x = currentCell.coordinates.x;
        int y = currentCell.coordinates.y;
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

    private void DestroyWall(MazeCellEdge wall)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall.gameObject);
        }
    }
}