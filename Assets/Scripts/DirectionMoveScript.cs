using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class DirectionMoveScript : MonoBehaviour
{
    public Vector2 speed = new Vector2(10, 10);
    public Vector2 direction = new Vector2(-1, 0);

    private Vector2 _movement;
    private Rigidbody2D _rigidbodyComponent;

    public void Awake()
    {
        _rigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _movement = new Vector2(
            speed.x * direction.x,
            speed.y * direction.y);
    }

    void FixedUpdate()
    {
        _rigidbodyComponent.velocity = _movement;
    }
}