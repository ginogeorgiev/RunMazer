using System.Collections.Generic;
using Maze.Item;
using UnityEngine;

namespace Maze
{
    public class MazeManager : MonoBehaviour
    {
        [SerializeField] private Maze mazePrefab = null;
        
        // This is a list of all prefabs to be instantiated
        // If you add a new item, make sure to implement generation logic 
        // and modify serializable field count in the prefab
        [SerializeField] private List<MazeItem> mazeItemPrefabList = new List<MazeItem>();

        private Maze mazeInstance;

        private void Start()
        {
            BeginGame();
        }

        private void BeginGame()
        {
            {
                mazeInstance = Instantiate(mazePrefab) as global::Maze.Maze;
                mazeInstance.Generate();
                ItemGenerator itemGenerator = gameObject.AddComponent<ItemGenerator>();
                itemGenerator.Generate(mazeInstance, mazeItemPrefabList);
            }
        }
    }
}