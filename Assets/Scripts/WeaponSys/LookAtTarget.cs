using System;
using Mirror;
using UnityEngine;

namespace WeaponSys
{
    public class LookAtTarget : NetworkBehaviour
    {
        private Camera _cam;
        private Ray _ray;
        private RaycastHit _htiInfo;

        public Vector3 CrossHairPos { get; private set; }
        private float _distance = 20f;
        
        public void UpdateShootingPos(Camera cam)
        {
            _ray.origin = cam.transform.position;
            _ray.direction = cam.transform.forward;
            Physics.Raycast(_ray, out _htiInfo);
            if (_htiInfo.point == Vector3.zero)
            {
                _ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                CrossHairPos = _ray.origin + _ray.direction * _distance;;
            }
            else
            {
                CrossHairPos = _htiInfo.point;
            }
        }

        public void UpdatePos(Camera cam)
        {
            Vector2 screenPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = cam.ScreenPointToRay(screenPoint);
            transform.position = ray.origin + ray.direction * _distance;
        }
    }
}