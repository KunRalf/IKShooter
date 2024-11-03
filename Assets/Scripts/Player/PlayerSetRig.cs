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
        [SerializeField] private Transform _lHand;
        [SerializeField] private Transform _rHand;
        // [SerializeField] private TwoBoneIKConstraint _lHand;
        // [SerializeField] private TwoBoneIKConstraint _rHand;

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
        

        public void SetHandsOnWeapon(Transform lHand, Transform rHand)
        {
            _lHand.position = lHand.position;
            _lHand.rotation = lHand.rotation;
            _rHand.position = rHand.position;
            _rHand.rotation = rHand.rotation;
        }
    }
}