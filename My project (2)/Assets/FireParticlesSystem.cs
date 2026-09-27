using UnityEngine;

public class FireParticlesSystem : MonoBehaviour
{
    public ParticleSystem fireParticles;
    public ParticleSystem smokeParticles;
    public bool startOnAwake = true;

    private void Awake()
    {
        if (startOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        if (fireParticles != null)
        {
            fireParticles.Play();
        }

        if (smokeParticles != null)
        {
            smokeParticles.Play();
        }
    }

    public void Stop()
    {
        if (fireParticles != null)
        {
            fireParticles.Stop();
        }

        if (smokeParticles != null)
        {
            smokeParticles.Stop();
        }
    }
}
