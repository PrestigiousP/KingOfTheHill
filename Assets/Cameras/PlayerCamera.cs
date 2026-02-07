using System;
using Characters.Player.scripts;
using UnityEngine;

namespace Cameras
{
    public class PlayerCamera : MonoBehaviour
    {
        public Transform targetTransform;
        public Vector3 offset;
    
        // Start is called before the first frame update
        private void Start()
        {
            transform.position = targetTransform.position + offset;
            transform.LookAt(targetTransform);
            Debug.Log("target pos: " + transform.position);
            Debug.Log("cam pos: " + transform.position);
        }

        // Update is called once per frame
        private void Update()
        {
            transform.position = targetTransform.position + offset;
            transform.LookAt(targetTransform);
        }
    }
}
