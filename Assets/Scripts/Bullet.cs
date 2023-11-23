using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private float _lifeTime;
    [SerializeField, Range(0.01f, float.PositiveInfinity)] private float _localTimeScale = 1f;

    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;

    private Vector2 _initialPositionVector;
    private Vector2 _initialVelocityVector;

    private float _time = 0;
    private float _defaultTimeScale;
    private const float GRAVITY = 9.8f;


    public Rigidbody2D Rigidbody => _rigidbody;
    public Vector2 VelocityVector => GetVelocity();

    // Method to set local time scale with value validation
    internal void SetLocalTimeScale(float value)
    {
        if (value < 0.01f) throw new Exception("Set Local TimeScale value <= 0");
        _localTimeScale = value;
    }

    // Method to set default local time scale
    internal void SetDefaultLocalTimeScale()
    {
        _localTimeScale = _defaultTimeScale;
    }

    // Called when values are changed in the inspector
    private void OnValidate()
    {
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
        if (_collider == null) _collider = GetComponent<CircleCollider2D>();
    }

    // Called on start
    private void Start()
    {
        _rigidbody = _rigidbody != null ? _rigidbody : GetComponent<Rigidbody2D>();
        _collider = _collider != null ? _collider : GetComponent<CircleCollider2D>();

        // Set initial position vector and initial velocity vector
        _initialPositionVector = transform.position;
        _initialVelocityVector = GetInitialVelocityVector();
        _defaultTimeScale = _localTimeScale;
    }

    // Called on fixed update
    private void FixedUpdate()
    {
        // Update time with respect to local time scale
        _time += Time.fixedDeltaTime * _localTimeScale;

        // Calculate velocity vector and move the bullet
        _rigidbody.MovePosition(GetPosition());

        // Check if the bullet has exceeded its lifetime
        if (_time >= _lifeTime)
        {
            Destroy(gameObject);
        }
    }

    // Called on collision with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bullet otherBullet))
        {
            HandleCollision(collision, otherBullet);
        }
    }

    // Calculate the initial velocity vector based on the rotation angle of the bullet
    private Vector2 GetInitialVelocityVector()
    {
        // Get the rotation angle and ensure it's in the range [-180, 180]
        float angle = transform.eulerAngles.z;
        angle = Mathf.Repeat(angle + 180, 360) - 180;

        // Convert angle to radians
        float angleRad = Mathf.Abs(angle) * Mathf.Deg2Rad;

        // Determine the horizontal velocity based on the angle
        float velocityX = angle >= 0 ? _velocity : -_velocity;

        // Calculate and return the velocity vector
        return new Vector2(Mathf.Cos(angleRad) * velocityX, Mathf.Sin(angleRad) * _velocity);
    }

    // Calculate and return the velocity vector of the bullet
    private Vector2 GetVelocity()
    {
        float velocityX = _initialVelocityVector.x;
        float velocityY = _initialVelocityVector.y - GRAVITY * _time;
        return new Vector2(velocityX, velocityY);
    }

    // Calculate and return the position vector of the bullet
    private Vector2 GetPosition()
    {
        float posX = _initialPositionVector.x + _initialVelocityVector.x * _time;
        float posY = _initialPositionVector.y + _initialVelocityVector.y * _time - 0.5f * GRAVITY * Mathf.Pow(_time, 2);

        return new Vector2(posX, posY);
    }

    // Handle collision with another bullet
    private void HandleCollision(Collision2D collision, Bullet otherBullet)
    {
        Vector2 velocityVector = GetVelocity();

        Vector2 relativeVelocity = velocityVector - otherBullet.VelocityVector;
        Vector2 normal = collision.contacts[0].normal;
        float impulse = Vector2.Dot(relativeVelocity, normal) * 2 / (1 / _rigidbody.mass + 1 / otherBullet.Rigidbody.mass);

        _initialPositionVector = _rigidbody.position;
        _initialVelocityVector = velocityVector - (impulse / _rigidbody.mass * normal);

        _time = 0;
    }
}
