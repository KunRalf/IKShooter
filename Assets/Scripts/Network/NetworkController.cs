using System;
using System.Collections.Generic;
using System.Linq;

using Mirror;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Network
{
    public class NetworkController : NetworkRoomManager
    {
        public static event Action<bool> ServerConnect;
        [SerializeField] private int CountPlayersToStart;
       // [SerializeField] private NetworkUI _networkUI;
       [SerializeField] private GameObject _aimLook;

      //  private static List<MyStartPoint> _myStartPositions = new List<MyStartPoint>();
        // [SerializeField] private PlayerInfo _playerInfo;
        // [SerializeField] private GamePlayerInfo _gamePlayerInfo;
        
        private void OnEnable()
        {
            // _networkUI.AddListenerToHostButton(StartHost);
            // _networkUI.AddListenerToJoinButton(StartClient);
            // _networkUI.AddListenerToLeaveButton(LeaveFromServer);
        }

        private void OnDisable()
        {
            // _networkUI.RemoveListeners();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            // UserStore.SetCountToStartGame(CountPlayersToStart);
        }
        
        public override void OnClientConnect()
        {
            base.OnClientConnect();
            ServerConnect?.Invoke(true);
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            ServerConnect?.Invoke(false);
        }


        // public override Transform GetStartPosition()
        // {
        //     // first remove any dead transforms
        //     _myStartPositions.RemoveAll(t => t.transform == null);
        //
        //     if (_myStartPositions.Count == 0)
        //         return null;
        //
        //     if (playerSpawnMethod == PlayerSpawnMethod.Random)
        //     {
        //         return _myStartPositions[UnityEngine.Random.Range(0, _myStartPositions.Count)].transform;
        //     }
        //     else
        //     {
        //         Transform startPosition = _myStartPositions[startPositionIndex].transform;
        //         startPositionIndex = (startPositionIndex + 1) % _myStartPositions.Count;
        //         return startPosition;
        //     }
        // }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            if (conn != null && conn.identity != null)
            {
                GameObject roomPlayer = conn.identity.gameObject;
                if (roomPlayer != null && roomPlayer.GetComponent<NetworkRoomPlayer>() != null)
                    SceneLoadedForPlayer(conn, roomPlayer);
            }
            base.OnServerReady(conn);
        }
        
        private void SceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            //Debug.Log($"NetworkRoom SceneLoadedForPlayer scene: {SceneManager.GetActiveScene().path} {conn}");

            if (Utils.IsSceneActive(RoomScene))
            {
                // cant be ready in room, add to ready list
                PendingPlayer pending;
                pending.conn = conn;
                pending.roomPlayer = roomPlayer;
                pendingPlayers.Add(pending);
                return;
            }

            var playerInfo = roomPlayer.GetComponent<PlayerInfo>();
            GameObject gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);
            if (gamePlayer == null)
            {
                // get start position from base class
                // Transform startPos = GetStartPosition();
                // MyStartPoint point = startPos.GetComponent<MyStartPoint>();
               
                gamePlayer =  Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                var aimLook = Instantiate(_aimLook);
                NetworkServer.Spawn(aimLook,conn);
                gamePlayer.GetComponent<PlayerController>().Init(aimLook);
            }

            if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
                return;

            // replace room player with game player
            NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, ReplacePlayerOptions.KeepAuthority);
        }
        
        // public static void RegisterMyStartPosition(MyStartPoint start)
        // {
        //     _myStartPositions.Add(start);
        //     _myStartPositions = _myStartPositions.OrderBy(_ => _.Index).ToList();
        // }
        // public static void UnRegisterMyStartPosition(MyStartPoint start)
        // {
        //     _myStartPositions.Remove(start);
        // }
        public void StartGame()
        {
            // foreach (var player in UserStore.GetPlayers())        
            // {
            //     if(!player.IsReady)
            //         player.connectionToClient.Disconnect();
            // }
            if (SceneManager.GetActiveScene().name == "Lobby")
            {
                ServerChangeScene("GameScene");
            }
        }
        
        private void LeaveFromServer()
        {
            
            if (NetworkServer.active && NetworkClient.active)
            {
                StopHost();
            }
            else
            {
                StopClient();
            }
        }
        
    }
}