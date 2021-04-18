using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

/// <summary>
/// Projectile behavior
/// </summary>

[RequireComponent (typeof(Rigidbody2D))]
public class ShotScript : NetworkBehaviour
{
	// 1 - Designer variables

	/// <summary>
	/// Damage inflicted
	/// </summary>
	public int damage = 1;

	/// <summary>
	/// Projectile damage player or enemies?
	/// </summary>
	public bool isEnemyShot = false;

	void Start()
	{
		// 2 - Limited time to live to avoid any leak
		if (IsClient)
			Invoke(nameof(DeleteServerRpc), 6);
		else
			Destroy(gameObject, 3); // 3sec

	}

	[ServerRpc(RequireOwnership = false)]
	private void DeleteServerRpc()
	{
		gameObject.GetComponent<NetworkObject>().Despawn(true);
	}
}