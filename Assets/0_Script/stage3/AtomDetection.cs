using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomDetection : MonoBehaviour
{
    [SerializeField] private Area area;
    private enum Area
    {
        A, B, C, D
    }
    private Tutorial_3_1 tutorialObject;

    private void Start()
    {
        tutorialObject = FindObjectOfType<Tutorial_3_1>();
    }

    private void OnTriggerStay(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom)
        {
            if (!atom.isGrabbing && !atom.isUsing)
            {
                atom.isUsing = true;
                tutorialObject.Reaction(area.ToString(), other.gameObject.name, true);
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        AtomBall atom = other.GetComponent<AtomBall>();
        if (atom && atom.isUsing)
        {
            tutorialObject.Reaction(area.ToString(), atom.gameObject.name, false);
            //atom.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom.isUsing = false;
        }
    }
}