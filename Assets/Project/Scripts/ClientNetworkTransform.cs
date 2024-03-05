using Unity.Netcode.Components;
using UnityEngine;

namespace Project
{
	[DisallowMultipleComponent]
	public class ClientNetworkTransform : NetworkTransform
	{
		protected override bool OnIsServerAuthoritative()
		{
			return false;
		}
	}
}
