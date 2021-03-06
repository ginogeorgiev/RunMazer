﻿using Survival;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Item
{
    /// <summary>
    /// Extends health by determined value
    /// </summary>
    public class Apple : MazeItem
    {
        [SerializeField] private float healthExtensionEffect = 20f;

        protected override void EnterEffect()
        {
            Slider health = CoreBars.HealthCore.Bar;

            RectTransform size = health.GetComponent<RectTransform>();

            //moves bar to the right by half the extension percentage
            size.position = new Vector3(
                size.position.x + size.sizeDelta.x * (healthExtensionEffect * 0.01f) / 2,
                size.position.y, size.position.z);

            size.localScale = new Vector3(
                size.localScale.x + healthExtensionEffect * 0.01f, size.localScale.y,
                size.localScale.z);

            CoreBars.HealthCore.MaxValue += healthExtensionEffect;
            //current value is increased by extension value
            CoreBars.HealthCore.CurrentValue += healthExtensionEffect;

            Destroy(gameObject);
        }

        protected override void ExitEffect()
        {
            
        }

        protected override void StayEffect()
        {
        }
    }
}