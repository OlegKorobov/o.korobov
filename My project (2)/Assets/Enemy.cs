using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    private float nextAttackTime;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.green;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
        rb.mass = 1f;

        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    private void Update()
    {
        if (currentHealth <= 0)
            return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
            return;

        Vector3 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        if (distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.transform.position,
                moveSpeed * Time.deltaTime
            );
        }
        else if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log($"{name} атакует игрока");

            Health customHealth = player.GetComponent<Health>();
            if (customHealth != null)
            {
                customHealth.TakeDamage(Mathf.CeilToInt(attackDamage));
                return;
            }

            var fpsHealth = player.GetComponent<Unity.FPS.Game.Health>();
            if (fpsHealth != null)
            {
                fpsHealth.TakeDamage(attackDamage, gameObject);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} побеждён");
        Destroy(gameObject);
    }
}
