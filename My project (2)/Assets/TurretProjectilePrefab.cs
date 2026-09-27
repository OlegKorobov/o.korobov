using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class TurretProjectilePrefab : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = 0.2f;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        if (other.CompareTag("Player"))
        {
            Health customHealth = other.GetComponentInParent<Health>();
            if (customHealth != null)
            {
                customHealth.TakeDamage(damage);
                Debug.Log($"Turret projectile hit player {other.name} for {damage} damage");
                Destroy(gameObject);
                return;
            }

            Unity.FPS.Game.Health fpsHealth = other.GetComponentInParent<Unity.FPS.Game.Health>();
            if (fpsHealth != null)
            {
                fpsHealth.TakeDamage(damage, gameObject);
                Debug.Log($"Turret projectile hit FPS player {other.name} for {damage} damage");
                Destroy(gameObject);
                return;
            }
        }

        Destroy(gameObject);
    }
}
