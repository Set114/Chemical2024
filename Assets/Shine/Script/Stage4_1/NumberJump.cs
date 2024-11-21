using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class NumberJump : MonoBehaviour
{
    public TextMeshProUGUI TempText;
    int Temp=25;
    public Material CarbonMaterial1, CarbonMaterial2, metal;
    int ColorTemp=255;
    // Start is called before the first frame update
 

    public void StartAddTemp() {
        CarbonMaterial1.color = new Color32(255, 255, 255, 255);
        CarbonMaterial2.color = new Color32(255, 255,255, 255);
        metal.color = new Color32(255, 255, 255, 255);
        InvokeRepeating("AddTemp", 0f, 0.1f);
    }
    void AddTemp() {
        if (Temp < 300&&FindObjectOfType<Level4_1>().ClickHeaterObj)
        {
            Temp += 25;
            TempText.text = "Temp." + Temp;
            ColorTemp -= Temp;
            CarbonMaterial1.color = new Color32(255, (byte)ColorTemp, (byte)ColorTemp, 255);
            CarbonMaterial2.color = new Color32(255, (byte)ColorTemp, (byte)ColorTemp, 255);
            metal.color = new Color32(252, (byte)ColorTemp, (byte)ColorTemp, 255);

        }
        if (Temp >= 300) {
            CarbonMaterial1.color = new Color32(255,0, 0, 255);
            CarbonMaterial2.color = new Color32(255,0, 0, 255);
            metal.color = new Color32(252, 58, 58, 255);

        }
        if (!FindObjectOfType<Level4_1>().ClickHeaterObj)
        {
            CancelInvoke("AddTemp");
            Clear();
        }
    }
    public void Clear() {
        Temp = 25;
        TempText.text = "";
        CarbonMaterial1.color = new Color32(255, 255, 255, 255);
        CarbonMaterial2.color = new Color32(255, 255, 255, 255);
        metal.color = new Color32(255, 255, 255, 255);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
