using UnityEngine;

public class LightController : MonoBehaviour
{

    public float minIntensity = -10f; // Minimum intensity of the light
    public float maxIntensity = 0f; // Maximum intensity of the light
    public float flickerSpeed = 2f; // Speed of flickering

    [SerializeField] Light pointLight;
    private float randomOffset;

    private void Start()
    {
        randomOffset = Random.Range(0f, 10f); // Random initial offset for flickering
    }

    private void Update()
    {
        // Calculate flicker intensity using PerlinNoise
        float flickerIntensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed + randomOffset, 0f));

        // Update the light intensity
        // pointLight.intensity = flickerIntensity;
    }

}

