using Survival;
using UnityEngine;

namespace Item
{
    public class Apple : Item
    {
        protected override void EnterEffect()
        {
            GameObject health = GameObject.Find("HealthBar");

            RectTransform size = health.GetComponent<RectTransform>();
            
            size.position = new Vector3(size.position.x + size.sizeDelta.x * 0.1f, size.position.y, size.position.z);
            
            size.localScale = new Vector3(size.localScale.x * 1.2f, size.localScale.y, size.localScale.z);

            CoreBars.HealthCore.MaxValue *= 1.2f;
            CoreBars.HealthCore.CurrentValue += CoreBars.HealthCore.MaxValue * 0.2f;

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