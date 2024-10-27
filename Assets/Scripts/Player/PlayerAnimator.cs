using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int InputX = Animator.StringToHash("InputX");
        private static readonly int InputY = Animator.StringToHash("InputY");
        private Animator _animator;
        private Vector2 _axis;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            // _axis.x = Input.GetAxis("Horizontal");
            // _axis.y = Input.GetAxis("Vertical");
            //
            // _animator.SetFloat(InputX, _axis.x);
            // _animator.SetFloat(InputY, _axis.y);
        }
    }
}