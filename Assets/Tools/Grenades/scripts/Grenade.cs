using System.Collections;
using Characters.Players.scripts;
using Classes.Interfaces;
using UnityEngine;

namespace Tools.Grenades.scripts
{
    public class Grenade : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public Transform throwPoint;
        public float throwForce = 50f;
        public float throwAngleY = 0.75f; 
        public float explosionRadius = 8f;
        public float explosionForce = 20;

        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }
        
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            ThrowProjectile();
            StartCoroutine(ExplodeAfterDelay(projectilePrefab, 2f));
        }

        private IEnumerator ExplodeAfterDelay(GameObject projectile, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            Debug.Log("Should explode.");
    
            if (!projectile) yield break;
            
            var component = projectile.GetComponent<Renderer>();
            if (component)
            {
                component.material.color = Color.red;
            }
        
            Explode(projectile.transform.position);
        
            Destroy(projectile, 0.1f);
        }

        private void Explode(Vector3 explosionPosition)
        {
            var colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
    
            foreach (var nearbyObject in colliders)
            {
                if (!nearbyObject.TryGetComponent<IKnockBack>(out var knockBack)) continue;
                
                var direction = nearbyObject.transform.position - explosionPosition;
               
                knockBack.KnockBack(direction, explosionForce);
            }
        }

        private void ThrowProjectile()
        {
            var projectile = Instantiate(
                projectilePrefab, 
                throwPoint.position, 
                throwPoint.rotation
            );

            var projectileType = projectile.GetComponent<IRigidbody>();
            
            var direction = (_player.Forward + new Vector3(0, _player.Forward.y + throwAngleY, 0)).normalized;
            projectileType.Rigidbody.AddForce(direction * throwForce, ForceMode.Impulse);
        }
    }
}
