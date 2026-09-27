using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public float range = 10f;
    public float fireRate = 1f;
    public float projectileSpeed = 8f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float fireCooldown;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > range)
        {
            target = null;
            return;
        }

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (Time.time >= fireCooldown)
        {
            Shoot();
            fireCooldown = Time.time + fireRate;
        }
    }

    private void FindTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Turret projectile prefab is missing.");
            return;
        }

        if (firePoint == null)
        {
            firePoint = transform;
        }

        TurretShotEffect effect = GetComponent<TurretShotEffect>();
        if (effect != null)
        {
            effect.PlayShotEffect();
        }

        TurretMuzzleFlash muzzleFlash = GetComponentInChildren<TurretMuzzleFlash>();
        if (muzzleFlash != null)
        {
            muzzleFlash.PlayFlash();
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = projectile.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        Collider collider = projectile.GetComponent<Collider>();
        if (collider == null)
        {
            collider = projectile.AddComponent<SphereCollider>();
        }

        collider.isTrigger = true;

        Vector3 direction = (target.position - firePoint.position).normalized;
        rb.linearVelocity = direction * projectileSpeed;
    }
}
