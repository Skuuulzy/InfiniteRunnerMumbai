using System;
using UnityEngine;

public class GameState : State
{
    public GameState(StateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        Debug.Log("Game Started");
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Game Exit");
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
    }
    
    private void HandlePlayerLifeUpdated(int playerLife)
    {
        if (playerLife > 0)
        {
            return;
        }
        
        var gameOverState = new GameOverState(StateMachine);
        StateMachine.ChangeState(gameOverState);
    }
}