using Characters.scripts.Interfaces;
using UnityEngine;
using Utils;

namespace Characters.Player.scripts
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] public float punchRange = 0.1f;
        [SerializeField] public float punchForce = 5f;
        [SerializeField] public float punchDamage = 10f;

        private Player _player;

        private void Awake()
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
        
        private void Punch()
        {
            var hits = new RaycastHit[2];
            Physics.RaycastNonAlloc(transform.position, transform.forward, hits, punchRange);

            foreach (var hit in hits)
            {
                if (!hit.collider || !hit.collider.CompareTag(Tags.Enemy)) continue;
                
                // Try to get the interface from the hit object
                var damageable = hit.collider.GetComponent<IDamageable>();

                damageable?.GetPunched(transform.forward, punchDamage);
            }
        }
    }
}
