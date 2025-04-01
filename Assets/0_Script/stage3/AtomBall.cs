using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AtomBall : MonoBehaviour
{
    public bool isGrabbing = false;
    public bool isUsing = false;
    private bool firstGrab = true;
    private Tutorial_3_1 tutorialObject;
    private Rigidbody myRb;

    private void Start()
    {
        tutorialObject = FindObjectOfType<Tutorial_3_1>();
        myRb= GetComponent<Rigidbody>();
        myRb.constraints = RigidbodyConstraints.FreezeAll;
    }

    //抓取或鬆開時觸發
    public void Grab(bool isGrab)
    {
        if (isGrab)
        {
            myRb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            myRb.constraints = RigidbodyConstraints.None;
        }

        isGrabbing = isGrab;
        if (isGrab && firstGrab)
        {
            tutorialObject.UseAtom(gameObject.name);
            firstGrab = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TrashBin"))
        {
            tutorialObject.AtomReturn(gameObject.name);
            Destroy(gameObject);
        }
    }
}
