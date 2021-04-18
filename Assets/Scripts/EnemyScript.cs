using MLAPI;
using UnityEngine;

/// <summary>
/// Enemy generic behavior
/// </summary>
public class EnemyScript : NetworkBehaviour
{
    private bool _hasSpawn;
    private DirectionMoveScript _moveScript;
    private WeaponScript[] _weapons;
    private Collider2D _coliderComponent;
    private SpriteRenderer _rendererComponent;
    private Camera _camera;

    void Awake()
    {
        // Retrieve the weapon only once
        _weapons = GetComponentsInChildren<WeaponScript>();

        // Retrieve scripts to disable when not spawn
        _moveScript = GetComponent<DirectionMoveScript>();

        _coliderComponent = GetComponent<Collider2D>();

        _rendererComponent = GetComponent<SpriteRenderer>();
        _camera = Camera.main;
        
    }

    // 1 - Disable everything
    void Start()
    {
        _hasSpawn = false;

        // Disable everything
        // -- collider
        _coliderComponent.enabled = false;
        // -- Moving
        _moveScript.enabled = false;
        // -- Shooting
        foreach (WeaponScript weapon in _weapons)
        {
            weapon.enabled = false;
        }
    }

    void Update()
    {
        // 2 - Check if the enemy has spawned.
        if (_hasSpawn == false)
        {
            if (_rendererComponent.IsVisibleFrom(_camera))
            {
                Spawn();
            }
        }
        else
        {
            // Auto-fire
            foreach (WeaponScript weapon in _weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                {
                    weapon.Attack(true);
                    SoundEffectsHelper.Instance.MakeEnemyShotSound();
                }
            }

            // 4 - Out of the camera ? Destroy the game object.
            if (_rendererComponent.IsVisibleFrom(_camera) == false)
            {
                Destroy(gameObject);
            }
        }
    }

    // 3 - Activate itself.
    private void Spawn()
    {
        _hasSpawn = true;

        // Enable everything
        // -- Collider
        _coliderComponent.enabled = true;
        // -- Moving
        _moveScript.enabled = true;
        // -- Shooting
        foreach (WeaponScript weapon in _weapons)
        {
            weapon.enabled = true;
        }
    }
}