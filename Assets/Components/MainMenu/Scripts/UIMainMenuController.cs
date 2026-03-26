using System;
using Components.SaveService;
using TMPro;
using UnityEngine;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text _runCountText;
    [SerializeField] private TMP_Text _bestTimeText;
    private SaveData _saveData;
    
    private void Start()
    {
        _saveData = SaveService.Load();
        _runCountText.text = "Attempts: " + _saveData.RunCount;

        if (_saveData.BestTime == 0)
        {
            _bestTimeText.text = "No Best Time";
        }
        else
        {
            var timeSpan = new TimeSpan(0, 0, _saveData.BestTime);
            _bestTimeText.text = "Best Time: " + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
        }
    }

    public void StartGame(string characterName)
    {
        _saveData.RunCount++;
        _saveData.SelectedCharacterName = characterName;
        SaveService.Save(_saveData);
        
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
