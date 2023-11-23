using UnityEngine;

public class TimeFreezeZone : MonoBehaviour
{
    [SerializeField, Range(1, 100)] private float _timeDilationFactor = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Bullet bullet))
        {
            float timeFactor = 1 / _timeDilationFactor;
            bullet.SetLocalTimeScale(timeFactor);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Bullet bullet)) 
        {
            bullet.SetDefaultLocalTimeScale();
        }
    }
}

