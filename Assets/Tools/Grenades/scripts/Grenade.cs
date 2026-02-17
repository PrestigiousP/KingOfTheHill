using System.Collections;
using Characters.scripts.Interfaces;
using Characters.scripts.Shared;
using UnityEngine;

namespace Tools.Grenades.scripts
{
    public class Grenade : Throwable
    {
        [SerializeField] public float explosionRadius = 8f;
        [SerializeField] public float explosionForce = 20;
        
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            var projectile = ThrowProjectile();
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
    }
}
