using UnityEngine;

public class GameOverState : State
{
    public GameOverState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Game Over Enter");
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        Debug.Log("Game Over Exit");
    }
}