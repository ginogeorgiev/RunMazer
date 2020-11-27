using Survival;
using UnityEngine;

namespace Item
{
    
    /// <summary>
    /// increases stamina currentValue by given value
    /// </summary>
    public class O2Tank : Item
    {
        [SerializeField] private float staminaValueEffect = 20f;
        
        protected override void EnterEffect()
        {
            CoreBars.StaminaCore.CurrentValue += staminaValueEffect;
            
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