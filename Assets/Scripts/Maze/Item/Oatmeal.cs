using Survival;
using UnityEngine;

namespace Item
{
    
    /// <summary>
    /// decreases hunger depleting rate to given percentage of initial value
    /// </summary>
    public class Oatmeal : Item
    {
        [SerializeField] private float hungerDepletingEffect = 0.8f;
        
        protected override void EnterEffect()
        {
            CoreBars.HungerCore.DepletingRate *= hungerDepletingEffect;

            Debug.Log("hunger depleting rate down");

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