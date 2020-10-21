namespace Survival
{
    public class Stamina : Core
    {
        private static Stamina instance;

        private float currentValue;
        private float maxValue;

        private bool isEmpty;


        public static Stamina getInstance()
        {
            return instance ?? (instance = new Stamina());
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
            this.maxValue = this.CurrentValue = maxValue;
        }

        public void BuffValue(float value)
        {
            float buff = value * 15f;
            
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

                this.isEmpty = true;
                
                return;
            }
            
            this.currentValue -= debuff;
        }

        public void OnEmpty()
        {
            
        }
    }
}