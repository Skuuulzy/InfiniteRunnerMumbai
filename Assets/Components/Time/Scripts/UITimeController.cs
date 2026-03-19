using System;
using TMPro;
using UnityEngine;

public class UITimeController : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    
    private GameState _gameState;
    private bool _inGameState;
    
    private void Awake()
    {
        EventSystem.OnStateChanged += HandleStateChanged;
        _timeText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventSystem.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(State newState)
    {
        if (newState is not GameState gameState)
        {
            _inGameState = false;
            _timeText.gameObject.SetActive(false);
            return;
        }
        
        _gameState = gameState;
        _inGameState = true;
        _timeText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_inGameState)
        {
            return;
        }

        var timeSpan = new TimeSpan(0, 0, _gameState.Timer);
        
        // Display time on the following format mm:ss
        _timeText.text = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
    }
}
