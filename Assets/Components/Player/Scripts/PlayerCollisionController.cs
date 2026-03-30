using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    private Vector3 _sphereCenter;
    private float _sphereRadius;
    private Vector3 _shrinkSphereCenter;
    private float _shrinkSphereRadius;
    
    private bool _isHit;
    
    private Vector3 _currentSphereCenter;
    private float _currentSphereRadius;

    private Vector3 PlayerSpherePosition => transform.position + _currentSphereCenter;

    public void Initialize(CharacterTemplateSO characterTemplate)
    {
        _sphereCenter = characterTemplate.SphereCenter;
        _sphereRadius = characterTemplate.SphereRadius;
        _shrinkSphereCenter = characterTemplate.ShrinkSphereCenter;
        _shrinkSphereRadius = characterTemplate.ShrinkSphereRadius;
        
        _currentSphereCenter = _sphereCenter;
        _currentSphereRadius = _sphereRadius;
        
        EventSystem.OnPlayerSlideDown += ShrinkCollider;
    }
    
    private void OnDestroy()
    {
        EventSystem.OnPlayerSlideDown -= ShrinkCollider;
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, _currentSphereRadius);
        if (hitColliders.Length > 0 && !_isHit)
        {
            EventSystem.OnPlayerCollision?.Invoke();
            _isHit = true;
        }

        if (hitColliders.Length == 0)
        {
            _isHit = false;
        }
    }

    private void ShrinkCollider(bool isSlidingDown)
    {
        if (isSlidingDown)
        {
            _currentSphereCenter = _shrinkSphereCenter;
            _currentSphereRadius = _shrinkSphereRadius;
        }
        else
        {
            _currentSphereCenter = _sphereCenter;
            _currentSphereRadius = _sphereRadius;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _sphereCenter, _sphereRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + _shrinkSphereCenter, _shrinkSphereRadius);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(PlayerSpherePosition, _currentSphereRadius);
    }
}