using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMove : NetworkBehaviour
    {
        [SerializeField] private float _moveSpeed = 2f; 
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _radiusCheckGround;
        [SerializeField] private LayerMask _groundMask;
        private bool _isGrounded => Physics.CheckSphere(_groundCheck.position, _radiusCheckGround,_groundMask);
        private float _gravity = 0.05f;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private Vector3 _direction;
        // private PlayerAnimator _playerAnimator;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }
        
        private void FixedUpdate()
        {
            if(!isOwned) return;
            Move();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void OnAnimatorMove()
        {
            
        }

        private void Move()
        {
            var axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _direction = transform.forward * axis.y + transform.right * axis.x;
            _direction.Normalize();
            _rigidbody.velocity = _direction * _moveSpeed;
            UpdateMoveAnim(axis);
        }

        [Command]
        private void UpdateMoveAnim(Vector2 axis)
        {
            RpcUpdateMoveAnim(axis);
        }

        [ClientRpc]
        private void RpcUpdateMoveAnim(Vector2 axis)
        {
            if(_animator==null) return;
            _animator.SetFloat("Velocity", axis.magnitude);
        }


        private void Jump()
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            // Синхронизация анимации прыжка
            // TriggerJumpAnimation();
        }
    }
}