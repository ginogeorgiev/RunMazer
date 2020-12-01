using UnityEngine;

namespace Maze
{
    public static class MazeDirections
    {

        //************ UTILITY METHODS REGARDING DIRECTIONS ************//
    
        public const int Count = 4;
    
        private static Vector2Int[] vectors = {
            new Vector2Int(1,0),
            new Vector2Int(0,-1),
            new Vector2Int(-1,0),
            new Vector2Int(0,1)
        };

        public static Vector2Int ToIntVector2 (MazeDirection direction) {
            return vectors[(int)direction];
        }
    
        private static MazeDirection[] opposites = {
            MazeDirection.South,
            MazeDirection.West,
            MazeDirection.North,
            MazeDirection.East
        };

        public static MazeDirection GetOpposite (this MazeDirection direction) {
            return opposites[(int)direction];
        }
    
        private static Quaternion[] rotations = {
            Quaternion.identity,
            Quaternion.Euler(0f, 90f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };
	
        public static Quaternion ToRotation (this MazeDirection direction) {
            return rotations[(int)direction];
        }

        private static MazeDirection[] nexts = {
            MazeDirection.West,
            MazeDirection.North,
            MazeDirection.East,
            MazeDirection.South
        };
        public static MazeDirection GetNext(this MazeDirection direction)
        {
            return nexts[(int) direction];
        }
        
        public static MazeDirection RandomValue {
            get {
                return (MazeDirection)Random.Range(0, Count);
            }
        }
    }
}
