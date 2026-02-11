using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Characters.Player.scripts
{
    public class Attack : MonoBehaviour
    {
        public float punchRange = 0.1f;
        public float punchRadius = 0.05f;
        public float punchForce = 5f;
        
        private IList<RaycastHit2D> _hits;
        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Punch();
            }
        }
        
        // This shows the sphere cast range in the editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
    
            var origin = transform.position;
            var endPos = origin + transform.forward * punchRange;
    
            // Gizmos.DrawWireSphere(origin, punchRadius);
            // Gizmos.DrawWireSphere(endPos, punchRadius);
            Gizmos.DrawLine(origin, endPos);
        }
        
        private void Punch()
        {
            var hits = new RaycastHit[2];
            Physics.RaycastNonAlloc(transform.position, transform.forward, hits, punchRange);

            foreach (var hit in hits)
            {
                if (hit.collider && hit.collider.CompareTag(Tags.Enemy))
                {
                    // TODO: fix later
                    // Push enemy
                    var rb = hit.collider.GetComponent<Rigidbody>();
                    if (rb)
                    {
                        rb.AddForce(transform.forward * punchForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
