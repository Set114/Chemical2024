using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class part0run: MonoBehaviour
{
    public GameObject part0; 
    public GameObject part1;
    public GameObject p0run1;

    public void OnOkClicked()
    {
        part0.SetActive(false);
        p0run1.SetActive(false);
        part1.SetActive(true);
    
    }
}

