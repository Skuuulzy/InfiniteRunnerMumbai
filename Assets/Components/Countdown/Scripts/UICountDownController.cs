using System;
using TMPro;
using UnityEngine;

public class UICountDownController : MonoBehaviour
{
    [SerializeField] private GameObject _window;
    [SerializeField] private TMP_Text _countdownText;
    
    private bool _inCountdown;
    private CountdownState _countdownState;
    
    private void Awake()
    {
        _window.SetActive(false);
        EventSystem.OnStateChanged += HandleStateChanged;
    }
    
    private void OnDestroy()
    {
        EventSystem.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(State state)
    {
        if (state is not CountdownState countdownState)
        {
            _inCountdown = false;
            _window.SetActive(false);
            return;
        }

        _window.SetActive(true);
        _countdownState = countdownState;
        _inCountdown = true;
    }

    private void Update()
    {
        if (!_inCountdown)
        {
            return;
        }

        _countdownText.text = _countdownState.Timer.ToString("0");
    }
}