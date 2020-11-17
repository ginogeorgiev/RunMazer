
using System;
using UnityEngine.UI;

namespace Survival
{
    public class Core
    {
        private Slider bar;
        
        private float maxValue;
        private float currentValue;
        private float depletingRate;

        public Core(Slider slider, float maxValue, float depletingRate)
        {
            this.bar = slider;
            this.maxValue = this.currentValue = maxValue;
            this.depletingRate = depletingRate;
        }

        public Slider Bar
        {
            get => bar;
            set => bar = value;
        }

        public float UpdateCore()
        {
            bar.value = this.currentValue;

            return currentValue;
        }

        public bool DepleteCore(float debuff)
        {
            //bottom for currentValue
            if (this.currentValue - debuff * depletingRate  < 0)
            {
                this.currentValue = 0;

                return false;
            }
            
            this.currentValue -= debuff * depletingRate;

            return true;
        }

        public bool IncreaseCore(float buff)
        {
        //ceiling for currentValue
        if (this.currentValue + buff > maxValue)
        {
            this.currentValue = maxValue;
            
            return false;
        }

        this.currentValue += buff;

        return true;
        }

        public void EmptyCore()
        {
            this.currentValue = 0;
        }
    }
}