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
    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "C":
            case "O2":
            case "N2":
            case "H":
            case "Fe":
                tutorialObject.Reaction(area.ToString(), other.gameObject.name, true);
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                break;
        }
    }
    void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "C":
            case "O2":
            case "N2":
            case "H":
            case "Fe":
                tutorialObject.Reaction(area.ToString(), other.gameObject.name, false);
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                break;
        }
    }
}