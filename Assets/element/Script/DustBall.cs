using UnityEngine;

public class DustBall : MonoBehaviour
{
    public ParticleSystem destructionParticles; // Particules de disparition
    public AudioClip destructionSound; // Son de disparition
    public float soundVolume = 1.0f; // Volume du son

    public void DestroyDustBall()
    {
        // Joue les particules de disparition
        if (destructionParticles != null)
        {
            Instantiate(destructionParticles, transform.position, Quaternion.identity);
        }

        // Joue le son de disparition
        if (destructionSound != null)
        {
            AudioSource.PlayClipAtPoint(destructionSound, transform.position, soundVolume);
        }

        // Détruit l'objet
        Destroy(gameObject);
    }
}