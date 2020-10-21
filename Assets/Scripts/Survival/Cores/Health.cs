using System.Runtime.CompilerServices;
using UnityEngine;

namespace Survival
{
    public class Health : Core
    {
        private static Health instance;
        
        [SerializeField] private float maxValue;
        [SerializeField] private float currentValue;

        private bool isEmpty;

        public static Health GetInstance()
        {
            return instance ?? (instance = new Health());
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
            float buff = value * 5f;
            
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
            float debuff = value * 5f;
            
            if (this.currentValue - debuff  < 0)
            {
                this.currentValue = 0;

                this.OnEmpty();
                
                return;
            }
            
            this.currentValue -= debuff;
        }

        public void OnEmpty()
        {
            
        }
    }
}