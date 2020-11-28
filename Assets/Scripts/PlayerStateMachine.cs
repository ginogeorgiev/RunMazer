using System.Collections.Generic;
using System.Linq;

public class PlayerStateMachine
{
    private static PlayerStateMachine instance;
    private State currentState = State.IsIdle;

    public static PlayerStateMachine GetInstance()
    {
        return instance ?? (instance = new PlayerStateMachine());
    }

    public enum State
    {
        IsIdle,
        IsWalking,
        IsRunning,
    }

    public State GetState()
    {
        return currentState;
    }

    public void ChangeState(State state)
    {
        this.currentState = state;
    }

}
