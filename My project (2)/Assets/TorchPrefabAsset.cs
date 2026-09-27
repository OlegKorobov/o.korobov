using UnityEngine;

public class TorchPrefabAsset : MonoBehaviour
{
    [Header("Torch Setup")]
    public GameObject flame;
    public Light flameLight;

    [Header("Settings")]
    public Color lightColor = new Color(1f, 0.6f, 0.2f);
    public float lightIntensity = 2.5f;
    public float lightRange = 6f;

    private void Reset()
    {
        BuildTorch();
    }

    private void Awake()
    {
        BuildTorch();
    }

    private void BuildTorch()
    {
        if (transform.childCount == 0)
        {
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            body.name = "Body";
            body.transform.SetParent(transform, false);
            body.transform.localScale = new Vector3(0.12f, 0.8f, 0.12f);
            body.transform.localPosition = Vector3.zero;

            flame = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flame.name = "Flame";
            flame.transform.SetParent(transform, false);
            flame.transform.localPosition = new Vector3(0f, 0.85f, 0f);
            flame.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);

            var flameRenderer = flame.GetComponent<Renderer>();
            if (flameRenderer != null)
            {
                var mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(1f, 0.5f, 0.1f);
                flameRenderer.material = mat;
            }

            GameObject lightGo = new GameObject("TorchLight");
            lightGo.transform.SetParent(transform, false);
            lightGo.transform.localPosition = new Vector3(0f, 0.85f, 0f);
            flameLight = lightGo.AddComponent<Light>();
            flameLight.type = LightType.Point;
            flameLight.color = lightColor;
            flameLight.intensity = lightIntensity;
            flameLight.range = lightRange;

            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.radius = 0.45f;
            collider.isTrigger = true;
        }

        if (flameLight != null)
        {
            flameLight.color = lightColor;
            flameLight.intensity = lightIntensity;
            flameLight.range = lightRange;
        }
    }
}
