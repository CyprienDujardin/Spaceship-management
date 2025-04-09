using System.Collections.Generic;
using UnityEngine;


public class Saisir : MonoBehaviour
{
    [SerializeField] private bool droite = false;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable closestItem;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactingItem;
    private HashSet<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable> objectsHoveringOver = new HashSet<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor xrInteractor;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable xrInteractable;


    //Adaption du script Saisir initial avec mise � jour par le XR Interaction Toolkit
    void Start()
    {
        interactor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor>();
        if (interactor == null)
        {
            Debug.LogError("XRBaseInteractor n�cessaire pour ce GO.");
        }
        xrInteractor = interactor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor;
        xrInteractable = interactingItem as UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable;
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (interactable != null && !objectsHoveringOver.Contains(interactable))
        {
            objectsHoveringOver.Add(interactable);
            UpdateClosestItem(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (interactable != null)
        {
            objectsHoveringOver.Remove(interactable);

            if (interactable == closestItem)
            {
                closestItem = null;
                FindClosestItem();
            }
        }
    }

    void Update()
    {
        if (closestItem != null && interactingItem == null && interactor.isSelectActive)
        {
            interactingItem = closestItem;
            interactor.interactionManager.SelectEnter(xrInteractor, xrInteractable);
            Debug.Log($"Item grabbed by {(droite ? "right" : "left")} hand");
        }

        if (interactingItem != null && !interactor.isSelectActive)
        {
            interactor.interactionManager.SelectExit(xrInteractor, xrInteractable);
            interactingItem = null;
            Debug.Log("Item released");
        }
    }

    private void UpdateClosestItem(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable newInteractable)
    {
        if (closestItem == null ||
            (newInteractable.transform.position - transform.position).sqrMagnitude <
            (closestItem.transform.position - transform.position).sqrMagnitude)
        {
            closestItem = newInteractable;
        }
    }

    private void FindClosestItem()
    {
        float minDistance = float.MaxValue;
        closestItem = null;

        foreach (var item in objectsHoveringOver)
        {
            float distance = (item.transform.position - transform.position).sqrMagnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                closestItem = item;
            }
        }
    }
}
