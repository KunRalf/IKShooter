using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private PlayerHealthBar _playerHealthBar;
        [SyncVar] [SerializeField] private GameObject _aimLook;
        [SerializeField] private GameObject _camera;
        [SerializeField] private PlayerHud _hudPrefab;
        
        private PlayerHud _hud;

        public override void OnStartClient()
        {
            _camera.SetActive(isOwned);
            if(isOwned)
            {
                _hud = Instantiate(_hudPrefab);
                _playerHealth.OnHealthChangedEvent += _hud.UpdateHealth;
                _playerHealth.OnArmorChangedEvent += _hud.UpdateArmor;
                _playerWeaponSystem.OnUpdateAmmo += _hud.UpdateAmmo;
            }
        
        }

        public void Init(GameObject aimLook)
        {
            _aimLook = aimLook;
            _playerSetRig.Init(_aimLook);
            _playerWeaponSystem.weaponExample.Init(aimLook);
            _playerHealth.SetHeroStats(100,100);
           
        }
    }
}