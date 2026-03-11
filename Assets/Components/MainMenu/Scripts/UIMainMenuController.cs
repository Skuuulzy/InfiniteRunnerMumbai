using System;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        SceneLoaderService.LoadGame();
    }
    
    public void QuitGame()
    {
        #if !UNITY_EDITOR
        Application.Quit();
        #else
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
