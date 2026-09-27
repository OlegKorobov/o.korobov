using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;
    public bool damagePlayer = true;
    public bool damageEnemies = true;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        ApplyDamage(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyDamage(collision.gameObject);
    }

    private void ApplyDamage(GameObject target)
    {
        if (target == null)
            return;

        if (damagePlayer && target.CompareTag("Player"))
        {
            Health customHealth = target.GetComponentInParent<Health>();
            if (customHealth != null)
            {
                customHealth.TakeDamage(damage);
                Debug.Log($"Projectile hit player {target.name} for {damage} damage");
                Destroy(gameObject);
                return;
            }

            Unity.FPS.Game.Health fpsHealth = target.GetComponentInParent<Unity.FPS.Game.Health>();
            if (fpsHealth != null)
            {
                fpsHealth.TakeDamage(damage, gameObject);
                Debug.Log($"Projectile hit FPS player {target.name} for {damage} damage");
                Destroy(gameObject);
                return;
            }
        }

        if (damageEnemies)
        {
            Enemy enemy = target.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Projectile hit enemy {target.name} for {damage} damage");
                Destroy(gameObject);
                return;
            }
        }

        Destroy(gameObject);
    }
}
