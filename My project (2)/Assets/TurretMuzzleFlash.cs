using UnityEngine;

public class TurretMuzzleFlash : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public float flashDuration = 0.08f;

    private float flashTimer;

    private void Update()
    {
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f && muzzleFlash != null)
            {
                muzzleFlash.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    public void PlayFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
            flashTimer = flashDuration;
        }
    }
}
