using UnityEngine;

public class GameState : State
{
    public GameState(StateMachine stateMachine) : base(stateMachine) { }
    
    // The timer since the game started in seconds.
    public int Timer => Mathf.RoundToInt(_timer);

    private float _timer;
    
    public override void Enter()
    {
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
        _timer = 0;
    }

    public override void Update()
    {
        _timer += Time.deltaTime;
    }

    public override void Exit()
    {
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