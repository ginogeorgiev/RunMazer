using Survival;
using UnityEngine;

namespace Item
{
    
    /// <summary>
    /// Extends health by determined value (in %)
    /// </summary>
    public class Apple : Item
    {
        [SerializeField] private float healthExtensionEffect = 0.2f;
        
        protected override void EnterEffect()
        {
            GameObject health = GameObject.Find("HealthBar");

            RectTransform size = health.GetComponent<RectTransform>();
            
            //moves bar to the right by half the extension percentage
            size.position = new Vector3(size.position.x + size.sizeDelta.x * (healthExtensionEffect  / 2), size.position.y, size.position.z);
            
            size.localScale = new Vector3(size.localScale.x * (healthExtensionEffect + 1), size.localScale.y, size.localScale.z);

            CoreBars.HealthCore.MaxValue *= (healthExtensionEffect + 1);
            //current value is increased by extension value
            CoreBars.HealthCore.CurrentValue += CoreBars.HealthCore.MaxValue * healthExtensionEffect;

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