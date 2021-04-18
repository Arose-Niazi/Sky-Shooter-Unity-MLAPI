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

	private Transform _shotTransform;
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
				_shotTransform = Instantiate(shotPrefab,transform.position, shotPrefab.rotation);
				CompleteShot(isEnemy);
			}
			else if(IsOwner)
				CreateShotServerRpc(OwnerClientId,isEnemy, transform.position);

			// Assign position
			
		}
	}
	
	public bool CanAttack => _shootCooldown <= 0f;

	[ServerRpc(RequireOwnership = false)]
	private void CreateShotServerRpc(ulong clientID, bool isEnemy, Vector3 pos)
	{
		Debug.Log($"[Server] CreateShot for {clientID}");
		_shotTransform = Instantiate(shotPrefab, pos, shotPrefab.rotation);
		_shotTransform.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(clientID,null, true);
		CompleteShot(isEnemy);
	}

	private void CompleteShot(bool isEnemy)
	{
		if(IsClient)
			Debug.Log($"[Client] ShotComplete for {OwnerClientId}");

		// The is enemy property
		ShotScript shot = _shotTransform.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			shot.isEnemyShot = isEnemy;
		}

		// Make the weapon shot always towards it
		DirectionMoveScript move = _shotTransform.gameObject.GetComponent<DirectionMoveScript>();
		if (move != null)
		{
			move.direction = transform.right; // towards in 2D space is the right of the sprite
		}
	}
}