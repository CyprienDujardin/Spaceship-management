using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilFollow : MonoBehaviour
{
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.localScale = initialScale;
    }
}
