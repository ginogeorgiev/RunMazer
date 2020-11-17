using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InnerMaze : MazePiece
{
    [SerializeField] private Vector2Int sizeBase = Vector2Int.zero;
    [SerializeField] private MazeCellBase baseCellPrefab = null;
    [SerializeField] private GameObject basePrefab = null;

    /// <summary>
    /// Generate maze including all its game parts
    /// </summary>
    public override void Generate()
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

        DestroyWall(getCell(0, Random.Range(0, size.y / 2 - 1)).GetEdge(MazeDirection.South));
        DestroyWall(getCell(0, Random.Range(size.y / 2 + 1, size.y - 1)).GetEdge(MazeDirection.South));

        DestroyWall(getCell(size.x - 1, Random.Range(0, size.y / 2 - 1)).GetEdge(MazeDirection.North));
        DestroyWall(getCell(size.x - 1, Random.Range(size.y / 2 + 1, size.y - 1)).GetEdge(MazeDirection.North));

        DestroyWall(getCell(Random.Range(0, size.x / 2 - 1), 0).GetEdge(MazeDirection.East));
        DestroyWall(getCell(Random.Range(size.x / 2 + 1, size.x - 1), 0).GetEdge(MazeDirection.East));

        DestroyWall(getCell(Random.Range(0, size.x / 2 - 1), size.y - 1).GetEdge(MazeDirection.West));
        DestroyWall(getCell(Random.Range(size.x / 2 + 1, size.x - 1), size.y - 1).GetEdge(MazeDirection.West));
    }


    /// <summary>
    /// Creates Base depending on given sizes
    /// </summary>
    private void InstantiateBase()
    {
        GameObject baseObject = Instantiate(basePrefab) as GameObject;
        baseObject.transform.localScale = new Vector3(sizeCells * sizeBase.x, 3, sizeCells * sizeBase.y);
    }

    /// <summary>
    /// Instantiates a maze cell and puts it in its spot in the scene
    /// If x and y is in Base create a Base Cell 
    /// </summary>
    /// <param name="x">coordinate of the cell</param>
    /// <param name="y">coordinate of the cell</param>
    public override void CreateCell(int x, int y)
    {
        MazeCell cell;
        if (isInBase(x, y))
        {
            cell = Instantiate(baseCellPrefab, transform, true) as MazeCellBase;
            cell.SetVisited(true);
        }
        else
        {
            cell = Instantiate(cellPrefab, transform, true) as MazeCell;
            cell.SetVisited(false);
        }

        cell.SetCoordinates(x, y);
        cell.SetPath(0);
        cell.name = "Maze Cell" + x + "," + y;

        cell.transform.GetChild(0).localScale = new Vector3(sizeCells * 0.1f, 1, sizeCells * 0.1f);
        cell.transform.localPosition =
            new Vector3(sizeCells * (x - size.x / 2), -1f, sizeCells * (y - size.y / 2));
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
    /// Variant of Hunt-and-Kill algorithm
    /// </summary>
    public override void HuntAndKill()
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

    public Vector2 GetSize()
    {
        return new Vector2(size.x * sizeCells, size.y * sizeCells);
    }
}