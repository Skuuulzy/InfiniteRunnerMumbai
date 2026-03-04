using UnityEngine;

public class ChunkController : MonoBehaviour
{
    [SerializeField] private Transform _endAnchor;
    
    public Vector3 EndAnchor => _endAnchor.position;

    public bool IsBehindPlayer()
    {
        return EndAnchor.z <= 0;
    }
}
