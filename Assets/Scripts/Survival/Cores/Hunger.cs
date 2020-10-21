using UnityEngine;

namespace Survival
{
    public class Hunger : Core
    {
        private static Hunger instance;
        
        private float maxValue;
        private float currentValue;

        private bool isEmpty;


        public static Hunger GetInstance()
        {
            return instance ?? (instance = new Hunger());
        }


        public float CurrentValue
        {
            get => currentValue; 
            set => currentValue = value;
        }

        public float MaxValue
        {
            get => maxValue; 
            set => maxValue = value;
        }

        public bool IsEmpty
        {
            get => isEmpty; 
            set => isEmpty = value;
        }


        public void Init(float maxValue)
        {
            this.maxValue = this.currentValue = maxValue;
        }
        
        public void BuffValue(float value)
        {
            float buff = value * 10f;
            
            if (this.currentValue + buff > maxValue)
            {
                this.currentValue = maxValue;
                return;
            }

            this.isEmpty = false;
            
            this.currentValue += buff;
        }

        public void DebuffValue(float value)
        {
            float debuff = value * 10f;
            
            if (this.currentValue - debuff  < 0)
            {
                this.currentValue = 0;

                this.IsEmpty = true;
                
                return;
            }
            
            this.currentValue -= debuff;
        }

        public void OnEmpty()
        {
            
        }
    }
}