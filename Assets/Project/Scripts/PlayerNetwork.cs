using Unity.Netcode;
using UnityEngine;

namespace Project
{
	public class PlayerNetwork : NetworkBehaviour
	{
		private const int MaxInputLength = 1;
		[SerializeField] private float _speed = 10;

		private Vector2 _movementInput;

		private void Start()
		{
			if (!IsOwner) enabled = false;
		}

		private void Update()
		{
			GetInputs();
			HandleMovement();
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
	}
}
