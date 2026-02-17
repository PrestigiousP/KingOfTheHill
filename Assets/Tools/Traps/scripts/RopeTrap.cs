using System;
using Characters.scripts.Shared;
using UnityEngine;

namespace Tools.Traps.scripts
{
    public class RopeTrap : Throwable
    { 
        [SerializeField] public float pullForce = 10f;
        
        private GameObject _firstTrap;
        private GameObject _firstTrapCollidedObject;
        private bool _isPullingFirstTrap;
        private Vector3 _firstTrapPullDirection;
        
        private GameObject _secondTrap;
        private GameObject _secondTrapCollidedObject;
        private bool _isPullingSecondTrap;
        private Vector3 _secondTrapPullDirection;
        
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            
            if (!_firstTrap)
            {
                _firstTrap = ThrowProjectile();
                var ropeTrapDetector = _firstTrap.GetComponent<RopeTrapDetector>();
                ropeTrapDetector.SetTrapTag(TrapTag.First);
                ropeTrapDetector.OnTrapCollided += HandleTrapCollision;
                return;
            }

            if (!_secondTrap)
            {
                _secondTrap = ThrowProjectile();
                var ropeTrapDetector = _secondTrap.GetComponent<RopeTrapDetector>();
                ropeTrapDetector.SetTrapTag(TrapTag.Second);
                ropeTrapDetector.OnTrapCollided += HandleTrapCollision;
            }
        }

        private void FixedUpdate()
        {
            if (_isPullingFirstTrap)
            {
                PullObject(_firstTrapCollidedObject.transform, _firstTrap.transform, _firstTrapPullDirection);
            }

            if (_isPullingSecondTrap)
            {
                PullObject(_secondTrapCollidedObject.transform, _secondTrap.transform, _secondTrapPullDirection);
            }
            
            
        }

        private void HandleTrapCollision(TrapTag trapTag, GameObject collidedObject)
        {
            switch (trapTag)
            {
                case TrapTag.First:
                    _firstTrapCollidedObject = collidedObject;
                    
                    // Remove physics components from trap
                    var firstRb = _firstTrap.GetComponent<Rigidbody>();
                    if (firstRb != null) Destroy(firstRb);
                    
                    var firstCollider = _firstTrap.GetComponent<Collider>();
                    if (firstCollider != null) Destroy(firstCollider);
                    
                    // Parent trap to collided object
                    _firstTrap.transform.SetParent(collidedObject.transform);
                    
                    break;
                case TrapTag.Second:
                {
                    _secondTrapCollidedObject = collidedObject;
                    
                    // Remove physics components from trap
                    var secondRb = _secondTrap.GetComponent<Rigidbody>();
                    if (secondRb != null) Destroy(secondRb);
                    
                    var secondCollider = _secondTrap.GetComponent<Collider>();
                    if (secondCollider != null) Destroy(secondCollider);
                    
                    // Parent trap to collided object
                    _secondTrap.transform.SetParent(collidedObject.transform);
                    
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(trapTag), trapTag, null);
            }

            if (!(_firstTrapCollidedObject && _secondTrapCollidedObject))
            {
                return;
            }
            
            var firstTrapCollidedObjRb = _firstTrapCollidedObject.GetComponent<Rigidbody>();
            var secondTrapCollidedObjRb = _secondTrapCollidedObject.GetComponent<Rigidbody>();

            if (firstTrapCollidedObjRb == null && secondTrapCollidedObjRb == null)
            {
                ResetTraps();
                return;
            }

            if (firstTrapCollidedObjRb != null)
            {
                _isPullingFirstTrap = true;
                _firstTrapPullDirection = (_secondTrapCollidedObject.transform.position 
                                           - _firstTrapCollidedObject.transform.position).normalized;
            }

            if (secondTrapCollidedObjRb != null)
            {
                _isPullingSecondTrap = true;
                _secondTrapPullDirection = (_firstTrapCollidedObject.transform.position 
                                            - _secondTrapCollidedObject.transform.position).normalized;
            }

        }

        private void PullObject(Transform objectTransform, Transform trapTransform, Vector3 direction)
        {
            objectTransform.position += direction * (pullForce * Time.deltaTime);
            // trapTransform.position += newPosition;

            var firstTrapCollider = _firstTrapCollidedObject.GetComponent<Collider>();
            var secondTrapCollider = _secondTrapCollidedObject.GetComponent<Collider>();

            if (!firstTrapCollider || !secondTrapCollider) return;
            
            if (firstTrapCollider.bounds.Intersects(secondTrapCollider.bounds))
            {
                ResetTraps();
            }
        }

        private void ResetTraps()
        {
            if (_firstTrap != null)
            {
                var detector = _firstTrap.GetComponent<RopeTrapDetector>();
                if (detector != null)
                {
                    detector.OnTrapCollided -= HandleTrapCollision;
                }
                // Unparent from collided object
                _firstTrap.transform.SetParent(null);
                Destroy(_firstTrap);
            }
            
            if (_secondTrap != null)
            {
                var detector = _secondTrap.GetComponent<RopeTrapDetector>();
                if (detector != null)
                {
                    detector.OnTrapCollided -= HandleTrapCollision;
                }
                // Unparent from collided object
                _secondTrap.transform.SetParent(null);
                Destroy(_secondTrap);
            }
            
            _firstTrap = null;
            _firstTrapCollidedObject = null;
            _isPullingFirstTrap = false;
            _firstTrapPullDirection = default;

            _secondTrap = null;
            _secondTrapCollidedObject = null;
            _isPullingSecondTrap = false;
            _secondTrapPullDirection = default;
        }

        private void OnDestroy()
        {
            // Clean up event subscriptions
            if (_firstTrap != null)
            {
                var detector = _firstTrap.GetComponent<RopeTrapDetector>();
                if (detector != null)
                {
                    detector.OnTrapCollided -= HandleTrapCollision;
                }
            }
            
            if (_secondTrap != null)
            {
                var detector = _secondTrap.GetComponent<RopeTrapDetector>();
                if (detector != null)
                {
                    detector.OnTrapCollided -= HandleTrapCollision;
                }
            }
        }
    }
}