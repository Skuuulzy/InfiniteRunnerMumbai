using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoaderService
{
    public static void LoadGame()
    {
        Debug.Log("Loading Game...");
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
        SceneManager.LoadScene("LevelUI", LoadSceneMode.Additive);
        Debug.Log("Game loaded");
    }
    
    public static void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        Debug.Log("Main Menu loaded");
    }
}