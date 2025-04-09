using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class SoldringIron : MonoBehaviour
{
    
    private XRGrabInteractable grabInteractable;

    private bool isAspiring = false;
    private XRNode controllerNodeL = XRNode.LeftHand; // (gauche)
    private XRNode controllerNodeR = XRNode.RightHand; // (droite)

    private AudioSource audioSource;

    private bool isGrabbed = false;
    private bool isPressed = false;


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

    public bool getIsPressed() {
        return isPressed;
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


            isPressed = flagA || flagB;
        

            // Gère l'aspiration si elle est activée
            if (isPressed && !audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("start audio SoldringIron");
            }
            else if(!isPressed && audioSource.isPlaying) {
                audioSource.Stop();
                Debug.Log("Stop audio SoldringIron");
            }
        }
        else if(audioSource.isPlaying) {
            audioSource.Stop();
        }
    }
}