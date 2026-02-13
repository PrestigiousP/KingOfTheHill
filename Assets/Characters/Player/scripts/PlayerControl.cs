using UnityEngine;

namespace Characters.Player.scripts
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] public float moveSpeed = 5f;
        [SerializeField]  public float mouseSensitivity = 2f;
        [SerializeField] public float jumpForce = 10f;

        private Camera _camera;
        private Rigidbody _rigidbody;
        private float _verticalAngle;
        private float _horizontalAngle;
        private Vector3 _cameraPositionRelatedToPlayer;
        
        private void Awake()
        {
            _verticalAngle = 0f;
            _horizontalAngle = 0f;
            
            // Lock and hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _rigidbody = GetComponent<Rigidbody>();
            _camera = GetComponentInChildren<Camera>();
            _cameraPositionRelatedToPlayer = _camera.transform.position - transform.position;
        }

        private void Update()
        {
            HandleMovement();
            HandleMouseLook();
        }

        private bool IsGrounded()
        {
            const float rayDistance = 1.1f; // Slightly longer than player height/collider

            if (!Physics.Raycast(transform.position, Vector3.down, out var hit, rayDistance)) return false;
            
            return hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground");
        }

        private void HandleMovement()
        {
            // Get WASD input
            float horizontal = Input.GetAxis("Horizontal"); // A and D
            float vertical = Input.GetAxis("Vertical");     // W and S
            
            if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            
            var direction = transform.position - _camera.transform.position;
            var playerDirection = new Vector3(direction.x, 0, direction.z).normalized;
            
            var rightDirection = Quaternion.Euler(0, 90f, 0) * playerDirection;
            
            // Calculate movement direction relative to where the player is facing
            var movement = rightDirection * horizontal + playerDirection * vertical;
            
            // Move the player
            transform.position += movement * (moveSpeed * Time.deltaTime);
        }

        private void HandleMouseLook()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
            // Update angles
            _horizontalAngle += mouseX;
            _verticalAngle -= mouseY;
            _verticalAngle = Mathf.Clamp(_verticalAngle, -89f, 89f); // Limit looking up/down
        
            // Rotate player horizontally
            transform.rotation = Quaternion.Euler(0, _horizontalAngle, 0);
        
            // Calculate camera orbit position
            var rotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0);
            var newCameraPosition = transform.position + rotation * Vector3.back * _cameraPositionRelatedToPlayer.magnitude;
        
            // Update camera position and make it look at player
            _camera.transform.position = newCameraPosition;
            _camera.transform.LookAt(transform.position);
        }
    }
}
