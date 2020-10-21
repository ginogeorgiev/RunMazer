using UnityEngine;

public class MazeCell : MonoBehaviour
{
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
    private Vector2Int coordinates;
    private bool visited;
    private int path;

    //************** GETTERS & SETTERS ************//
    public void SetCoordinates(int x, int y)
    {
        coordinates = new Vector2Int(x,y);
    }

    public int GetX()
    {
        return coordinates.x;
    }

    public int GetY()
    {
        return coordinates.y;
    }

    public MazeCellEdge GetEdge (MazeDirection direction) {
        return edges[(int)direction];
    }

    public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
        edges[(int)direction] = edge;
    }

    public void SetVisited(bool visited)
    {
        this.visited = visited;
    }

    public bool IsVisited()
    {
        return visited;
    }

    public void SetPath(int path)
    {
        this.path = path;
    }

    public int getPath()
    {
        return path;
    }
}