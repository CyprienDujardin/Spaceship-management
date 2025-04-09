using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EndCableEtirable : MonoBehaviour
{

    public GameObject fils;


    public void Hide()
    {
        fils.SetActive(false);
    }

    public void Show()
    {
        fils.SetActive(true);
    }
}
