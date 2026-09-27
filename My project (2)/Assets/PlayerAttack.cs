using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2.5f;
    public int damage = 25;
    public KeyCode attackKey = KeyCode.F;
    public bool useMouseClick = true;

    private void Update()
    {
        if (useMouseClick && Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse pressed");
            Attack();
            return;
        }

        if (Input.GetKeyDown(attackKey))
        {
            Attack();
            return;
        }
    }

    private void Attack()
    {
    
        Debug.Log("Attack started");
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        Debug.Log("Attack radius hits: " + hits.Length);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Player hit enemy {hit.name} for {damage} damage");
                continue;
            }

            Health customHealth = hit.GetComponentInParent<Health>();
            if (customHealth != null)
            {
                customHealth.TakeDamage(damage);
                Debug.Log($"Player hit {hit.name} for {damage} damage");
                continue;
            }

            var fpsHealth = hit.GetComponentInParent<Unity.FPS.Game.Health>();
            if (fpsHealth != null)
            {
                fpsHealth.TakeDamage(damage, gameObject);
                Debug.Log($"Player hit FPS Health target {hit.name} for {damage} damage");
            }
        }
    }
}
