using System;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerAiming : NetworkBehaviour
    {
        [SerializeField] private float _turnSpeed = 15f;
        [SerializeField] private float _aimDuration = 0.3f;
        [SerializeField] private Rig _aimRig;

        [SyncVar(hook = nameof(AimRig)),Range(0f, 1f)] private float _armsAim;
        
        private Camera _cam;
        
        private void Awake()
        {
            _cam = Camera.main;
        }

        private void FixedUpdate()
        {
            if(!isOwned) return;
            ChangeVisableCursor();
            float yawCamera = _cam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera,0), _turnSpeed * Time.fixedDeltaTime);
        }

        private void ChangeVisableCursor()
        {
            if(!isOwned) return;
            if (Input.GetKeyUp(KeyCode.M))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
            }
        }
        
        private void Update()
        {
            if(!isOwned) return;
            if (Input.GetKey(KeyCode.Mouse1))
            {
                CmdChangeAimingRig(Time.deltaTime / _aimDuration);
            }
            else
            {
                CmdChangeAimingRig(-Time.deltaTime / _aimDuration);
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                // reset weapon pos
            }
        }

        [Command]
        private void CmdChangeAimingRig(float value)
        {
            _armsAim += value;
            _armsAim = Mathf.Clamp(_armsAim, 0, 1);
        }
        
        private void AimRig(float o, float n)
        {
            _aimRig.weight = _armsAim;
        }
    }
}