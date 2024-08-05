using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class SpawnObjectExample : NetworkBehaviour
    {
        [SerializeField] private Transform _prefab;

        private Queue<Transform> _spawnedObjects;

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

            if (Input.GetKeyDown(KeyCode.G))
            {
                TestDespawnServerRpc();
            }
        }

        /// <summary>
        /// Despawns spawned objects. OBS: If called directly by the client, it won't work, because the client does not know which objects were spawned.
        /// </summary>
        private void TestDespawn()
        {
            if (_spawnedObjects == null || _spawnedObjects.Count == 0) return;

            Transform item = _spawnedObjects.Dequeue();
            if (item.TryGetComponent(out NetworkObject networkObject))
                networkObject.Despawn();

            Debug.Assert(networkObject, "Despawned object should have NetworkObject");
        }

        [ServerRpc]
        private void TestDespawnServerRpc()
        {
            TestDespawn();
        }

        [ServerRpc]
        private void TestSpawnServerRpc(bool destroyWithScene = true)
        {
            var item = Instantiate(_prefab);

            if (item.TryGetComponent(out NetworkObject networkObject))
            {
                // networkObject.Spawn(destroyWithScene);
                networkObject.SpawnWithOwnership(OwnerClientId, destroyWithScene);
            }

            _spawnedObjects ??= new();
            _spawnedObjects.Enqueue(item);

            Debug.Assert(networkObject, "Prefab should have NetworkObject");
        }
    }
}