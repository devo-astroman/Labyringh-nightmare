using UnityEngine;

public class EnemyBulletFirer : MonoBehaviour
{   
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _originBullet;
    [SerializeField] float bulletSpeed = 20f;

    public void FireBulletFromOringin(Vector3 destiny)
    {
        if (_bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned.");
            return;
        }

        Vector3 origin = _originBullet.position;

        // 1️ Instantiate bullet
        GameObject bullet = Instantiate(_bulletPrefab, origin, Quaternion.identity);

        // 2️ Calculate direction
        Vector3 direction = (destiny - origin).normalized;

        // 3️ Rotate bullet to face direction
        if (direction != Vector3.zero)
            bullet.transform.rotation = Quaternion.LookRotation(direction);

        // 4️ Apply velocity (requires Rigidbody)
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab has no Rigidbody.");
        }

        Destroy(bullet, 1f);
    }

    public void FireBullet(Vector3 origin, Vector3 destiny)
    {
        if (_bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned.");
            return;
        }

        // 1️ Instantiate bullet
        GameObject bullet = Instantiate(_bulletPrefab, origin, Quaternion.identity);

        // 2️ Calculate direction
        Vector3 direction = (destiny - origin).normalized;

        // 3️ Rotate bullet to face direction
        if (direction != Vector3.zero)
            bullet.transform.rotation = Quaternion.LookRotation(direction);

        // 4️ Apply velocity (requires Rigidbody)
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab has no Rigidbody.");
        }
    }
}