using Survival;
using UnityEngine;

namespace Item
{
    public class SmallSatchel : Item
    {
        protected override void EnterEffect()
        {
            GameObject hunger = GameObject.Find("HungerBar");

            RectTransform size = hunger.GetComponent<RectTransform>();
            
            size.position = new Vector3(size.position.x + size.sizeDelta.x * 0.1f, size.position.y, size.position.z);
            
            size.localScale = new Vector3(size.localScale.x * 1.2f, size.localScale.y, size.localScale.z);

            CoreBars.HungerCore.MaxValue *= 1.2f;
            CoreBars.HungerCore.CurrentValue += CoreBars.HungerCore.MaxValue * 0.2f;
            
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