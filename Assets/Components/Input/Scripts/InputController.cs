using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static event Action OnJump;
    public static event Action OnSlideDown;
    public static event Action OnSlideLeft;
    public static event Action OnSlideRight;
        
    public void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            OnJump?.Invoke();
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            OnSlideDown?.Invoke();
        }
        
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            OnSlideLeft?.Invoke();
        }
        
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            OnSlideRight?.Invoke();
        }
    }
}