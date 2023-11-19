using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject _leftTurretPrefab;
    [SerializeField] private GameObject _rightTurretPrefab;
    [SerializeField] private GameObject _timeFreezeZonePrefab;
    [SerializeField] private Transform _spawnPointLeftTurret;
    [SerializeField] private Transform _spawnPointRightTurret;
    [SerializeField] private Transform _spawnPointTimeFreezeZone;

    private void Start()
    {
        GameObject freezeZone = Instantiate(_timeFreezeZonePrefab, _spawnPointTimeFreezeZone);
        GameObject leftTurretObject = Instantiate(_leftTurretPrefab, _spawnPointLeftTurret);
        GameObject rightTurretObject  = Instantiate(_rightTurretPrefab, _spawnPointRightTurret);
    }
}
