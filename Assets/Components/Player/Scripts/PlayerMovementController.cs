using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _jumpDuration = 1f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private AnimationCurve _fallCurve;

    public void Update()
    {
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            StartCoroutine(JumpCoroutine());
        }
    }

    private IEnumerator JumpCoroutine()
    {
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
    }
}