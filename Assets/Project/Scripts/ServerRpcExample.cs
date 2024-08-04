using System;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class ServerRpcExample : NetworkBehaviour
    {
        private void Start()
        {
            Debug.Log($"{OwnerClientId}; IsServer? {IsServer}; IsClient? {IsClient}; IsOwner? {IsOwner}");
        }

        private void Update()
        {
            if (!IsOwner) return;
            if (Input.GetKeyDown(KeyCode.T))
            {
                // if (!IsServer)
                // {
                //     Debug.Log($"{OwnerClientId}; CANNOT call Server Rpc (game instance is not the server)");
                //     enabled = false;
                //     return;
                // }
                
                // if (IsOwnedByServer)
                // {
                //     if (IsServer)
                //         Debug.Log($"{OwnerClientId}; It is the Server");
                //     else
                //         Debug.Log($"{OwnerClientId}; It is NOT the Server");
                // }
                
                // if (!IsOwner)
                // {
                //     Debug.Log($"{OwnerClientId}; It CANNOT use Server Rpc");
                //     return;
                // }
                //
                // Debug.Log($"{OwnerClientId}; It can use ServerRpc");

                TestServerRpc(IsServer ? "Custom message from server" : "Custom message from client");
                TestServerRpc(new ServerRpcParams());
            }
        }

        [ServerRpc]
        private void TestServerRpc(string message)
        {
            Debug.Log($"{OwnerClientId}; Server Rpc running in {(IsServer ? "server": "client")}; {message}");
        }

        [ServerRpc]
        private void TestServerRpc(ServerRpcParams rpcParams)
        {
            Debug.Log($"{OwnerClientId}; Server Rpc; Sender Id {rpcParams.Receive.SenderClientId}");
        }
    }
}
