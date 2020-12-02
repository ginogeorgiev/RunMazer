using Survival;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Item
{
    /// <summary>
    /// Extends stamina by determined value
    /// </summary>
    public class Shoes : MazeItem
    {
        [SerializeField] private float staminaExtensionEffect = 100f;

        protected override void EnterEffect()
        {
            Slider stamina = CoreBars.StaminaCore.Bar;

            RectTransform size = stamina.GetComponent<RectTransform>();

            //moves bar to the right by half the extension percentage
            size.position = new Vector3(
                size.position.x + size.sizeDelta.x * (staminaExtensionEffect / (2 * CoreBars.StaminaCore.MaxValue)),
                size.position.y, size.position.z);

            size.localScale = new Vector3(
                size.localScale.x * ((staminaExtensionEffect / CoreBars.HungerCore.MaxValue) + 1), size.localScale.y,
                size.localScale.z);

            CoreBars.StaminaCore.MaxValue += staminaExtensionEffect;
            //current value is increased by extension value
            CoreBars.StaminaCore.CurrentValue += staminaExtensionEffect;

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