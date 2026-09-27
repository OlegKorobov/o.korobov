using UnityEngine;

public class SpawnRedCubesRow : MonoBehaviour
{
    public int count = 10;
    public float spacing = 1.5f;
    public Vector3 startPosition = new Vector3(-6.75f, 0.5f, 0f);

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"RedCube_{i}";
            cube.transform.position = startPosition + new Vector3(i * spacing, 0f, 0f);

            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red;
            }

            Rigidbody rb = cube.AddComponent<Rigidbody>();
            rb.mass = 1f;
            rb.useGravity = true;

            Health health = cube.AddComponent<Health>();
            health.maxHealth = 100;
        }
    }
}
