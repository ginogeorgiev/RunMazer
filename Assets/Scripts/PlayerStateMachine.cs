public class PlayerStateMachine
{
    private static PlayerStateMachine instance;
    private State currentState = State.IsEating;

    public static PlayerStateMachine GetInstance()
    {
        return instance ?? (instance = new PlayerStateMachine());
    }

    public enum State
    {
        HasHunger,
        IsEating,
    }

    public State GetState()
    {
        return currentState;
    }

    public void SetState(State state)
    {
        if (currentState != state)
        {
            this.currentState = state;
        }
    }

}
