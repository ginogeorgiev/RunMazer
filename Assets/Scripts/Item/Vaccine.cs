using Survival;
using UnityEngine;

namespace Item
{
    
    /// <summary>
    /// decreases health depleting rate to given percentage of initial value
    /// </summary>
    public class Vaccine : Item
    {
        [SerializeField] private float healthDepletingEffect = 0.8f;
        
        protected override void EnterEffect()
        {
            CoreBars.HealthCore.DepletingRate *= healthDepletingEffect;
            
            Debug.Log("health depleting rate down");

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