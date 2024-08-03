using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
	[System.Obsolete("Please use the new PlayerNetwork")]
	public class ObsoletePlayerNetwork : NetworkBehaviour
	{

		private const int MaxInputLength = 1;
		[SerializeField] private float _speed = 10;

		// NOTE: It can only be of value type (cannot use reference type)
		//private NetworkVariable<int> _randomNumber = new(1, writePerm: NetworkVariableWritePermission.Owner);
		private NetworkVariable<CustomData> _randomNumber = new(new()
		{
			Int = 1,
			Bool = true,
		}, writePerm: NetworkVariableWritePermission.Owner);

		private Vector2 _movementInput;

		private struct CustomData : INetworkSerializable
		{
			public int Int;
			public bool Bool;
			public FixedString128Bytes Message;

			public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
			{
				serializer.SerializeValue(ref Int);
				serializer.SerializeValue(ref Bool);
				serializer.SerializeValue(ref Message);
			}
		}

		// NOTE: ANY NETWORK OBJECT SHOULD NEVER USE START OR AWAKE. YOU NEED TO USE THIS METHOD BELLOW
		public override void OnNetworkSpawn()
		{
			base.OnNetworkSpawn();
			_randomNumber.OnValueChanged += RandomNumber_OnValueChanged;
		}

		private void RandomNumber_OnValueChanged(int previousValue, int newValue)
		{
			Debug.Log($"{OwnerClientId}; randomNumber: {newValue}");
		}

		private void RandomNumber_OnValueChanged(CustomData previousValue, CustomData newValue)
		{
			Debug.Log($"{OwnerClientId}; randomNumber: {newValue.Int}; {newValue.Bool}; {newValue.Message}");
		}

		private void Update()
		{
			//if (!IsOwner) enabled = false;
			if (!IsOwner) return;

			GetInputs();
			HandleMovement();

			if (Input.GetKeyDown(KeyCode.T))
			{
				//_randomNumber.Value = Random.Range(0, 100);
				//_randomNumber.Value = new()
				//{
				//	Int = Random.Range(0, 100),
				//	Bool = !_randomNumber.Value.Bool,
				//	Message = System.DateTime.Now.ToString(),
				//};
				TestServerRpc("TEST");
			}

		}

		private void GetInputs()
		{
			_movementInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), MaxInputLength);
		}

		private void HandleMovement()
		{
			if (_movementInput == Vector2.zero) return;

			//Vector3 deltaSpeed = _speed * Time.deltaTime * new Vector3(_movementInput.y, 0, _movementInput.x);
			float deltaForward = _speed * Time.deltaTime * _movementInput.y;
			float deltaRight = _speed * Time.deltaTime * _movementInput.x;

			Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
			forward *= deltaForward;

			Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;
			right *= deltaRight;

			Vector3 deltaMovement = forward + right;

			transform.position += deltaMovement;
		}

		// Requirements:
		// - ServerRpc method must end with 'ServerRpc' suffix;
		// - Must have the ServerRpcAttribute;
		// - Must be inside an NetworkBehaviour class;
		// - This NetworkBehaviour class must be inside a NetworkObject hierarchy.
		// ----------------------
		// Explanation:
		// - ServerRpc methods cannot run on the client, but the client can ask
		// the server to run it on it's behalf.
		// eg: When the client tries to run the Rpc method, the system interrupts it and asks
		//     the server to run it when it can. So the server always runs it, even if it was
		//     called by the client.
		// ----------------------
		// Notes:
		// - It can only pass value type parameters;
		// - For some reason, it parameters also work with strings.
		[ServerRpc]
		private void TestServerRpc(string message)
		{
			Debug.Log($"ServerRpc: {OwnerClientId}");
		}
	}
}
