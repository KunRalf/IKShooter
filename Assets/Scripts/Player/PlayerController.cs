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
        [SerializeField] private PlayerUI _playerUI;

        [SerializeField] private GameObject _camera;

        public override void OnStartClient()
        {
            _camera.SetActive(isOwned);
        }
    }
}