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

        public void Init(float value)
        {
            this.maxValue = this.currentValue = value;
        }

        public void BuffValue(float buff)
        {
            //ceiling for currentValue
            if (this.currentValue + buff > maxValue)
            {
                this.currentValue = maxValue;
                return;
            }

            if(this.isEmpty) this.isEmpty = false;
            
            this.currentValue += buff;
        }

        public void DebuffValue(float debuff)
        {
            if (this.currentValue - debuff  < 0)
            {
                this.currentValue = 0;

                this.OnEmpty();
                
                return;
            }
            
            this.currentValue -= debuff;
        }

        //TODO implement game over state on empty
        public void OnEmpty()
        {
            this.isEmpty = true;
        }
    }
}