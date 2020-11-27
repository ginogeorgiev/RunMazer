using System;
using System.Collections;
using UnityEngine;
using Object = System.Object;

namespace Maze.Item
{
    /// <summary>
    /// SpearTrap spawns a spear with a set interval (default is 1s)
    /// </summary>
    public class SpearTrap : MazeItem
    {
        [SerializeField] private float delayInSec = 1.0f;
        [SerializeField] private GameObject spear;
        [SerializeField] private float animSpeed;
        private bool isCoroutineExecuting = false;

        private void Start()
        {
            spear.gameObject.GetComponent<Spear>().AnimSpeed = animSpeed;
            Debug.Log(spear.gameObject.GetComponent<Spear>().AnimSpeed);
        }

        private void Update()
        {
            StartCoroutine(SpearSpawn());
        }

        protected override void EnterEffect()
        {
            
        }

        protected override void ExitEffect()
        {
        }

        protected override void StayEffect()
        {
        }
        IEnumerator SpearSpawn()
        {
            if(isCoroutineExecuting)
                yield break;
            isCoroutineExecuting = true;
            yield return new WaitForSeconds(delayInSec);
            
            Instantiate(spear,transform,false);
            isCoroutineExecuting = false;
        }
    }
}