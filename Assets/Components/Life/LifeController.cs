using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int _lifeCount = 3;
    
    private int _currentLifeCount;
    
    private void Start()
    {
        _currentLifeCount = _lifeCount;
        
        EventSystem.OnPlayerLifeUpdated?.Invoke(_currentLifeCount);
        EventSystem.OnPlayerCollision += HandlePlayerCollision;
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerCollision -= HandlePlayerCollision;
    }

    private void HandlePlayerCollision()
    {
        if (_currentLifeCount - 1 < 0)
        {
            // The player is dead
            return;
        }
        
        _currentLifeCount--;
        EventSystem.OnPlayerLifeUpdated?.Invoke(_currentLifeCount);
    }
}
