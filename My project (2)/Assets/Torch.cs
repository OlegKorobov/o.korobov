using UnityEngine;

public class Torch : MonoBehaviour
{
    [Header("Visual")]
    public GameObject flameObject;
    public GameObject smokeObject;
    public Light flameLight;
    public Color lightColor = new Color(1f, 0.5f, 0.1f, 1f);

    [Header("Settings")]
    public float lightIntensity = 2.5f;
    public float lightRange = 6f;

    private void Reset()
    {
        SetupTorch();
    }

    private void Awake()
    {
        SetupTorch();
    }

    private void SetupTorch()
    {
        if (flameObject == null)
        {
            flameObject = new GameObject("Flame");
            flameObject.transform.SetParent(transform, false);
            flameObject.transform.localPosition = new Vector3(0f, 0.9f, 0f);
            flameObject.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);
        }

        if (smokeObject == null)
        {
            smokeObject = new GameObject("Smoke");
            smokeObject.transform.SetParent(transform, false);
            smokeObject.transform.localPosition = new Vector3(0f, 0.95f, 0f);
            smokeObject.transform.localScale = new Vector3(0.35f, 0.5f, 0.35f);
        }

        if (flameLight == null)
        {
            GameObject lightGo = new GameObject("TorchLight");
            lightGo.transform.SetParent(transform, false);
            lightGo.transform.localPosition = new Vector3(0f, 0.9f, 0f);
            flameLight = lightGo.AddComponent<Light>();
            flameLight.type = LightType.Point;
            flameLight.color = lightColor;
            flameLight.intensity = lightIntensity;
            flameLight.range = lightRange;
        }

        if (GetComponent<Collider>() == null)
        {
            SphereCollider col = gameObject.AddComponent<SphereCollider>();
            col.radius = 0.45f;
            col.isTrigger = true;
        }

        FlameEffect(flameObject, true);
        FlameEffect(smokeObject, false);

        if (flameLight != null)
        {
            flameLight.enabled = true;
            flameLight.color = lightColor;
            flameLight.intensity = lightIntensity;
            flameLight.range = lightRange;
        }
    }

    private void FlameEffect(GameObject flame, bool isFire)
    {
        var ps = flame.GetComponent<ParticleSystem>();
        if (ps == null)
        {
            ps = flame.AddComponent<ParticleSystem>();
        }

        var main = ps.main;
        main.loop = true;
        main.playOnAwake = true;
        main.startLifetime = isFire ? 0.6f : 1.2f;
        main.startSpeed = isFire ? 0.8f : 0.2f;
        main.startSize = isFire ? 0.18f : 0.25f;
        main.startColor = isFire ? new Color(1f, 0.55f, 0.1f) : new Color(0.25f, 0.25f, 0.25f, 0.8f);

        var emission = ps.emission;
        emission.rateOverTime = isFire ? 30f : 12f;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = isFire ? 0.12f : 0.18f;

        var vel = ps.velocityOverLifetime;
        vel.enabled = true;
        vel.space = ParticleSystemSimulationSpace.Local;
        vel.y = new ParticleSystem.MinMaxCurve(isFire ? 0.5f : 0.15f, isFire ? 1.2f : 0.7f);

        var rendererComp = ps.GetComponent<ParticleSystemRenderer>();
        rendererComp.material = isFire ? FlameMaterialFactory.CreateFireMaterial() : FlameMaterialFactory.CreateSmokeMaterial();
        rendererComp.renderMode = ParticleSystemRenderMode.Billboard;
    }
}
