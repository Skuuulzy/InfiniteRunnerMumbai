using UnityEngine;

public class GameState : State
{
    public GameState(StateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("Game Started");
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        Debug.Log("Game Exit");
    }
}