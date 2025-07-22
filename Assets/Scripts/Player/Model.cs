using System;
using Controller;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Model : MonoBehaviour
    {
        //Components
        private Rigidbody _rigidbody;
        private InteractComponent _interactComponent;

        [Header("Movement Variables")] 
        [SerializeField] private float speed;
        private Vector3 _actualVelocity;
        private bool _canMove = true;

        [Header("Jump Variables")] [SerializeField]
        private float _jumpForce;

        [SerializeField] private float _jumpDelay = .2f;
        [SerializeField] private Vector3 _feetOffset;
        private Vector3 FeatPosition => transform.position + _feetOffset;
        private bool _recentlyJumped = false;
        private float _jumpTimer;
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _interactComponent = GetComponent<InteractComponent>();

            ControllerManager.AddPerformanceEvent(InputNames.Move, OnMove);
            ControllerManager.AddCancelEvent(InputNames.Move, OnMove);
            ControllerManager.AddPerformanceEvent(InputNames.Jump, OnJump);
            ControllerManager.AddPerformanceEvent(InputNames.Interact, OnInteract);
            
            EventManager.Subscribe(EventName.OnPlayerResetMovement, OnPlayerResetMovement);
        }

        private void Update()
        {
            if (!_canMove) return;
            
            UpdateJumpDelay();
            
            _actualVelocity.y = _rigidbody.linearVelocity.y;
            _rigidbody.linearVelocity = _actualVelocity;
        }

        #region Input Callbacks

        private void OnMove(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<Vector2>();
            dir.Normalize();
            dir *= speed;

            _actualVelocity = new Vector3(dir.x, _rigidbody.linearVelocity.y, dir.y);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (!_canMove || !IsGrounded()) return;

            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _recentlyJumped = true;
            _jumpTimer = 0;
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (_interactComponent.TryInteract())
            {
                _canMove = false;
            }
        }

        #endregion

        private bool IsGrounded()
        {
            if (_recentlyJumped) return false;

            return Physics.Raycast(FeatPosition, Vector3.down, 0.2f,
                LayerMask.GetMask("Floor"));
        }

        private void UpdateJumpDelay()
        {
            if (!_recentlyJumped) return;

            _jumpTimer += Time.deltaTime;

            if (_jumpTimer >= _jumpDelay)
            {
                _recentlyJumped = false;
            }
        }

        private void OnPlayerResetMovement(params object[] parameters)
        {
            _canMove = true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(FeatPosition, 0.2f);
        }
#endif
    }
}