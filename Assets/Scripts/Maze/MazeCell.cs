using UnityEngine;

public class MazeCell : MonoBehaviour
{
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
    public Vector2Int coordinates;
    private bool visited;
    private int path;

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

    public void SetPath(MazeDirection path)
    {
        this.path = (int) path;
    }

    public int getPath()
    {
        return path;
    }
}