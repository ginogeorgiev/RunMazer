namespace Survival
{
    public class Stamina : Core
    {
        private static Stamina instance;

        private float currentValue;
        private float maxValue;

        private bool isEmpty;


        public static Stamina GetInstance()
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
        public void Init(float value)
        {
            this.maxValue = this.CurrentValue = value;
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

                this.isEmpty = true;
                
                return;
            }
            
            this.currentValue -= debuff;
        }

        public void OnEmpty()
        {
            this.isEmpty = true;
        }
    }
}