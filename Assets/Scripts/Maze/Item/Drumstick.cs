using Survival;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Item
{
    
    /// <summary>
    /// Extends hunger by determined value
    /// </summary>
    public class Drumstick : MazeItem
    {
        [SerializeField] private float hungerExtensionEffect = 20f;
        
        protected override void EnterEffect()
        {
            Slider hunger = CoreBars.HungerCore.Bar;

            RectTransform size = hunger.GetComponent<RectTransform>();
            
            //moves bar to the right by half the extension percentage
            size.position = new Vector3(
                size.position.x + size.sizeDelta.x * (hungerExtensionEffect * 0.01f) / 2,
                size.position.y, size.position.z);

            size.localScale = new Vector3(
                size.localScale.x + hungerExtensionEffect * 0.01f, size.localScale.y,
                size.localScale.z);

            CoreBars.HungerCore.MaxValue += hungerExtensionEffect;
            //current value is increased by extension value
            CoreBars.HungerCore.CurrentValue += hungerExtensionEffect;
            
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