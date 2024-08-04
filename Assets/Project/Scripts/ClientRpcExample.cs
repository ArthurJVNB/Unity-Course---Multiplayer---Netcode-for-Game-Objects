using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class ClientRpcExample : NetworkBehaviour
    {
        // private void Start()
        // {
        //     Debug.Log($"{OwnerClientId}; IsServer? {IsServer}; IsClient? {IsClient}; IsOwner? {IsOwner}");
        // }

        private void Update()
        {
            if (!IsOwner) return;
            if (Input.GetKeyDown(KeyCode.T))
            {
                TestClientRpc($"Custom message from {(IsServer ? "server" : "client")}");
                TestClientRpc($"Custom message from {(IsServer ? "server" : "client")}", new ClientRpcParams() { Send = new() { TargetClientIds = new List<ulong> { 1 } } });
            }
        }

        [ClientRpc]
        private void TestClientRpc(string message)
        {
            Debug.Log($"Client Rpc sent to any client; {message}");
        }

        [ClientRpc]
        private void TestClientRpc(string message, ClientRpcParams rpcParams)
        {
            Debug.Log($"Client Rpc to specific client (you are one of them); {message}");
        }
    }
}