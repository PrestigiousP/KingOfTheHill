using UnityEngine;

namespace Characters.Player.scripts
{
    public class ThrowingGrenade : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public Transform throwPoint;
        public float throwForce = 10f;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ThrowProjectile();
            }
        }

        private void ThrowProjectile()
        {
            // Spawn the prefab
            var projectile = Instantiate(
                projectilePrefab, 
                throwPoint.position, 
                throwPoint.rotation
            );
        
            // TODO: refactor éventuellement
            // Apply force to throw it
            var rb = projectile.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
            }
        
            // Optional: Destroy after 5 seconds
            Destroy(projectile, 5f);
        }
    }
}
