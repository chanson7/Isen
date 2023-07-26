using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour
{

    [SerializeField] Light pointLight;
    [SerializeField] Light spotLight;
    [SerializeField] MeshRenderer bulbMesh;
    bool isFlickering = false;
    float timeDelay;

    private void Update()
    {
        if (isFlickering == false)
            StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight()
    {
        isFlickering = true;
        pointLight.enabled = false;
        spotLight.enabled = false;

        //turn the light off and wait a long time
        bulbMesh.material.DisableKeyword("_EMISSION");
        timeDelay = Random.Range(1f, 20f);
        yield return new WaitForSeconds(timeDelay);

        //turn the light on and wait a little time
        float intensity = Random.Range(0.1f, 2f);
        int flickerCount = Random.Range(1, 3);

        for (int i = 0; i < flickerCount; i++)
        {
            pointLight.enabled = true;
            pointLight.intensity = intensity / 2f;
            spotLight.enabled = true;
            spotLight.intensity = intensity;
            bulbMesh.material.EnableKeyword("_EMISSION");
            timeDelay = Random.Range(0.01f, 0.6f);
            yield return new WaitForSeconds(timeDelay);
            isFlickering = false;
        }
    }

}

