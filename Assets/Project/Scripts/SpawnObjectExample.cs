using System;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class SpawnObjectExample : NetworkBehaviour
    {
        [SerializeField] private Transform _prefab;

        private void Start()
        {
            if (!IsOwner) enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                TestSpawnServerRpc();
            }
        }

        [ServerRpc]
        private void TestSpawnServerRpc(bool destroyWithScene = true)
        {
            var item = Instantiate(_prefab);

            if (item.TryGetComponent(out NetworkObject networkObject))
                networkObject.Spawn(destroyWithScene);
            
            Debug.Assert(networkObject, "Prefab should have NetworkObject");
        }
    }
}