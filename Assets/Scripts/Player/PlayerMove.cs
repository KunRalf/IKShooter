using System;
using UnityEngine;

namespace Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 2f; // Скорость движения игрока
        [SerializeField] private float _rotateSpeed = 0.1f; // Скорость движения игрока

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
            // _playerAnimator = GetComponent<PlayerAnimator>();
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            // для тестовой сцены
            //  _camera.GetComponent<MainCameraScript>().InitTestScene(this.transform);
            
        }
        
        private void FixedUpdate()
        {
            Move();
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
            _animator.SetFloat("Velocity", _rigidbody.velocity.magnitude/_moveSpeed);
        }
    }
}