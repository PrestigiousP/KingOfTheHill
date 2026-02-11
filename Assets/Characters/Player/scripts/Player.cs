using UnityEngine;

namespace Characters.Player.scripts
{
    public class Player : MonoBehaviour
    {
        public Vector3 Forward { get; private set; }
        
        private Transform _cameraTransform;
        
        // Start is called before the first frame update
        private void Start()
        {
            _cameraTransform = transform.Find("Camera");
        }

        private void FixedUpdate()
        {
            Forward = (transform.position - _cameraTransform.position).normalized;
        }
    }
}
