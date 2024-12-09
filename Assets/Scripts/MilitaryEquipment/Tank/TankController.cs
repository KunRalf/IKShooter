using System;
using Mirror;
using Player;
using UnityEngine;

namespace MilitaryEquipment.Tank
{
    public class TankController : NetworkBehaviour
    {
        [SerializeField] private TankMove _tankMove;
        [SerializeField] private TankTurretRotate _tankTurretRotate;
        [SerializeField] private Collider _colliderSit;
        [SerializeField] private Transform _releasePlace;
        private bool _isControlled;
        private bool _isPlayerReadyToSit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController player))
            {
                _isPlayerReadyToSit = true;
                player.ShowInteractText("Press F to seat");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController player))
            {
                _isPlayerReadyToSit = false;
                player.ShowInteractText();
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdTakeControl(NetworkConnectionToClient conn = null)
        {
            if (connectionToClient != null)
            {
                Debug.Log("Is busy");
                return;
            }

            conn.authenticationData = conn.identity.gameObject;

            _isControlled = true;
            _tankMove.enabled = true;
            _tankTurretRotate.enabled = true;
            NetworkServer.ReplacePlayerForConnection(conn, gameObject, ReplacePlayerOptions.Unspawn);
        }

        [Command]
        private void CmdReleaseControl()
        {
            if (connectionToClient.authenticationData is GameObject player)
            {
                player.transform.SetPositionAndRotation(_releasePlace.transform.position,_releasePlace.transform.rotation);
                _isControlled = false;
                _tankMove.enabled = false;
                _tankTurretRotate.enabled = false;
                NetworkServer.ReplacePlayerForConnection(connectionToClient, player, ReplacePlayerOptions.KeepActive);
            }
        }
        
        [ClientCallback]
        void Update()
        {
            if (_isPlayerReadyToSit && Input.GetKeyDown(KeyCode.F))
                CmdTakeControl();

            if (isOwned && Input.GetKeyDown(KeyCode.F))
                CmdReleaseControl();
        }
    }
}