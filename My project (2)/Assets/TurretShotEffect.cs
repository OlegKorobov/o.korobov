using UnityEngine;

public class TurretShotEffect : MonoBehaviour
{
    public ParticleSystem muzzleFlash;
    public AudioSource shotSound;

    public void PlayShotEffect()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (shotSound != null)
        {
            if (!shotSound.isPlaying)
            {
                shotSound.Play();
            }
        }
    }
}
