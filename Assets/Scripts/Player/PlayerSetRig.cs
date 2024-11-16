using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;
using WeaponSys;
using Random = UnityEngine.Random;

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
        [SyncVar(hook = nameof(SetAimRig))][SerializeField] private LookAtTarget _target;
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

        public void Init(GameObject aimLook)
        {
            _target = aimLook.GetComponent<LookAtTarget>();
        }
        
        private void SetAimRig(LookAtTarget o, LookAtTarget n)
        {
            var newSourceArray = new WeightedTransformArray { new WeightedTransform(_target.transform, 1f) };
            _spineMain.data.sourceObjects = newSourceArray;
            _spineSub.data.sourceObjects = newSourceArray;
            _head.data.sourceObjects = newSourceArray;
            _weapon.data.sourceObjects = newSourceArray;
            
            _rig.Build();
        }  
        
        private void Update()
        {
            if (!isOwned) return;
            _target.UpdatePos(Camera.main);
            _target.UpdateShootingPos(Camera.main);
            // if(_target == null) return;
            // Vector2 screenPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            // Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            // float distance = 20f;
            // _target.transform.position = ray.origin + ray.direction * distance;
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