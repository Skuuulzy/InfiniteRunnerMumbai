using UnityEngine;

public class CountdownState :State
{
    private float _initialTime = 3f;
    private float _timer;
    
    public float Timer => _timer;

    public CountdownState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Countdown started");
        _timer = _initialTime;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0)
        {
            return;
        }
        
        // Go to game state
        var gameState = new GameState(StateMachine);
        StateMachine.ChangeState(gameState);
    }

    public override void Exit()
    {
        Debug.Log("Countdown finished");
    }
}