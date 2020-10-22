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
        CaffeineRush,
        IsJiggiling,
    }

    public State getState()
    {
        return currentState;
    }

    public void changeState(State state)
    {
        this.currentState = state;
    }

}
