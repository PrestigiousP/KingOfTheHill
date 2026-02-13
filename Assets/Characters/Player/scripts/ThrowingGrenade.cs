using System.Collections;
using Characters.scripts.Interfaces;
using UnityEngine;

namespace Characters.Player.scripts
{
    public class ThrowingGrenade : MonoBehaviour
    {
        [SerializeField] public GameObject projectilePrefab;
        [SerializeField] public Transform throwPoint;
        [SerializeField] public float throwForce = 10f;
        [SerializeField] public float explosionRadius = 2f;
        [SerializeField] public float explosionForce = 10;
        
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
                var direction = (_player.Forward + new Vector3(0, _player.Forward.y + 0.75f, 0)).normalized;
                rb.AddForce(direction * throwForce, ForceMode.Impulse);
            }
            
            // Start explosion timer
            StartCoroutine(ExplodeAfterDelay(projectile, 2f));
        }

        private IEnumerator ExplodeAfterDelay(GameObject projectile, float delay)
        {
            yield return new WaitForSeconds(delay);
    
            if (!projectile) yield break;
            
            var component = projectile.GetComponent<Renderer>();
            if (component)
            {
                component.material.color = Color.red;
            }
        
            Explode(projectile.transform.position);
        
            Destroy(projectile, 0.1f);
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawSphere(transform.position, 2f);
        // }

        private void Explode(Vector3 explosionPosition)
        {
            var colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
    
            foreach (var nearbyObject in colliders)
            {
                if (!nearbyObject.TryGetComponent<IKnockBack>(out var knockBack)) continue;
                
                var direction = nearbyObject.transform.position - explosionPosition;
               
                knockBack.KnockBack(direction, explosionForce);

                // var knockback = nearbyObject.TryGetComponent<IKnockBack>();
               //
               // var direction = nearbyObject.transform.position - explosionPosition;
               //
               // knockback.KnockBack(direction, explosionForce);
            }
        }
    }
}
