using UnityEngine;

public class FirePrefab : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private ParticleSystem smokeParticles;

    private void Reset()
    {
        CreateFireObject();
    }

    private void Awake()
    {
        if (fireParticles == null || smokeParticles == null)
        {
            CreateFireObject();
        }
    }

    private void CreateFireObject()
    {
        if (fireParticles == null)
        {
            fireParticles = CreateParticleSystem("Fire", new Color(1f, 0.5f, 0f), 0.5f, 1.5f, 30f, 0.6f, 0.3f);
            fireParticles.transform.SetParent(transform, false);
        }

        if (smokeParticles == null)
        {
            smokeParticles = CreateParticleSystem("Smoke", new Color(0.3f, 0.3f, 0.3f, 0.8f), 0.8f, 2.2f, 12f, 0.8f, 0.5f);
            smokeParticles.transform.SetParent(transform, false);
        }

        fireParticles.Play();
        smokeParticles.Play();
    }

    [System.Obsolete]
    private ParticleSystem CreateParticleSystem(string name, Color color, float startSize, float lifeTime, float emissionRate, float gravity, float speed)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(transform, false);

        ParticleSystem ps = obj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = lifeTime;
        main.startSpeed = speed;
        main.startSize = startSize;
        main.startColor = color;
        main.loop = true;
        main.playOnAwake = false;

        var emission = ps.emission;
        emission.rateOverTime = emissionRate;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;

        var vel = ps.velocityOverLifetime;
        vel.enabled = true;
        vel.space = ParticleSystemSimulationSpace.Local;

        //var gravityModifier = ps.gravityModifier;
        //gravityModifier.enabled = true;
        //gravityModifier.multiplier = gravity;

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.material.color = color;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        return ps;
    }
}
