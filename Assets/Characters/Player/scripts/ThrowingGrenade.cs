using UnityEngine;

namespace Characters.Player.scripts
{
    public class ThrowingGrenade : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public Transform throwPoint;
        public float throwForce = 10f;
        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Debug.DrawLine(throwPoint.position, throwPoint.rotation * throwPoint.forward, Color.red);
                ThrowProjectile();
            }
            
        }
        
        private void ThrowProjectile()
        {
            var projectile = Instantiate(
                projectilePrefab, 
                throwPoint.position, 
                throwPoint.rotation
            );
        
            // TODO: refactor éventuellement
            var rb = projectile.GetComponent<Rigidbody>();
            if (rb)
            {
                Debug.Log("camera angle: " + _player.Forward);
                
                var direction = (_player.Forward + new Vector3(0, _player.Forward.y + 0.5f, 0)).normalized;
                
                // throwPoint.forward
                rb.AddForce(direction * throwForce, ForceMode.Impulse);
            }
        
            // Optional: Destroy after 5 seconds
            Destroy(projectile, 5f);
        }
    }
}
