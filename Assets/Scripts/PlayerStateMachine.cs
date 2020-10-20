using System.Collections.Generic;
using System.Linq;

public class PlayerStateMachine
{
    private static PlayerStateMachine instance;
    private List<State> currentState = new List<State>();

    public static PlayerStateMachine GetInstance()
    {
        return instance ?? (instance = new PlayerStateMachine());
    }

    public enum State
    {
        IsInBase,
        IsEating,
        IsStarving,
        IsTired,
    }

    public bool CheckForState(State state)
    {
        return currentState.Any(t => t == state);
    }

    public void AddState(State state)
    {
        if (!this.CheckForState(state))
        {
            this.currentState.Add(state);
        }
    }

    public void RemoveState(State state)
    {
        if (this.CheckForState(state))
        {
            this.currentState.Remove(state);
        }
    }

}
