using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private Transform _missileSpawnPoint;
    [SerializeField] private float _rateOfFire;
    [SerializeField] private Transform _turretTransform;
    [SerializeField] private float _offset;
    [SerializeField] private float _minDegRotate;
    [SerializeField] private float _maxDegRotate;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;

        // Start coroutines for aiming at the mouse and firing missiles
        StartCoroutine(ShotCoroutine());
        StartCoroutine(AimTargetCoroutine());
    }

    private IEnumerator AimTargetCoroutine()
    {
        while (true)
        {
            // Calculate the angle between the turret and the mouse position
            Vector2 difference = _camera.ScreenToWorldPoint(Input.mousePosition) - _turretTransform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            
            // Clamp the rotation angle within the specified range
            rotZ = Mathf.Clamp(rotZ, _minDegRotate, _maxDegRotate);

            // Apply rotation to the turret with the specified offset
            _turretTransform.rotation = Quaternion.Euler(new Vector3(0, 0, rotZ + _offset));
            
            yield return null;
        }
    }

    private IEnumerator ShotCoroutine()
    {
        float delayShotTime = 1 / _rateOfFire; // Calculate delay between shots based on rate of fire

        while (true)
        {
            // Instantiate a missile at the spawn point with the turret's rotation
            Instantiate(_missilePrefab, _missileSpawnPoint.position, _turretTransform.rotation);

            yield return new WaitForSeconds(delayShotTime);
        }
    }
}