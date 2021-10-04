using System.Collections.Generic;
using UnityEngine;

public class CeilingLight : MonoBehaviour
{
    private static readonly int EmissionAmount = Shader.PropertyToID("_EmissionAmount");

    [SerializeField]
    private List<MeshRenderer> lightbulbRenderers;

    [SerializeField]
    private List<Light> lights;

    [SerializeField]
    [Range(0, 1.0f)]
    private float startingIntensity;

    private List<float> initialLightValues;

    private List<Material> lightbulbMaterials;

    protected void Awake()
    {
        lightbulbMaterials = new List<Material>();

        for (int i = 0; i < lightbulbRenderers.Count; i++) lightbulbMaterials.Add(lightbulbRenderers[i].material);

        initialLightValues = new List<float>();
        for (int i = 0; i < lights.Count; i++) initialLightValues.Add(lights[i].intensity);

        SetLightIntensity(startingIntensity);
    }

    public void ToggleLight(bool enable)
    {
        SetLightIntensity(enable ? 1.0f : 0.0f);
    }

    public void SetLightIntensity(float intensity)
    {
        for (int i = 0; i < lightbulbMaterials.Count; i++) lightbulbMaterials[i].SetFloat(EmissionAmount, intensity);

        for (int i = 0; i < lights.Count; i++)
        {
            float remappedIntensity = (intensity - 0.0f) / (1.0f - 0.0f) * (initialLightValues[i] - 0.0f) + 0.0f;
            lights[i].intensity = remappedIntensity;
        }
    }
}