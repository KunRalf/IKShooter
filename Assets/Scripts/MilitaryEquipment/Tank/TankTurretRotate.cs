using System;
using UnityEngine;

namespace MilitaryEquipment.Tank
{
    public class TankTurretRotate : MonoBehaviour
    {
        [SerializeField] private Transform _turret; 
        [SerializeField] private Transform _cannon;
        
        [SerializeField] private float _turretTurnSpeed = 5f; 
        [SerializeField] private float _cannonPitchSpeed = 2f; 
        [SerializeField] private float _cannonMinAngle = -10f; 
        [SerializeField] private float _cannonMaxAngle = 30f; 

         
        [SerializeField] private Transform _cameraFollowTarget;
        [SerializeField] private Transform _cameraPivot; 
        [SerializeField] private float _cameraDistance = 10f; 
        [SerializeField] private float _cameraRotationSpeed = 3f; 
        [SerializeField] private float _cameraVerticalMin = -20f; 
        [SerializeField] private float _cameraVerticalMax = 60f; 
        
        private Vector3 _cameraOffset;
        private float _verticalAngle = 0f; 
        private float _horizontalAngle = 0f;

        private void Start()
        {
            _cameraOffset = new Vector3(0, 0, -_cameraDistance);
        }

        private void Update()
        {
            RotateTurret();
            RotateCannon();
        }

        private void LateUpdate()
        {
            ControlCamera();
        }

        private void RotateTurret()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPosition = hit.point;
                Vector3 direction = targetPosition - _turret.position;
                direction.y = 0; 
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                _turret.rotation = Quaternion.Slerp(_turret.rotation, targetRotation, Time.deltaTime * _turretTurnSpeed);
            }
        }

        private void RotateCannon()
        {
            float mouseY = Input.GetAxis("Mouse Y");
            float currentAngle = _cannon.localEulerAngles.x;
            
            if (currentAngle > 180f) currentAngle -= 360f;
            
            float newAngle = Mathf.Clamp(currentAngle - mouseY * _cannonPitchSpeed, _cannonMinAngle, _cannonMaxAngle);
            
            _cannon.localEulerAngles = new Vector3(newAngle, 0, 0);
        }
        
        private void ControlCamera()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            _horizontalAngle += mouseX * _cameraRotationSpeed;
            _verticalAngle -= mouseY * _cameraRotationSpeed;
            _verticalAngle = Mathf.Clamp(_verticalAngle, _cameraVerticalMin, _cameraVerticalMax);
            
            Quaternion rotation = Quaternion.Euler(_verticalAngle, _horizontalAngle, 0);
            Camera.main.transform.position = _cameraFollowTarget.position + rotation * _cameraOffset;
            
            Camera.main.transform.LookAt(_cameraFollowTarget);
        }
    }
}