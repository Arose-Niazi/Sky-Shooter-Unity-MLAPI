using UnityEngine;

/// <summary>
/// Launch projectile
/// </summary>
public class WeaponScript : MonoBehaviour
{
	public Transform shotPrefab;
	public float shootingRate = 0.25f;
	
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
			var shotTransform = Instantiate(shotPrefab);

			// Assign position
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
	
	public bool CanAttack => _shootCooldown <= 0f;
}