using System;
using System.Collections;
using System.Collections.Generic;
using Maze.Item;
using Survival;
using UnityEngine;

namespace Maze.Item
{
    /// <summary>
    /// if player steps on the needle trap, the needles come out and player gets damaged.
    /// if player steps away from the trap, the needles go back to the default hiding position
    /// </summary>
    public class TrapNeedle : MazeItem
    {
        [SerializeField] private float delayInSec = 0.3f;
        private Animation anim;
        private bool isExposeCoroutineExecuting = false;
        private bool isHideCoroutineExecuting = false;
        private bool supposedToHide = false;
        private void Start()
        {
            anim = GetComponent<Animation>();
            anim.wrapMode = WrapMode.Once;
            
        }

        private void Update()
        {
            if(supposedToHide && !isExposeCoroutineExecuting)
            {
                StartCoroutine(HideNeedles());
                supposedToHide = false;
            }
        }

        protected override void EnterEffect()
        {
            StartCoroutine(ExposeNeedles());
        }

        protected override void ExitEffect()
        {
            supposedToHide = true;
        }

        protected override void StayEffect()
        {
        }
        //needles take delay in seconds time to get exposed
        IEnumerator ExposeNeedles()
        {
            if(isExposeCoroutineExecuting)
                yield break;
            isExposeCoroutineExecuting = true;
            yield return new WaitForSeconds(delayInSec);
            anim.Play("Anim_TrapNeedle_Show");
            isExposeCoroutineExecuting = false;
        }
        //needles take delay in seconds * 2 time to hide after exiting the trap
        IEnumerator HideNeedles()
        {
            if(isHideCoroutineExecuting)
                yield break;
            isHideCoroutineExecuting = true;
            yield return new WaitForSeconds(delayInSec*2);
            anim.Play("Anim_TrapNeedle_Hide");
            isHideCoroutineExecuting = false;
        }
    }
}
