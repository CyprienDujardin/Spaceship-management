using UnityEngine;

public class ResetPositionObject : MonoBehaviour
{

    private Vector3 initialPosition; // Position initiale
    private Quaternion initialRotation; // Rotation initiale

    void Start()
    {
        // Stocker la position et rotation de départ
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetToInitialPosition()
    {
        // Réinitialiser la vélocité si un Rigidbody est présent
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Réinitialiser la position et la rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
