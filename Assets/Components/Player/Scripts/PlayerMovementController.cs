using System;
using System.Collections;
using Components.SaveService;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Jump parameters")]
    [SerializeField,Tooltip("Duration of jump in seconds")] private float _jumpDuration = 1f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private AnimationCurve _fallCurve;

    [Header("Slide parameters")] 
    [SerializeField] private float _slideDuration = 1f;
    [SerializeField] private Transform[] _slideTarget;
    
    [Header("Slide parameters")] 
    [SerializeField] private float _slideDownDuration = 1.5f;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    
    [Header("Debug")]
    [SerializeField] private int _currentLaneIndex = 1;
    [SerializeField] private bool _isSliding;
    [SerializeField] private bool _isSlidingDown;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _locked;
    
    private Coroutine _slideCoroutine;

    private void Awake()
    {
        EventSystem.OnStateChanged += HandleStateChanged;
        _locked = true;

        var saveData = SaveService.Load();
        if (saveData.SlideSpeed > 0)
        {
            _slideDuration = saveData.SlideSpeed;
        }
        else
        {
            Debug.LogError("Slide speed is invalid.");
        }
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
        EventSystem.OnStateChanged -= HandleStateChanged;
    }
    
    private void HandleStateChanged(State newState)
    {
        if (newState is not GameState)
        {
            _locked = true;
            StopAllCoroutines();
            EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
            return;
        }
        
        _animator.SetTrigger("Running");
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
        _locked = false;
    }

    private void HandlePlayerLifeUpdated(int playerLife)
    {
        if (playerLife > 0)
        {
            _animator.SetTrigger("TakeDamage");
            return;
        }
        
        StopAllCoroutines();
        _animator.SetTrigger("Dead");
        _locked = true;
    }

    public void Update()
    {
        if (_locked)
        {
            return;
        }
        
        HandleJump();
        HandleSlide();
        HandleSlideDown();
    }

    private void HandleJump()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (_isJumping || _isSlidingDown)
            {
                return;
            }
            
            StartCoroutine(JumpCoroutine());
        }
    }
    
    private void HandleSlide()
    {
        // Slide left
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            if (_isSliding)
            {
                StopCoroutine(_slideCoroutine);
                _isSliding = false;
            }
            
            if (_currentLaneIndex == 0)
            {
                return;
            }
            
            _currentLaneIndex --;
            _slideCoroutine = StartCoroutine(SlideCoroutine(_slideTarget[_currentLaneIndex]));
        }
        
        // Slide right
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            if (_isSliding)
            {
                StopCoroutine(_slideCoroutine);
                _isSliding = false;
            }
            
            if (_currentLaneIndex == _slideTarget.Length - 1)
            {
                return;
            }
            
            _currentLaneIndex++;
            _slideCoroutine = StartCoroutine(SlideCoroutine(_slideTarget[_currentLaneIndex]));
        }
    }
    
    private void HandleSlideDown()
    {
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            if (_isSlidingDown || _isJumping)
            {
                return;
            }
            
            StartCoroutine(SlideDownCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
        _isJumping = true;
        _animator.SetBool("IsJumping", true);
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
        _animator.SetTrigger("Falling");
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
        _animator.SetBool("IsJumping", false);
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

    private IEnumerator SlideDownCoroutine()
    {
        _isSlidingDown = true;
        _animator.SetBool("IsSlidingDown", true);
        EventSystem.OnPlayerSlideDown?.Invoke(true);
        
        var slideTimer = 0f;

        while (slideTimer <= _slideDownDuration)
        {
            slideTimer += Time.deltaTime;
            yield return null;
        }
        
        _isSlidingDown = false;
        _animator.SetBool("IsSlidingDown", false);
        EventSystem.OnPlayerSlideDown?.Invoke(false);
    }
}