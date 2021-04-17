using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

/// <summary>
/// Launch projectile
/// </summary>
public class WeaponScript : NetworkBehaviour
{
	public Transform shotPrefab;
	public float shootingRate = 0.25f;

	private Transform shotTransform;
	private float _shootCooldown;

	void Start()
	{
		_shootCooldown = 0f;
	}

	void Update()
	{
		if (_shootCooldown > 0)
		{
			_shootCooldown -= Time.deltaTime;
		}
	}
	
	public void Attack(bool isEnemy)
	{
		if (CanAttack)
		{
			_shootCooldown = shootingRate;

			// Create a new shot
			if (!IsClient)
			{
				shotTransform = Instantiate(shotPrefab);
				CompleteShot(isEnemy);
			}
			else
				CreateShotServerRpc(OwnerClientId,isEnemy);

			// Assign position
			
		}
	}
	
	public bool CanAttack => _shootCooldown <= 0f;

	[ServerRpc]
	private void CreateShotServerRpc(ulong clientID, bool isEnemy)
	{
		Debug.Log($"[Server] CreateShot for {clientID}");
		shotTransform = Instantiate(shotPrefab);
		shotTransform.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientID,null, true);
		CompleteShot(isEnemy);
	}

	private void CompleteShot(bool isEnemy)
	{
		shotTransform.position = transform.position;

		// The is enemy property
		ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			shot.isEnemyShot = isEnemy;
		}

		// Make the weapon shot always towards it
		DirectionMoveScript move = shotTransform.gameObject.GetComponent<DirectionMoveScript>();
		if (move != null)
		{
			move.direction = transform.right; // towards in 2D space is the right of the sprite
		}
	}
}