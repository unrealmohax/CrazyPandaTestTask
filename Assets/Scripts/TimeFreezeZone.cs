using System.Collections.Generic;
using UnityEngine;

public class TimeFreezeZone : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private float _timeDilationFactor = 10f;
    private Dictionary<Rigidbody2D, BodyInfo> _rigidBodiesInfo = new Dictionary<Rigidbody2D, BodyInfo>();
    private float _timeFactor;

    private void Start()
    {
        _timeFactor = 1 / _timeDilationFactor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Save information about the rigid body when it enters the trigger zone
        var info = new BodyInfo
        {
            PrevVelocity = null,
            UnscaledVelocity = other.attachedRigidbody.velocity,
            GravityScale = other.attachedRigidbody.gravityScale,
        };

        _rigidBodiesInfo.Add(other.attachedRigidbody, info);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_rigidBodiesInfo.ContainsKey(other.attachedRigidbody))
        {
            // Restore the original properties of the rigid body when it exits the trigger zone
            var info = _rigidBodiesInfo[other.attachedRigidbody];
            other.attachedRigidbody.velocity = info.UnscaledVelocity;
            other.attachedRigidbody.gravityScale = info.GravityScale;
            _rigidBodiesInfo.Remove(other.attachedRigidbody);
        }
    }

    private void FixedUpdate()
    {
        foreach (var rigidBodyInfo in _rigidBodiesInfo)
        {
            var rigidBody = rigidBodyInfo.Key;
            var info = rigidBodyInfo.Value;

            if (info.PrevVelocity != null)
            {
                // Apply time dilation to the rigid body's velocity
                var acceleration = rigidBody.velocity - info.PrevVelocity.Value;
                info.PrevVelocity = rigidBody.velocity = info.UnscaledVelocity * _timeFactor;
                info.UnscaledVelocity += acceleration;
            }
            else
            {
                // Apply time dilation to the rigid body's gravity scale and initial velocity
                rigidBody.gravityScale = info.GravityScale * _timeFactor;
                rigidBody.velocity = info.UnscaledVelocity * _timeFactor;
                info.PrevVelocity = rigidBody.velocity;
            }
        }
    }
}

// Class to store information about a rigid body
class BodyInfo
{
    public Vector2 UnscaledVelocity;   // Original velocity of the rigid body
    public Vector2? PrevVelocity;      // Previous velocity after time dilation is applied
    public float GravityScale;         // Original gravity scale of the rigid body
}