using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField] private float _jumpDuration = 1f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private AnimationCurve _fallCurve;

    [Header("Slide parameters")] 
    [SerializeField] private float _slideDuration = 1f;
    [SerializeField] private Transform[] _slideTarget;

    [Header("Debug")]
    [SerializeField] private int _currentLaneIndex = 1;
    [SerializeField] private bool _isSliding;
    [SerializeField] private bool _isJumping;
    
    
    public void Update()
    {
        // Jump
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (_isJumping)
            {
                return;
            }
            
            StartCoroutine(JumpCoroutine());
        }

        // Slide left
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            if (_isSliding)
            {
                return;
            }
            
            if (_currentLaneIndex == 0)
            {
                return;
            }
            
            _currentLaneIndex --;
            StartCoroutine(SlideCoroutine(_slideTarget[_currentLaneIndex]));
        }
        
        // Slide right
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            if (_isSliding)
            {
                return;
            }
            
            if (_currentLaneIndex == _slideTarget.Length - 1)
            {
                return;
            }
            
            _currentLaneIndex++;
            StartCoroutine(SlideCoroutine(_slideTarget[_currentLaneIndex]));
        }
    }

    private IEnumerator JumpCoroutine()
    {
        _isJumping = true;
        float jumpTimer = 0f;
        float halfJumpDuration = _jumpDuration / 2f;

        // Jump
        while (jumpTimer < halfJumpDuration)
        {
            jumpTimer += Time.deltaTime;
            var normalizedTime = jumpTimer / halfJumpDuration;

            var targetHeight = _jumpCurve.Evaluate(normalizedTime) * _jumpHeight;
            var targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);

            transform.position = targetPosition;

            yield return null;
        }
        
        // Fall
        jumpTimer = 0f;
        
        while (jumpTimer < halfJumpDuration)
        {
            jumpTimer += Time.deltaTime;
            var normalizedTime = jumpTimer / halfJumpDuration;

            var targetHeight = _fallCurve.Evaluate(normalizedTime) * _jumpHeight;
            var targetPosition = new Vector3(transform.position.x, targetHeight, transform.position.z);

            transform.position = targetPosition;

            yield return null;
        }

        _isJumping = false;
    }

    private IEnumerator SlideCoroutine(Transform target)
    {
        _isSliding = true;
        var slideTimer = 0f;
        
        while (slideTimer < _slideDuration)
        {
            slideTimer += Time.deltaTime;

            var normalizedTime = slideTimer / _slideDuration;
            var targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

            transform.position = Vector3.Lerp(transform.position, targetPosition, normalizedTime);
            
            yield return null;
        }

        _isSliding = false;
    }
}