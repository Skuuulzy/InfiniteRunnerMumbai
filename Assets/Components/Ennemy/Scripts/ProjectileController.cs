using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _projectileLifetime = 5f;
    
    private Vector3 _targetPosition;
    private float _timer;
    
    public void LaunchTowards(Vector3 position)
    {
        _targetPosition = position;
    }
    
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (transform.position == _targetPosition)
        {
            Destroy(gameObject);
        }
        
        _timer += Time.deltaTime;
        if (_timer > _projectileLifetime)
        {
            Destroy(gameObject);
        }
    }
}
