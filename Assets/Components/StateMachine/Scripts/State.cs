public abstract class State
{
    protected readonly StateMachine StateMachine;
    
    protected State(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}