using MLAPI;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(HealthScript))]
public class PlayerScript : NetworkBehaviour
{
	public Vector2 speed = new Vector2(50, 50);

	// 2 - Store the movement and the component
	private Vector2 _movement;
	private Rigidbody2D _rigidbodyComponent;
	private WeaponScript _weapon;
	private HealthScript _playerHealth;
	private Camera _camera;
	
	private void Awake()
	{
		_rigidbodyComponent = GetComponent<Rigidbody2D>();
		_weapon = GetComponent<WeaponScript>();
		_playerHealth = GetComponent<HealthScript>();
		_camera = Camera.main;
	}

	void Update()
	{
		if(IsClient && !IsOwner) return;

		float inputX;
		float inputY;
		
		bool shoot;
		
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		inputX = Input.GetAxis("Horizontal");
		inputY = Input.GetAxis("Vertical");
		
		shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
#elif UNITY_IOS || UNITY_ANDROID		
		
		inputX = Input.GetAxis("Mouse X");
		inputY = Input.GetAxis("Mouse Y");
		shoot = false;
		if (Input.touchCount > 0)
		{
			inputX = Input.touches[0].deltaPosition.x;
			inputY = Input.touches[0].deltaPosition.y;
		}
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);

			// -- Tap: quick touch & release
			// ------------------------------------------------
			if (touch.phase == TouchPhase.Ended && touch.tapCount == 1)
			{
				shoot = true;
			}
		}
		inputX /= 4;
		inputY /= 4;
#endif
		_movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
		
		
		if (shoot)
		{
			
			if (_weapon != null)
			{
				// false because the player is not an enemy
				_weapon.Attack(false);
				SoundEffectsHelper.Instance.MakePlayerShotSound();
			}
		}

		var position = transform.position;
		var dist = (position - _camera.transform.position).z;

		var leftBorder = _camera.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
		).x;

		var rightBorder = _camera.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
		).x;

		var topBorder = _camera.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
		).y;

		var bottomBorder = _camera.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
		).y;

		position = new Vector3(
			Mathf.Clamp(position.x, leftBorder, rightBorder),
			Mathf.Clamp(position.y, topBorder, bottomBorder),
			position.z
		);
		transform.position = position;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		bool damagePlayer = false;

		// Collision with enemy
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
		if (enemy != null)
		{
			// Kill the enemy
			HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
			if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);

			damagePlayer = true;
		}

		// Damage the player
		if (damagePlayer)
		{
			_playerHealth.Damage(1);
		}
	}

	void FixedUpdate()
	{
		_rigidbodyComponent.velocity = _movement;
	}
}