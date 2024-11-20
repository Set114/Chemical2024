using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourSolution : MonoBehaviour
{
    [SerializeField] GameObject Liquid;
    [SerializeField] GameObject putpoint;
    [SerializeField] GameObject TestTube;
    bool isputed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("mouth"))
        {
            Liquid.SetActive(true);
        }
        else if (other.gameObject.name.Equals("TestTube") && TestTube.transform.Find("Liquid").gameObject.activeSelf)
        {
            isputed = true;          
           
        }
    }

    void Update()
    {
        if (isputed)
        {
            TestTube.gameObject.transform.position = putpoint.transform.position;
            TestTube.gameObject.transform.rotation = putpoint.transform.rotation;
        }
    }

}
