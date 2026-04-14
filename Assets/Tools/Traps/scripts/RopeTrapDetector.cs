using UnityEngine;
using System;
using UnityEngine.Serialization;

namespace Tools.Traps.scripts
{
    public class RopeTrapDetector : MonoBehaviour
    {
        public TrapTag TrapTag { get; set; }
        public Rigidbody Rigidbody { get; private set; }
        
        public event Action<TrapTag, GameObject> OnTrapCollided;

        private GameObject _collidedObject;
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (Rigidbody == null) return;

            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            
            HandleTrapCollision(other.gameObject);
        }

        private void HandleTrapCollision(GameObject collidedObject)
        {
            _collidedObject = collidedObject;

            Destroy(Rigidbody);
            Destroy(GetComponent<Collider>());

            transform.SetParent(collidedObject.transform);
            
            OnTrapCollided?.Invoke(TrapTag, collidedObject);
        }
    }
}