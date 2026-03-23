using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    [SerializeField] private ProjectileController _projectile;
    [SerializeField] private float _projectileThrowingInterval = 1f;
    
    private Transform _playerTransform;
    
    private float _throwProjectileTimer;
    
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ThrowProjectile(_playerTransform.GetChild(0).position);
    }

    private void Update()
    {
        _throwProjectileTimer += Time.deltaTime;
        if (_throwProjectileTimer < _projectileThrowingInterval)
        {
            return;
        }
        
        ThrowProjectile(_playerTransform.GetChild(0).position);
        _throwProjectileTimer = 0;
    }
    
    private void ThrowProjectile(Vector3 position)
    {
        var projectile = Instantiate(_projectile, transform);
        projectile.LaunchTowards(position);
    }
}
