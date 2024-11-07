using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{
    public GameObject hint1;
    public GameObject hint2;

    public void show1()
    {
        hint1.SetActive(true);
        
    }

    public void show2()
    {
        hint2.SetActive(true);
    }
}

