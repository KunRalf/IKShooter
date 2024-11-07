using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;
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
        [SyncVar(hook = nameof(SetAimRig))][SerializeField] private GameObject _target;
        [SyncVar(hook = nameof(SetAim))][SerializeField] private uint _targetId;
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
            Debug.Log(_targetId);
            base.OnStartClient();
            if (!isOwned)return;
            CmdCreateAimLook();
        }

        [Command]
        private void CmdCreateAimLook()
        {
            var aimLook = Instantiate(_aimLook);
            
            NetworkServer.Spawn(aimLook, connectionToClient);
            RPCSetTarget(aimLook.GetComponent<NetworkIdentity>().netId);
        }  
        
        private void SetAimRig(GameObject o, GameObject n)
        {
            var newSourceArray = new WeightedTransformArray { new WeightedTransform(_target.transform, 1f) };
            _spineMain.data.sourceObjects = newSourceArray;
            _spineSub.data.sourceObjects = newSourceArray;
            _head.data.sourceObjects = newSourceArray;
            _weapon.data.sourceObjects = newSourceArray;
            
            _rig.Build();
        }  
        private void SetAim(uint o, uint n)
        {
            // var newSourceArray = new WeightedTransformArray { new WeightedTransform(_target.transform, 1f) };
            // _spineMain.data.sourceObjects = newSourceArray;
            // _spineSub.data.sourceObjects = newSourceArray;
            // _head.data.sourceObjects = newSourceArray;
            // _weapon.data.sourceObjects = newSourceArray;
            //
            // _rig.Build();
       
        }

        [ClientRpc]
        private void RPCSetTarget(uint id)
        {
            _targetId =id;
        }
        
        private void Update()
        {
            if (!isOwned) return;
            if(_target == null) return;
            Vector2 screenPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            // Определяем позицию прицела в 3D-пространстве
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);

            // Определяем дистанцию для точки в 3D-пространстве, например, 10 единиц вперед
            float distance = 10f;

            // Обновляем позицию TargetPoint на этой линии
            _target.transform.position = ray.origin + ray.direction * distance;
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