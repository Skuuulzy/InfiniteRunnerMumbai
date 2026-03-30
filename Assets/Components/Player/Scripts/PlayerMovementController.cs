using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Transform[] _slideTarget;
    
    [Header("Debug")]
    [SerializeField] private int _currentLaneIndex = 1;
    [SerializeField] private bool _isSliding;
    [SerializeField] private bool _isSlidingDown;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _locked;
    
    private float _jumpDuration;
    private float _jumpHeight;
    private AnimationCurve _jumpCurve;
    private AnimationCurve _fallCurve;
    
    private float _slideDuration ;
    private float _slideDownDuration;
    private Animator _animator;
    
    private Coroutine _slideCoroutine;

    public void Initialize(CharacterTemplateSO characterTemplate, Animator animator)
    {
        if (animator == null)
        {
            Debug.LogError("Animator is null");
        }
        _animator = animator;
        
        _jumpDuration = characterTemplate.JumpDuration;
        _jumpHeight = characterTemplate.JumpHeight;
        _jumpCurve = characterTemplate.JumpCurve;
        _fallCurve = characterTemplate.FallCurve;
        
        _slideDuration = characterTemplate.SlideDuration;
        _slideDownDuration = characterTemplate.SlideDownDuration;
        
        _inputBuffer = new InputBuffer();
        
        EventSystem.OnStateChanged += HandleStateChanged;
        
        InputController.OnJump += HandleJump;
        InputController.OnSlideLeft += HandleSlideLeft;
        InputController.OnSlideRight += HandleSlideRight;
        InputController.OnSlideDown += HandleSlideDown;
        
        _locked = true;
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerLifeUpdated -= HandlePlayerLifeUpdated;
        EventSystem.OnStateChanged -= HandleStateChanged;
        
        InputController.OnJump -= HandleJump;
        InputController.OnSlideLeft -= HandleSlideLeft;
        InputController.OnSlideRight -= HandleSlideRight;
        InputController.OnSlideDown -= HandleSlideDown;
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

        if (_animator)
            _animator.SetTrigger("Running");
        EventSystem.OnPlayerLifeUpdated += HandlePlayerLifeUpdated;
        _locked = false;
    }

    private void HandlePlayerLifeUpdated(int playerLife)
    {
        if (playerLife > 0)
        {
            if (_animator)
                _animator.SetTrigger("TakeDamage");
            return;
        }
        
        StopAllCoroutines();
        
        if (_animator)
            _animator?.SetTrigger("Dead");
        _locked = true;
    }

    private void HandleJump()
    {
        if (_locked)
        {
            return;
        }
        
        _inputBuffer.Buffer("Jump");
        
        if (!_isJumping && !_isSlidingDown && _inputBuffer.TryConsume("Jump"))
        {
            StartCoroutine(JumpCoroutine());
        }
    }

    private void HandleSlideLeft()
    {
        if (_locked)
        {
            return;
        }
        
        if (_isSliding)
        {
            StopCoroutine(_slideCoroutine);
            _isSliding = false;
        }

        if (_currentLaneIndex == 0)
        {
            return;
        }

        _currentLaneIndex--;
        _slideCoroutine = StartCoroutine(SlideCoroutine(_slideTarget[_currentLaneIndex]));
    }

    private void HandleSlideRight()
    {
        if (_locked)
        {
            return;
        }
        
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

    private void HandleSlideDown()
    {
        if (_locked)
        {
            return;
        }
        
        _inputBuffer.Buffer("SlideDown");
        
        if (!_isSlidingDown && !_isJumping && _inputBuffer.TryConsume("SlideDown"))
        {
            StartCoroutine(SlideDownCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
        _isJumping = true;
        
        if (_animator)
            _animator?.SetBool("IsJumping", true);
        
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
        if (_animator)
            _animator?.SetTrigger("Falling");
        
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
        
        if (_animator)
            _animator?.SetBool("IsJumping", false);
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
        
        if (_animator)
            _animator?.SetBool("IsSlidingDown", true);
        
        EventSystem.OnPlayerSlideDown?.Invoke(true);
        
        var slideTimer = 0f;

        while (slideTimer <= _slideDownDuration)
        {
            slideTimer += Time.deltaTime;
            yield return null;
        }
        
        _isSlidingDown = false;
        
        if (_animator)
            _animator?.SetBool("IsSlidingDown", false);
        
        EventSystem.OnPlayerSlideDown?.Invoke(false);
    }
}