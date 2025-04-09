using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform porte; // Référence à la porte
    public float angleOuverture = 90f; // Angle d'ouverture
    public float vitesseRotation = 2f; // Vitesse de rotation

    private bool porteOuverte = false;
    private Quaternion positionInitiale;
    private Quaternion positionFinale;

    void Start()
    {
        // Sauvegarde des positions initiale et finale
        positionInitiale = porte.localRotation;
        positionFinale = positionInitiale * Quaternion.Euler(angleOuverture, 0f, 0f);
    }

    // Méthode appelée par PhysicsButton quand onPressed est invoqué
    public void ActiverPorte()
    {

        Debug.Log("Active Porte");
        porteOuverte = !porteOuverte; // Inverse l'état de la porte
        StopAllCoroutines();
        StartCoroutine(OuvrirPorte());
    }

    private System.Collections.IEnumerator OuvrirPorte()
    {
        Quaternion cible = porteOuverte ? positionFinale : positionInitiale;

        while (Quaternion.Angle(porte.localRotation, cible) > 0.01f)
        {
            porte.localRotation = Quaternion.Lerp(porte.localRotation, cible, Time.deltaTime * vitesseRotation);
            yield return null;
        }

        porte.localRotation = cible; 
    }
}