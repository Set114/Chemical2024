using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider Hit)
    {
        if (Hit.tag == "C") {
            FindObjectOfType<Level4_1>().CreateCO2(Hit.transform.position);
            transform.parent.parent.gameObject.SetActive(false);
            Hit.transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
