
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

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public float CurrentValue
        {
            get => currentValue;
            set => currentValue = value;
        }

        public float DepletingRate
        {
            get => depletingRate;
            set => depletingRate = value;
        }
    }
}