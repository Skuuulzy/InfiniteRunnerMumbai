using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private Transform _endAnchor;
    [SerializeField] private EnnemyController _enemyPrefab;
    [SerializeField] private float _enemyProbability = 0.3f;
    
    public Vector3 EndAnchor => _endAnchor.position;

    private void Start()
    {
        var random = Random.Range(0, 1f);
        if (random < _enemyProbability)
        {
            Instantiate(_enemyPrefab, transform);
        }
    }
    
    public bool IsBehindPlayer()
    {
        return EndAnchor.z <= 0;
    }
}
