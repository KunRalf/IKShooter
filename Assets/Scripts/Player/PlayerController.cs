using Mirror;
using UnityEngine;

namespace Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerMove _playerMove;
        [SerializeField] private PlayerWeaponSystem _playerWeaponSystem;
        [SerializeField] private PlayerAiming _playerAiming;
        [SerializeField] private PlayerSetRig _playerSetRig;
        [SerializeField] private PlayerUI _playerUI;
        [SyncVar] [SerializeField] private GameObject _aimLook;
        [SerializeField] private GameObject _camera;

        public override void OnStartClient()
        {
            _camera.SetActive(isOwned);
        }

        public void Init(GameObject aimLook)
        {
            _aimLook = aimLook;
            _playerSetRig.Init(_aimLook);
        }
    }
}