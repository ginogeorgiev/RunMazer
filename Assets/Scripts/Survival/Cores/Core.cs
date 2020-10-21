namespace Survival
{
    public interface Core
    {
        float CurrentValue { get; set; }
        float MaxValue { get; set; }
        
        bool IsEmpty { get; set; }
        
        void Init(float maxValue);
        
        void BuffValue(float value);
        void DebuffValue(float value);

        void OnEmpty();
    }
}