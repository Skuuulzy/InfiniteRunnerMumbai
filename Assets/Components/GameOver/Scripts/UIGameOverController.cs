using UnityEngine;

public class UIGameOverController : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverScreen;
    
    private void Awake()
    {
        _gameOverScreen.SetActive(false);
        EventSystem.OnStateChanged += HandleStateChanged;
    }

    private void OnDestroy()
    {
        EventSystem.OnStateChanged -= HandleStateChanged;
    }
    
    private void HandleStateChanged(State newState)
    {
        _gameOverScreen.SetActive(newState is GameOverState);
    }
    
    public void LoadMainMenu()
    {
        SceneLoaderService.LoadMainMenu();
    }
}
