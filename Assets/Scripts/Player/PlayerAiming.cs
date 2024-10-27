using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerAiming : MonoBehaviour
    {
        [SerializeField] private float _turnSpeed = 15f;
        [SerializeField] private float _aimDuration = 0.3f;
        [SerializeField] private Rig _aimRig;
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void FixedUpdate()
        {
            float yawCamera = _cam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera,0), _turnSpeed * Time.fixedDeltaTime);
        }

        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                _aimRig.weight += Time.deltaTime / _aimDuration;
            }
            else
            {
                _aimRig.weight -= Time.deltaTime / _aimDuration;
            }
        }
    }
}