using System;
using Characters.Players.scripts;
using Classes.Interfaces;
using Classes.Players;
using UnityEngine;

namespace Tools.Traps.scripts
{
    public class RopeTrap : MonoBehaviour
    {
        public float pullForce = 10f;
        public GameObject projectilePrefab;
        public Transform throwPoint;
        public float throwForce = 50f;
        public float throwAngleY = 0.75f;

        private RopeTrapDetector _firstTrap;
        private GameObject _firstTrapCollidedObject;
        private Collider _firstTrapCollidedObjectCollider;
        private bool _isFirstCollidedObjectPlayer;
        private Player _firstCollidedPlayer;
        private bool _isPullingFirstTrap;
        private Vector3 _firstTrapPullDirection;

        private RopeTrapDetector _secondTrap;
        private GameObject _secondTrapCollidedObject;
        private Collider _secondTrapCollidedObjectCollider;
        private bool _isSecondCollidedObjectPlayer;
        private Player _secondCollidedPlayer;
        private bool _isPullingSecondTrap;
        private Vector3 _secondTrapPullDirection;
        
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            if (!_firstTrap)
            {
                _firstTrap = ThrowProjectile(TrapTag.First);
                return;
            }

            if (!_secondTrap)
            {
                _secondTrap = ThrowProjectile(TrapTag.Second);
            }
        }

        private void FixedUpdate()
        {
            if (_isPullingFirstTrap)
            {
                // _firstCollidedPlayer.PlayerState = _isFirstCollidedObjectPlayer 
                //     ? PlayerState.Stunned 
                //     : _firstCollidedPlayer.PlayerState;

                PullObject(_firstTrapCollidedObject.transform, _firstTrapPullDirection);
            }

            if (_isPullingSecondTrap)
            {
                // _secondCollidedPlayer.PlayerState = _isSecondCollidedObjectPlayer 
                //     ? PlayerState.Stunned 
                //     : _secondCollidedPlayer.PlayerState;
                
                PullObject(_secondTrapCollidedObject.transform, _secondTrapPullDirection);
            }
        }

        private void OnTrapCollided(TrapTag trapTag, GameObject collidedObject)
        {
            switch (trapTag)
            {
                case TrapTag.First:
                    _firstTrapCollidedObject = collidedObject;
                    _isFirstCollidedObjectPlayer = _firstTrapCollidedObject.TryGetComponent<Player>(out var firstCollidedPlayer);
                    _firstCollidedPlayer = firstCollidedPlayer;
                    Debug.Log("first trap collided with " + collidedObject.name);
                    break;
                case TrapTag.Second:
                    _secondTrapCollidedObject = collidedObject;
                    _isSecondCollidedObjectPlayer = _secondTrapCollidedObject.TryGetComponent<Player>(out var secondCollidedPlayer);
                    _secondCollidedPlayer = secondCollidedPlayer;
                    Debug.Log("second trap collided with " + collidedObject.name);
                    break;
            }

            bool haveBothTrapsCollided = _firstTrapCollidedObject != null && _secondTrapCollidedObject != null;
            if (!haveBothTrapsCollided) return;
            
            var firstTrapCollidedObjRb = _firstTrapCollidedObject.GetComponent<Rigidbody>();
            var secondTrapCollidedObjRb = _secondTrapCollidedObject.GetComponent<Rigidbody>();
                
            if (firstTrapCollidedObjRb == null && secondTrapCollidedObjRb == null)
            {
                ResetTraps();
                return;
            }
                
            if (firstTrapCollidedObjRb)
            {
                _isPullingFirstTrap = true;
                _firstTrapPullDirection = (_secondTrapCollidedObject.transform.position
                                           - _firstTrapCollidedObject.transform.position).normalized;
            }

            if (!secondTrapCollidedObjRb) return;
            
            _isPullingSecondTrap = true;
            _secondTrapPullDirection = (_firstTrapCollidedObject.transform.position
                                        - _secondTrapCollidedObject.transform.position).normalized;
        }
        
        private void PullObject(Transform objectTransform, Vector3 direction)
        {
            objectTransform.position += direction * (pullForce * Time.deltaTime);

            if (!_firstTrapCollidedObjectCollider)
            {
                _firstTrapCollidedObjectCollider = _firstTrapCollidedObject.GetComponent<Collider>();
            }

            if (!_secondTrapCollidedObjectCollider)
            {
                _secondTrapCollidedObjectCollider = _secondTrapCollidedObject.GetComponent<Collider>();
            }
            
            if (!_firstTrapCollidedObjectCollider || !_secondTrapCollidedObjectCollider) return;

            if (_firstTrapCollidedObjectCollider.bounds.Intersects(_secondTrapCollidedObjectCollider.bounds))
            {
                ResetTraps();
            }
        }
        
        private void ResetTraps()
        {
            if (_firstTrap) Destroy(_firstTrap.gameObject);
            _firstTrapCollidedObject = null;
            _isPullingFirstTrap = false;
            _firstTrapPullDirection = default;
            
            if (_secondTrap) Destroy(_secondTrap.gameObject);
            _secondTrapCollidedObject = null;
            _isPullingSecondTrap = false;
            _secondTrapPullDirection = default;
        }

        private RopeTrapDetector ThrowProjectile(TrapTag trapTag)
        {
            var projectile = Instantiate(
                projectilePrefab,
                throwPoint.position,
                throwPoint.rotation
            );

            var projectileType = projectile.GetComponent<RopeTrapDetector>();
            projectileType.TrapTag = trapTag;
            projectileType.OnTrapCollided += OnTrapCollided;
            
            var direction = (_player.Forward + new Vector3(0, _player.Forward.y + throwAngleY, 0)).normalized;
            projectileType.Rigidbody.AddForce(direction * throwForce, ForceMode.Impulse);

            return projectileType;
        }
    }
}