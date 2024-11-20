using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class NumberJump : MonoBehaviour
{
    public TextMeshProUGUI TempText;
    int Temp=0;
    public Material CarbonMaterial1, CarbonMaterial2;
    int ColorTemp=255;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartAddTemp() {
        CarbonMaterial1.color = new Color32(255, 255, 255, 255);
        CarbonMaterial2.color = new Color32(255, 255,255, 255);
        InvokeRepeating("AddTemp", 0f, 0.1f);
    }
    void AddTemp() {
        if (Temp < 300)
        {
            Temp += 25;
            TempText.text = "Temp." + Temp;
            ColorTemp -= Temp;
            CarbonMaterial1.color = new Color32(255, (byte)ColorTemp, (byte)ColorTemp, 255);
            CarbonMaterial2.color = new Color32(255, (byte)ColorTemp, (byte)ColorTemp, 255);
        }
        else{
            CancelInvoke("AddTemp");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
