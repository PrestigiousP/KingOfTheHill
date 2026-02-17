using System;
using Characters.Players.scripts;
using UnityEngine;

namespace Characters.scripts.Shared
{
    public class Throwable : MonoBehaviour
    {
        [SerializeField] public GameObject projectilePrefab;
        [SerializeField] public Transform throwPoint;
        [SerializeField] public float throwForce = 50f;
        [SerializeField] public float throwAngleY = 0.75f;
        
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }
    
        protected GameObject ThrowProjectile()
        {
            var projectile = Instantiate(
                projectilePrefab, 
                throwPoint.position, 
                throwPoint.rotation
            );
        
            var rb = projectile.GetComponent<Rigidbody>();
            if (!rb)
            {
                throw new InvalidOperationException("Projectile component doesn't contain a Rigidbody");
            }
            
            var direction = (_player.Forward + new Vector3(0, _player.Forward.y + throwAngleY, 0)).normalized;
            rb.AddForce(direction * throwForce, ForceMode.Impulse);
            
            return projectile;
        }
    }
}
