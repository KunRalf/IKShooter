using Mirror;
using UnityEngine;

namespace MilitaryEquipment.Tank
{
    public class TankMove : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 10f;
        [SerializeField] private float _turnSpeed = 100f;

        private void ChangeVisableCursor()
        {
            // if(!isOwned) return;
            if (Input.GetKeyUp(KeyCode.M))
            {
                Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
            }
        }
        
        private void Update()
        {
            ChangeVisableCursor();
            float moveInput = Input.GetAxis("Vertical");
            float turnInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.forward * moveInput * _moveSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up * turnInput * _turnSpeed * Time.deltaTime);
        }
    }
}