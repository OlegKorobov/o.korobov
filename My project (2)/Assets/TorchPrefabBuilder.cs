using UnityEngine;

public class TorchPrefabBuilder : MonoBehaviour
{
    public static GameObject CreateTorchPrefab(string name = "TorchPrefab")
    {
        GameObject torch = new GameObject(name);

        var body = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        body.name = "Body";
        body.transform.SetParent(torch.transform, false);
        body.transform.localScale = new Vector3(0.12f, 0.8f, 0.12f);
        body.transform.localPosition = Vector3.zero;

        var flame = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flame.name = "Flame";
        flame.transform.SetParent(torch.transform, false);
        flame.transform.localPosition = new Vector3(0f, 0.8f, 0f);
        flame.transform.localScale = new Vector3(0.2f, 0.35f, 0.2f);

        var renderer = flame.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(1f, 0.55f, 0.1f);
        }

        var lightGo = new GameObject("TorchLight");
        lightGo.transform.SetParent(torch.transform, false);
        lightGo.transform.localPosition = new Vector3(0f, 0.8f, 0f);

        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.6f, 0.2f);
        light.intensity = 2.5f;
        light.range = 6f;

        var torchScript = torch.AddComponent<Torch>();
        torchScript.flameObject = flame;
        torchScript.flameLight = light;
        torchScript.lightColor = light.color;
        torchScript.lightIntensity = light.intensity;
        torchScript.lightRange = light.range;

        var col = torch.AddComponent<SphereCollider>();
        col.radius = 0.5f;
        col.isTrigger = true;

        return torch;
    }
}
