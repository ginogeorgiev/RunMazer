using Survival;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Item
{
    
    /// <summary>
    /// Extends stamina by determined value (in %)
    /// </summary>
    public class SmallSatchel : MazeItem
    {
        [SerializeField] private float hungerExtensionEffect = 0.2f;
        
        protected override void EnterEffect()
        {
            Slider hunger = CoreBars.HungerCore.Bar;

            RectTransform size = hunger.GetComponent<RectTransform>();
            
            //moves bar to the right by half the extension percentage
            size.position = new Vector3(size.position.x + size.sizeDelta.x * (hungerExtensionEffect / 2), size.position.y, size.position.z);
            
            size.localScale = new Vector3(size.localScale.x * (hungerExtensionEffect + 1), size.localScale.y, size.localScale.z);

            CoreBars.HungerCore.MaxValue *= (hungerExtensionEffect + 1);
            //current value is increased by extension value
            CoreBars.HungerCore.CurrentValue += CoreBars.HungerCore.MaxValue * hungerExtensionEffect;
            
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