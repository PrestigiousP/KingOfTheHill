using Characters.scripts.Interfaces;
using UnityEngine;

namespace Characters.Players.scripts
{
    public class Player : MonoBehaviour, IDamageable, IKnockBack
    {
        public Vector3 Forward { get; private set; }
        
        private Rigidbody _rb;
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = transform.Find("Camera");
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            Forward = (transform.position - _cameraTransform.position).normalized;
        }

        public void GetPunched(Vector3 direction, float damage)
        {
            throw new System.NotImplementedException();
        }

        public void KnockBack(Vector3 direction, float force)
        {
            _rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
    }
}
