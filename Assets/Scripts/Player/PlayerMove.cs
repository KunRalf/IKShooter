using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMove : NetworkBehaviour
    {
        private static readonly int INPUT_X = Animator.StringToHash("InputX");
        private static readonly int INPUT_Y = Animator.StringToHash("InputY");
        [SerializeField] private float _moveSpeed = 2f; 
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _radiusCheckGround;
        [SerializeField] private LayerMask _groundMask;
        [SyncVar(hook = nameof(SyncIsGrounded))][SerializeField]private bool _isGrounded;
        private float _gravity = 0.05f;
        private Rigidbody _rigidbody;
        [SerializeField]private Animator _animator;
        private Vector3 _direction;
        // private PlayerAnimator _playerAnimator;

       [SyncVar] private bool _isInAir;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            if(!isOwned) return;
            Move();
        }

        private void Update()
        {
            if(!isOwned) return;
            CheckGrounded();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CmdPlayerJump("Jump",_jumpForce);
            }
        }

        [Command]
        private void CheckGrounded()
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _radiusCheckGround,_groundMask);
        }
        
        private void SyncIsGrounded(bool o, bool n)
        {
            _isInAir = !n;
            _animator.SetBool("IsInAir", _isInAir);
        }
        
        private void OnAnimatorMove()
        {
            
        }

        private void Move()
        {
            if(!_isGrounded) return;
            var axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            _direction = transform.forward * axis.y + transform.right * axis.x;
            _direction.Normalize();
            _rigidbody.velocity = new Vector3(_direction.x*_moveSpeed,_rigidbody.velocity.y,_direction.z * _moveSpeed);
            
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
            _animator.SetFloat(INPUT_X, axis.x);
            _animator.SetFloat(INPUT_Y, axis.y);
        }


        [Command]
        private void CmdPlayerJump(string animTrigger, float jumpForce)
        {
            RcpJump(animTrigger,jumpForce);
        }

        [ClientRpc]
        private void RcpJump(string animTrigger, float jumpforce)
        {
            if(!_isGrounded) return;
            _animator.SetTrigger(animTrigger);
            _rigidbody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
 
            // Синхронизация анимации прыжка
            // TriggerJumpAnimation();
        }
    }
}