using System.Text.RegularExpressions;
using Survival;
using UnityEngine;

namespace Item
{
    public class Coffee : Item
    {
        protected override void EnterEffect()
        {
            GameObject stamina = GameObject.Find("StaminaBar");

            RectTransform size = stamina.GetComponent<RectTransform>();
            
            size.position = new Vector3(size.position.x + size.sizeDelta.x * 0.1f, size.position.y, size.position.z);
            
            size.localScale = new Vector3(size.localScale.x * 1.2f, size.localScale.y, size.localScale.z);

            CoreBars.StaminaCore.MaxValue *= 1.2f;

            CoreBars.StaminaCore.CurrentValue += CoreBars.StaminaCore.MaxValue * 0.2f;
            
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