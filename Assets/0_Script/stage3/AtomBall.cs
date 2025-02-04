using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomBall : MonoBehaviour
{
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
            if (firstGrab)
            {
                tutorialObject.UseAtom(gameObject.name);
                firstGrab = false;
            }
            //myRb.isKinematic = true;
        }
        else
        {
            //myRb.isKinematic = false;
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
