using UnityEngine;

namespace Maze
{
    public class MazeCellEdge : MonoBehaviour
    {
        public MazeCell cell, otherCell;
	
        public MazeDirection direction;
    
        /// <summary>
        /// Initializes Edge between two cells and transforms it to sit in between
        /// </summary>
        public void Initialize (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
            this.cell = cell;
            this.otherCell = otherCell;
            this.direction = direction;
            cell.SetEdge(direction, this);
            transform.parent = cell.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = direction.ToRotation();
        }
    }
}
