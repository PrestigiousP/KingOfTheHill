using UnityEngine;

namespace Characters.Player.scripts
{
    public class Movement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
    
        [Header("Mouse Look Settings")]
        public float mouseSensitivity = 2f;
        public Transform cameraTransform;
        
        private float _xRotation = 0f;

        private void Start()
        {
            // Lock and hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            // HandleMouseLook();
        }


        private void HandleMovement()
        {
            // Get WASD input
            float horizontal = Input.GetAxis("Horizontal"); // A and D
            float vertical = Input.GetAxis("Vertical");     // W and S
        
            // Calculate movement direction relative to where the player is facing
            var movement = transform.right * horizontal + transform.forward * vertical;
        
            // Move the player
            transform.position += movement * (moveSpeed * Time.deltaTime);
        }

        private void HandleMouseLook()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
            // Rotate the camera up and down
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); // Limit looking up/down
        
            if (cameraTransform)
            {
                cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            }
        
            // Rotate the player left and right
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
