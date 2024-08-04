using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class PlayerNetwork : NetworkBehaviour
    {
        private const int MaxInputLength = 1;
        [SerializeField] private float _speed = 10;
        [SerializeField] private bool _log = true;


        public NetworkVariable<int> _test = new(1, writePerm: NetworkVariableWritePermission.Owner);

        public NetworkVariable<CustomNetworkDataExample> _customData = new(new(true),
            writePerm: NetworkVariableWritePermission.Owner);

        public NetworkVariable<CustomNetworkDataAnotherExample> _anotherCustomData =
            new(new(true), writePerm: NetworkVariableWritePermission.Owner);

        private Vector2 _movementInput;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (_log)
                Debug.Log($"{OwnerClientId} Connected ({(IsOwner ? "You" : "Other Player")})");

            _test.OnValueChanged += (value, newValue) =>
            {
                if (_log)
                    Debug.Log($"{OwnerClientId}; {newValue}");
            };

            _customData.OnValueChanged += (value, newValue) =>
            {
                if (_log)
                    Debug.Log($"{OwnerClientId}; CustomData Changed; {newValue}");
            };

            _anotherCustomData.OnValueChanged += (value, newValue) =>
            {
                if (_log)
                    Debug.Log($"{OwnerClientId}; AnotherCustomData Changed; {newValue}");
            };
        }

        private void Update()
        {
            if (!IsOwner)
            {
                // enabled = false;
                return;
            }

            GetInputs();
            HandleMovement();
        }

        private void GetInputs()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                _test.Value = System.DateTime.Now.Millisecond;
                _customData.Value = new(true);
                _anotherCustomData.Value = new(true);
            }

            _movementInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
                MaxInputLength);
        }

        private void HandleMovement()
        {
            if (_movementInput == Vector2.zero) return;

            float deltaForward = _speed * Time.deltaTime * _movementInput.y;
            float deltaRight = _speed * Time.deltaTime * _movementInput.x;

            Vector3 forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
            forward *= deltaForward;

            Vector3 right = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;
            right *= deltaRight;

            Vector3 deltaMovement = forward + right;

            transform.position += deltaMovement;
        }
    }
}