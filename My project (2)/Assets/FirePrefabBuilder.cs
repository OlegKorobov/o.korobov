using UnityEngine;

public class FirePrefabBuilder : MonoBehaviour
{
    public static GameObject CreateFirePrefab(string name = "FirePrefab")
    {
        GameObject root = new GameObject(name);

        ParticleSystem fire = CreateParticleSystem(root.transform, "Fire", Color.yellow, 0.5f, 1.5f, 25f, 0.6f, 0.2f);
        ParticleSystem smoke = CreateParticleSystem(root.transform, "Smoke", new Color(0.2f, 0.2f, 0.2f, 0.8f), 0.7f, 2f, 12f, 0.9f, 0.5f);

        FireParticlesSystem system = root.AddComponent<FireParticlesSystem>();
        system.fireParticles = fire;
        system.smokeParticles = smoke;
        system.startOnAwake = true;

        return root;
    }

    [System.Obsolete]
    private static ParticleSystem CreateParticleSystem(Transform parent, string name, Color color, float startSize, float lifeTime, float rate, float gravity, float speed)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var ps = go.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = lifeTime;
        main.startSpeed = speed;
        main.startSize = startSize;
        main.startColor = color;
        main.loop = true;
        main.playOnAwake = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = rate;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.15f;

        var velocity = ps.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        var x = velocity.x;
        x.constantMin = -0.2f;
        x.constantMax = 0.2f;
        var y = velocity.y;
        y.constantMin = 0.5f;
        y.constantMax = 1.2f;
        var z = velocity.z;
        z.constantMin = -0.2f;
        z.constantMax = 0.2f;

        //var gravityModule = ps.gravityModifier;
        //gravityModule.enabled = true;
        //gravityModule.multiplier = gravity;

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = CreateMaterial(color);
        renderer.renderMode = ParticleSystemRenderMode.Billboard;

        return ps;
    }

    private static Material CreateMaterial(Color color)
    {
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = color;
        mat.SetInt("_RenderMode", 2);
        return mat;
    }
}
