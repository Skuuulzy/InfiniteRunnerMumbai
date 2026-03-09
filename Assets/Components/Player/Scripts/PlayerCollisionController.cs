using UnityEngine;

namespace Components.Player.Scripts
{
    public class PlayerCollisionController : MonoBehaviour
    {
        [Header("Parameters")] 
        [SerializeField] private Vector3 _sphereCenter;
        [SerializeField] private float _sphereRadius;

        private bool _isHit;
        
        private Vector3 PlayerSpherePosition => transform.position + _sphereCenter;


        private void Start()
        {
            EventSystem.EventSystem.OnPlayerSlideDown += ShrinkCollider;
        }
        
        private void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(PlayerSpherePosition, _sphereRadius);
            if (hitColliders.Length > 0 && !_isHit)
            {
                Debug.Log("Player hit something!");
                _isHit = true;
            }

            if (hitColliders.Length == 0)
            {
                _isHit = false;
            }
        }

        public void ShrinkCollider(bool isSlidingDown)
        {
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerSpherePosition, _sphereRadius);
        }
    }
}