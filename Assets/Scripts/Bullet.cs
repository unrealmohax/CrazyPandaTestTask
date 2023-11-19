using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private float _lifeTime;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private void OnValidate()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        _rigidbody2D = _rigidbody2D != null ? _rigidbody2D : GetComponent<Rigidbody2D>();

        // Set the initial velocity for the bullet
        _rigidbody2D.velocity = GetVelocityVector();

        StartCoroutine(DelayDestroy());
    }

    // Calculate the velocity vector based on the rotation angle of the bullet
    private Vector2 GetVelocityVector()
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

    // Coroutine to destroy the bullet after a delay
    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(_lifeTime);

        Destroy(gameObject);
    }
}