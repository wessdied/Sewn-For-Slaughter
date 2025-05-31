using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;

    private float timer;

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }
        timer = flickerSpeed;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);
            timer = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f); // Adds irregularity
        }
    }
}
