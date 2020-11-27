using Survival;
using UnityEngine;

namespace Item
{
    
    /// <summary>
    /// sets health to max
    /// sets hunger depleting to given percentage of initial value
    /// sets stamina depleting to given percentage of initial value
    /// </summary>
    public class J : Item
    {
        [SerializeField] private float hungerDepletingEffect = 1.2f;
        [SerializeField] private float staminaDepletingEffect = 1.2f;
        
        protected override void EnterEffect()
        {
            CoreBars.HealthCore.CurrentValue = CoreBars.HealthCore.MaxValue;
            CoreBars.HungerCore.DepletingRate *= hungerDepletingEffect;
            CoreBars.StaminaCore.DepletingRate *= staminaDepletingEffect;
            
            Debug.Log("hunger depleting rate up");
            Debug.Log("stamina depleting rate up");
            
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