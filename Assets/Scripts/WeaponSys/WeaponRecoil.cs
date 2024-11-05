using System;
using Cinemachine;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WeaponSys
{
    public class WeaponRecoil : MonoBehaviour
    {
        private CinemachineFreeLook _cam;
        
        [SerializeField] private Vector2 _verticalRecoil;
        [SerializeField] private Vector2 _horizontalRecoil;
        [SerializeField] private float _duration;

        private float _time;

        private void Awake()
        {
            var mainCam = Camera.main;
            _cam = mainCam.gameObject.GetComponent<SceneCameras>().PlayerCamera;
        }
        
        public void GenerateRecoil()
        {
             _time = _duration;
        }

        private void Update()
        {
            if (_time > 0)
            {
                _cam.m_YAxis.Value -= ((Random.Range(-_verticalRecoil.x, _verticalRecoil.y) / 1000) * Time.deltaTime) / _duration;
                _cam.m_XAxis.Value -= ((Random.Range(-_horizontalRecoil.x, _horizontalRecoil.y) / 10) * Time.deltaTime) / _duration;
                _time -= Time.deltaTime;
            }
        }
    }
}