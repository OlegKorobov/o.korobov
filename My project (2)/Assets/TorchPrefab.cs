using UnityEngine;

public class TorchPrefab : MonoBehaviour
{
    public static GameObject CreateTorch()
    {
        GameObject torch = new GameObject("TorchPrefab");

        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        body.name = "Body";
        body.transform.SetParent(torch.transform, false);
        body.transform.localScale = new Vector3(0.12f, 0.8f, 0.12f);
        body.transform.localPosition = Vector3.zero;

        GameObject flame = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flame.name = "Flame";
        flame.transform.SetParent(torch.transform, false);
        flame.transform.localPosition = new Vector3(0f, 0.9f, 0f);
        flame.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);

        var flameRenderer = flame.GetComponent<Renderer>();
        if (flameRenderer != null)
        {
            flameRenderer.material = new Material(Shader.Find("Standard"));
            flameRenderer.material.color = new Color(1f, 0.55f, 0.1f);
        }

        GameObject lightGo = new GameObject("TorchLight");
        lightGo.transform.SetParent(torch.transform, false);
        lightGo.transform.localPosition = new Vector3(0f, 0.9f, 0f);

        Light light = lightGo.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.6f, 0.2f);
        light.intensity = 2.5f;
        light.range = 6f;

        var collider = torch.AddComponent<SphereCollider>();
        collider.radius = 0.45f;
        collider.isTrigger = true;

        torch.AddComponent<Torch>();
        return torch;
    }
}
