using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnToStartPoint : MonoBehaviour
{
    private Transform initialParent;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private XRGrabInteractable xrGrab;

    private void Awake()
    {
        initialParent = transform.parent;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        xrGrab = GetComponent<XRGrabInteractable>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //當物件掉到地上時
        if(collision.gameObject.name == "Ground")
        {
            Return();
        }
    }
    //回歸原位
    public void Return()
    {
        if (xrGrab)
        {
            xrGrab.enabled = false;
            xrGrab.enabled = true;
        }
        transform.SetParent(initialParent);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
