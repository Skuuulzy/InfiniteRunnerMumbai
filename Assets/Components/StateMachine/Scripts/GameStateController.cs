using UnityEngine;

public class GameStateController: MonoBehaviour
{
    private StateMachine _stateMachine;
    
    private void Start()
    {
        _stateMachine = new StateMachine();
        var initialState = new CountdownState(_stateMachine);
        
        _stateMachine.ChangeState(initialState);
    }
    
    private void Update()
    {
        _stateMachine.Update();
    }
}