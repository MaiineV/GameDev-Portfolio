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

        [Header("Movement Variables")] [SerializeField]
        private float _speed;
        private Vector3 _actualVelocity;

        [Header("Jump Variables")] [SerializeField]
        private float _jumpForce;

        [SerializeField] private float _jumpDelay = .2f;
        [SerializeField] private Vector3 _feetOffset;
        private Vector3 _featPosition => transform.position + _feetOffset;
        private bool _recentlyJumped = false;
        private float _jumpTimer;
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _interactComponent = new InteractComponent();

            ControllerManager.AddPerformanceEvent(InputType.Move, OnMove);
            ControllerManager.AddCancelEvent(InputType.Move, OnMove);
            ControllerManager.AddPerformanceEvent(InputType.Jump, OnJump);
        }

        private void Update()
        {
            UpdateJumpDelay();
            
            _actualVelocity.y = _rigidbody.linearVelocity.y;
            _rigidbody.linearVelocity = _actualVelocity;
        }

        #region Input Callbacks

        private void OnMove(InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<Vector2>();
            dir.Normalize();
            dir *= _speed;

            _actualVelocity = new Vector3(dir.x, _rigidbody.linearVelocity.y, dir.y);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (!IsGrounded()) return;

            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _recentlyJumped = true;
            _jumpTimer = 0;
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            _interactComponent.TryInteract();
        }

        #endregion

        private bool IsGrounded()
        {
            if (_recentlyJumped) return false;

            return Physics.Raycast(_featPosition, Vector3.down, 0.2f,
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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_featPosition, 0.2f);
        }
#endif
    }
}