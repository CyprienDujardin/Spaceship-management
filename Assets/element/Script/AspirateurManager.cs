using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AspirateurController : MonoBehaviour
{
    public Transform aspirationPoint; // Position de l'embout de l'aspirateur
    public float aspirationRadius = 10f; // Rayon d'aspiration
    public float aspirationForce = 5f; // Force appliquée sur les objets aspirés
    
    private XRGrabInteractable grabInteractable;

    private AudioSource audioSource;

    private bool isAspiring = false;
    private XRNode controllerNodeL = XRNode.LeftHand; // (gauche)
    private XRNode controllerNodeR = XRNode.RightHand; // (droite)

    private bool isGrabbed = false;


    void Start() {

        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if(grabInteractable != null) {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args) {
        isGrabbed = true;
    }

    void OnRelease(SelectExitEventArgs args) {
        isGrabbed = false;
    }

    void Update()
    {

        if(isGrabbed) {
            InputDevice controllerL = InputDevices.GetDeviceAtXRNode(controllerNodeL);
            InputDevice controllerR = InputDevices.GetDeviceAtXRNode(controllerNodeR);

            bool flagA = false;
            bool flagB = false;

            if (controllerL.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressedL))
            {
                flagA = isPressedL;
            }

            if (controllerR.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressedR))
            {
                flagB = isPressedR;
            }


            isAspiring = flagA || flagB;
        

            // Gère l'aspiration si elle est activée
            if (isAspiring)
            {
                Debug.Log("Pression");
                AspirerObjets();
            }

            if(isAspiring && !audioSource.isPlaying) {
                audioSource.Play();
            }
            else if(!isAspiring && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        else {

            if(audioSource.isPlaying) {
                audioSource.Stop();
            }

            isAspiring = false;
        }
    }

    private void AspirerObjets()
    {
        // Recherche tous les objets dans le rayon d'aspiration
        Collider[] colliders = Physics.OverlapSphere(aspirationPoint.position, aspirationRadius);

        Debug.Log("debut");
        foreach (Collider collider in colliders)
        {

            Debug.Log(collider.name);   

            // Vérifie si l'objet est une boule de poussière (par Tag)
            if (collider.CompareTag("DustBall"))
            {

                Debug.Log("DG 1");
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {

                    Debug.Log("DG 2");
                    // Applique une force vers l'embout de l'aspirateur
                    Vector3 direction = (aspirationPoint.position - collider.transform.position).normalized;
                    rb.AddForce(direction * aspirationForce, ForceMode.Acceleration);

                    // Détruit la boule si elle est très proche de l'embout
                    if (Vector3.Distance(aspirationPoint.position, collider.transform.position) < 0.2f)
                    {

                        Debug.Log("DG 3");
                        DustBall db = collider.GetComponent<DustBall>();
                        db.DestroyDustBall();
                    }
                }
            }
        }

        Debug.Log("end");
    }
}