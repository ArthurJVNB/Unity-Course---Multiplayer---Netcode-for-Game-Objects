using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
	public class NetworkManagerUI : MonoBehaviour
	{
		[SerializeField] private Button _buttonServer;
		[SerializeField] private Button _buttonHost;
		[SerializeField] private Button _buttonClient;

		private void Awake()
		{
			_buttonServer.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
			_buttonHost.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
			_buttonClient.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
		}
	}
}
