using UnityEngine;
using System;

namespace Tools.Traps.scripts
{
    public class RopeTrapDetector : MonoBehaviour
    {
        private TrapTag _trapTag;
        private Rigidbody _rb;
        
        public event Action<TrapTag, GameObject> OnTrapCollided;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void SetTrapTag(TrapTag trapTag)
        {
            _trapTag = trapTag;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (_rb == null) return;

            _rb.constraints = RigidbodyConstraints.FreezeAll;
            
            OnTrapCollided?.Invoke(_trapTag, other.gameObject);
        }
    }
}