using System;
using System.Collections.Generic;
using Mirror;
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
    
    public class PlayerSetRig : NetworkBehaviour
    {
        [SerializeField] private RigBuilder _rig;
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _aimLook;
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

        public override void OnStartClient()
        {
            if(!isOwned) return;
            // var mainCam = Camera.main;
            // Transform target = null;
            // if (mainCam != null)
            // {
            //     target = mainCam.transform.GetChild(0);
            //     if(target != null)
            //     {
            //         GameObject aimLook = Instantiate(_aimLook, target);
            //         NetworkServer.Spawn(aimLook,connectionToClient);
            //         _target = aimLook.transform;
            //     }
            // }
            var newSourceArray = new WeightedTransformArray { new WeightedTransform(_target, 1f) };
            _spineMain.data.sourceObjects = newSourceArray;
            _spineSub.data.sourceObjects = newSourceArray;
            _head.data.sourceObjects = newSourceArray;
            _weapon.data.sourceObjects = newSourceArray;
            
            _rig.Build();
        }

        private void Start()
        {
          
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