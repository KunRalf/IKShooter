using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player
{
    [Serializable]
    public class WeaponToRig
    {
        public string Name;
        public Transform LHand;
        public Transform RHand;
    }
    
    public class PlayerSetRig : MonoBehaviour
    {
        [SerializeField] private RigBuilder _rig;
        [SerializeField] private Transform _target;
        [Header("BodyAim")] 
        [SerializeField] private MultiAimConstraint _spineMain;
        [SerializeField] private MultiAimConstraint _spineSub;
        [SerializeField] private MultiAimConstraint _head;
        [SerializeField] private MultiAimConstraint _weapon;
        [Header("Hands")]
        [SerializeField] private TwoBoneIKConstraint _lHand;
        [SerializeField] private TwoBoneIKConstraint _rHand;

        [Header("Weapons")] 
        [SerializeField] private GameObject _ak;
        [SerializeField] private GameObject _m16;
        [SerializeField] private List<WeaponToRig> _weapons;
        
        private void Start()
        {
            var newSourceArray = new WeightedTransformArray { new WeightedTransform(_target, 1f) };
            _spineMain.data.sourceObjects = newSourceArray;
            _spineSub.data.sourceObjects = newSourceArray;
            _head.data.sourceObjects = newSourceArray;
            _weapon.data.sourceObjects = newSourceArray;

            _rig.Build();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _ak.SetActive(true);
                _m16.SetActive(false);
                _lHand.data.target = _weapons[0].LHand;
                _rHand.data.target = _weapons[0].RHand;
                _rig.Build();
            } 
            if (Input.GetKeyDown(KeyCode.E))
            {
                _ak.SetActive(false);
                _m16.SetActive(true);
                _lHand.data.target = _weapons[1].LHand;
                _rHand.data.target = _weapons[1].RHand;
                _rig.Build();
            }   
            if (Input.GetKeyDown(KeyCode.R))
            {
                _ak.SetActive(false);
                _m16.SetActive(false);
                _lHand.data.target = null;
                _rHand.data.target = null;
                _rig.Build();
            }
        }
    }
}