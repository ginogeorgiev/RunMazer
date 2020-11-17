using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OuterMazePiece : MazePiece
{
    private MazeDirection orientation = MazeDirection.South;
    private Vector2 innerSize;
    private Vector2Int boundary;
    private bool generated = false;

    /// <summary>
    /// Generates a SOUTH outer maze piece including all its game parts
    /// </summary>
    public override void Generate()
    {
        if (generated)
        {
            return;
        }
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
                if (x < boundary.x || y < boundary.y)
                {
                    CreateWall(getCell(x, y), getCell(x + 1, y), MazeDirection.North);
                    CreateWall(getCell(x, y), getCell(x - 1, y), MazeDirection.South);
                    CreateWall(getCell(x, y), getCell(x, y - 1), MazeDirection.East);
                    CreateWall(getCell(x, y), getCell(x + 1, y + 1), MazeDirection.West);
                }
            }
        }

        // adds outer walls for path between inner and outer maze
        CreateWall(getCell(boundary.x, size.y - 1), null, MazeDirection.West);
        CreateWall(getCell(size.x - 1, boundary.y), null, MazeDirection.North);

        HuntAndKill();

        // removes unneccesary cells lying under the innerMaze cells
        for (int x = boundary.x + 1; x < size.x; x++)
        {
            for (int y = boundary.y + 1; y < size.y; y++)
            {
                GameObject.Destroy(getCell(x, y).gameObject);
            }
        }
        
        generated = true;
    }

    /// <summary>
    /// Instantiates a maze cell and puts it in its spot in the scene
    /// If x and y is under inner maze, set visited to true
    /// </summary>
    /// <param name="x">coordinate of the cell</param>
    /// <param name="y">coordinate of the cell</param>
    public override void CreateCell(int x, int y)
    {
        MazeCell cell = Instantiate(cellPrefab, transform, true) as MazeCell;
        if (x < boundary.x || y < boundary.y)
        {
            cell.SetVisited(false);
        }
        else
        {
            cell.SetVisited(true);
        }

        cell.SetCoordinates(x, y);
        cell.name = "Outer Cell " + x + "," + y;
        cell.transform.GetChild(0).localScale = new Vector3(sizeCells * 0.1f, 1, sizeCells * 0.1f);
        cell.transform.localPosition =
            new Vector3(sizeCells * (x - size.x / 2), -1f, sizeCells * (y - size.y / 2));
        cells[x, y] = cell;
    }
    
    /// <summary>
    /// Variant of Hunt-and-Kill algorithm
    /// </summary>
    public override void HuntAndKill()
    {
        // Walk from 2 random cells in middle path
        currentCell = getCell(Random.Range(boundary.x, size.x - 1), boundary.y);
        Walk(1);
        currentCell = getCell(boundary.x, Random.Range(boundary.y, size.y - 1));
        Walk(2);

        // Walk from random Cells that have been visited
        for (int i = 0; i < size.x * size.y; i++)
        {
            currentCell = cells[Random.Range(0, size.x - 1), Random.Range(0, size.y - 1)];
            if (currentCell.IsVisited() && currentCell.getPath() != 0)
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

    // rotates the whole MazePiece and translates it accordingly
    public void SetOrientation(MazeDirection orientation)
    {
        this.orientation = orientation;
        if (generated)
        {
                    
            Vector2Int dir = MazeDirections.ToIntVector2(orientation) +
                             MazeDirections.ToIntVector2(orientation.GetNext());
            float diffX = innerSize.x / 2 - (size.x - boundary.x - 1) * sizeCells;
            float diffY = innerSize.x / 2 - (size.y - boundary.y - 1) * sizeCells;

            transform.localPosition =
                new Vector3(dir.x * (size.x * sizeCells / 2f + diffX), 0, dir.y * (size.y * sizeCells / 2f + diffY));
            transform.localRotation = MazeDirections.ToRotation(MazeDirections.GetOpposite(orientation));
        }
    }

    
    /// <summary>
    /// sets size of the innerMazePiece and calculates x and y boundaries where outer piece cells need to be placed
    /// </summary>
    /// <param name="innerSize"></param>
    public void SetInnerSize(Vector2 innerSize)
    {
        this.innerSize = innerSize;
        this.boundary = new Vector2Int((int) (size.x - (int) (innerSize.x / 2) / sizeCells),
            (int) (size.y - (int) (innerSize.y / 2) / sizeCells));
    }
}