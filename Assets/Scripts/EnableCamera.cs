using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCamera : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    private void OnTriggerEnter(Collider other)
    {
        cam1.SetActive(false);
        cam2.SetActive(true);
        Destroy(this);
    }
}
