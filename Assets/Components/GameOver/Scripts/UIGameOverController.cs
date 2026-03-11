using UnityEngine;

public class UIGameOverController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    
    private void Awake()
    {
        _gameOverScreen.SetActive(false);
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLife;
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLife;
    }

    private void HandlePlayerLife(int playerLife)
    {
        if (playerLife > 0)
        {
            return;
        }
        
        // Show game over screen
        _gameOverScreen.SetActive(true);
    }
    
    public void LoadMainMenu()
    {
        SceneLoaderService.LoadMainMenu();
    }
}
