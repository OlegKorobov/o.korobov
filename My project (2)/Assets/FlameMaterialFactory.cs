using UnityEngine;

public class FlameMaterialFactory : MonoBehaviour
{
    public static Material CreateFireMaterial()
    {
        Material fireMat = new Material(Shader.Find("Sprites/Default"));
        fireMat.color = new Color(1f, 0.55f, 0.1f, 1f);
        fireMat.EnableKeyword("_EMISSION");
        fireMat.SetColor("_EmissionColor", new Color(1f, 0.4f, 0f, 1f));
        return fireMat;
    }

    public static Material CreateSmokeMaterial()
    {
        Material smokeMat = new Material(Shader.Find("Sprites/Default"));
        smokeMat.color = new Color(0.3f, 0.3f, 0.3f, 0.75f);
        smokeMat.SetFloat("_Mode", 3f);
        smokeMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        smokeMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        smokeMat.SetInt("_ZWrite", 0);
        smokeMat.DisableKeyword("_ALPHATEST_ON");
        smokeMat.EnableKeyword("_ALPHABLEND_ON");
        smokeMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        return smokeMat;
    }
}
