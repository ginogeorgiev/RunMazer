public class GameStateMachine
{
    private static GameStateMachine instance;
    private State currentState = State.Playing;

    public static GameStateMachine GetInstance()
    {
        return instance ?? (instance = new GameStateMachine());
    }

    public enum State
    {
        Playing,
        Paused,
        MainMenu,
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
