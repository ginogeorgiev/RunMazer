using Survival;
using UnityEngine;

namespace Item
{
    
    /// <summary>
    /// Extends stamina by determined value (in %)
    /// </summary>
    public class Shoes : Item
    {
        [SerializeField] private float staminaExtensionEffect = 0.2f;
        
        protected override void EnterEffect()
        {
            GameObject stamina = GameObject.Find("StaminaBar");

            RectTransform size = stamina.GetComponent<RectTransform>();
            
            //moves bar to the right by half the extension percentage
            size.position = new Vector3(size.position.x + size.sizeDelta.x * (staminaExtensionEffect / 2), size.position.y, size.position.z);
            
            size.localScale = new Vector3(size.localScale.x * (staminaExtensionEffect + 1), size.localScale.y, size.localScale.z);

            CoreBars.StaminaCore.MaxValue *= (staminaExtensionEffect + 1);
            //current value is increased by extension value
            CoreBars.StaminaCore.CurrentValue += CoreBars.StaminaCore.MaxValue * staminaExtensionEffect;
            
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