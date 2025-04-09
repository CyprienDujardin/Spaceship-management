using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CableEtirable : MonoBehaviour
{

    public Transform cableStart;    // point fixe de d�part
    public Transform positionEndOrigin;

    public GameObject fils;

    public Transform targetEndCable;
    public XRGrabInteractable cableEnd;  

    private Vector3 basePositionCableEndGrab;
    private bool isSolderingIronGrapped = false;

    private bool isBeingGrapped = false;
    private Transform grabbedTransform;

    public Transform cableCylindre;
    public float solderingIronRange = 50f;

    private float maxLength = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        cableEnd.selectEntered.AddListener(OnGrab);
        cableEnd.selectExited.AddListener(OnRelease);

        basePositionCableEndGrab = cableEnd.transform.position;
        
        maxLength = Vector3.Distance(targetEndCable.transform.position, transform.position) * 1.5f;

        UpdateCable(cableStart.position, positionEndOrigin.position);
    }

    // Update is called once per frame
    void Update()
    {

        if(cableEnd != null) {   
            if(isBeingGrapped && grabbedTransform != null) {
                Vector3 endPosition = grabbedTransform.position;
                cableEnd.transform.position = endPosition;

                UpdateCable(cableStart.position, endPosition);
            }

            if(Vector3.Distance(cableStart.transform.position, cableEnd.transform.position) > maxLength) {
                Release();
                UpdateCable(cableStart.position, positionEndOrigin.position);
                cableEnd.transform.position = basePositionCableEndGrab;
            }
        }
    }

    void OnGrab(SelectEnterEventArgs e)
    {
        isBeingGrapped = true;
        grabbedTransform = e.interactorObject.transform;

        Debug.Log("Im grabbed");
    }

    private void Release() {
        isBeingGrapped = false;
        grabbedTransform = null;

        Debug.Log("Im released");
    }

    void OnRelease(SelectExitEventArgs e)
    {
        if(isBeingGrapped) {
            Release();
            CheckForTarget();
        }
    }

    void OnSolderGrab(SelectEnterEventArgs e)
    {
        isSolderingIronGrapped = true;
        Debug.Log("Iron is grabbed");
    }

    void OnSolderRelease(SelectExitEventArgs e)
    {
        isSolderingIronGrapped = false;
        Debug.Log("Iron is released");
    }

    void UpdateCable(Vector3 start, Vector3 end)
    {
        Vector3 midPoint = (start + end) / 2;
        cableCylindre.position = midPoint;

        float distance = Vector3.Distance(start, end);
        cableCylindre.localScale = new Vector3(cableCylindre.localScale.x, distance / 2, cableCylindre.localScale.z);

        Vector3 direction = end - start;
        cableCylindre.up = direction.normalized;
    }

    bool isSolderingIronNearCableAndPressed(Vector3 vector)
    {

        GameObject[] solderingIrons = GameObject.FindGameObjectsWithTag("SolderingIron");
        foreach(GameObject iron in solderingIrons) {

            SoldringIron sd = iron.GetComponent<SoldringIron>();
            float distance = Vector3.Distance(iron.transform.position, vector);
            if(distance <= solderingIronRange && sd.getIsPressed() ) {
                return true;
            }
        }
        return false;
    }
    void CheckForTarget()
    {
        if(targetEndCable != null) {
            Vector3 targetPosition = targetEndCable.position;

            if (isSolderingIronNearCableAndPressed(targetPosition) && isSolderingIronGrapped)
            {
                UpdateCable(cableStart.position, targetPosition);
                cableEnd.transform.position = basePositionCableEndGrab;

                EndCableEtirable endCableEtirable = targetEndCable.GetComponent<EndCableEtirable>();

                endCableEtirable.Hide();
                
                if (fils != null)
                {
                    fils.SetActive(false);
                }
                Destroy(cableEnd);
                cableEnd = null;
                
            }
            else
            {
                UpdateCable(cableStart.position, positionEndOrigin.position);
                cableEnd.transform.position = basePositionCableEndGrab;

                EndCableEtirable endCableEtirable = targetEndCable.GetComponent<EndCableEtirable>();

                endCableEtirable.Show();

                if (fils != null)
                {
                    fils.SetActive(true);
                }
            }
        }
        else {
            UpdateCable(cableStart.position, positionEndOrigin.position);
            cableEnd.transform.position = basePositionCableEndGrab;
            Debug.LogWarning("No target to assigned");
        }
    }
}
