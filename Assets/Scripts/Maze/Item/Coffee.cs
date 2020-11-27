using Survival;
using UnityEngine;

namespace Item
{
    
    /// <summary>
    /// sets stamina depleting rate to given percentage of initial value
    /// </summary>
    public class Coffee : Item
    {
        [SerializeField] private float staminaDepletingEffect = 0.8f;
        
        protected override void EnterEffect()
        {
            CoreBars.StaminaCore.DepletingRate *= staminaDepletingEffect;
            
            Debug.Log("Stamina depleting rate down");
            
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