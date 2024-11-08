using Mirror;
using UnityEngine;

namespace Network
{
    public class StartGame : MonoBehaviour
    {
        private NetworkController _networkController => NetworkManager.singleton as NetworkController;

        public void StartGame1()
        {
            _networkController.StartGame();
        }
    }
}