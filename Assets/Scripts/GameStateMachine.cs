public class GameStateMachine
{
    private static GameStateMachine _instance;
    private State _currentState = State.Playing;

    public static GameStateMachine GetInstance()
    {
        return _instance ?? (_instance = new GameStateMachine());
    }

    public enum State
    {
        Playing,
        Paused,
        MainMenu,
    }

    public State GetState()
    {
        return _currentState;
    }

    public void SetState(State state)
    {
        if (_currentState != state)
        {
            this._currentState = state;
        }
    }

}
