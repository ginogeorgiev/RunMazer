namespace Survival
{
    public interface Core
    {
        float CurrentValue { get; set; }
        float MaxValue { get; set; }
        bool IsEmpty { get; set; }
     
        //sets maxValue and currentValue to value
        void Init(float value);
        
        //increase/diminish currentValue
        void BuffValue(float buff);
        void DebuffValue(float debuff);

        //dictates what happens if Core is empty
        void OnEmpty();
    }
}