using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage6_2 : MonoBehaviour
{

    public GameObject[] ChemicaObj;
    public GameObject[] ChemicaImage;
    public GameObject YallowLight;
    public GameObject TempUI;
    public TMP_Text TempText;
    public int i;
    public bool MaxTemp, MinTemp;
    public GameObject Final;
    public GameObject[] Info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    
   }
    //開啟通氣閥
    public void Open() {
        YallowLight.SetActive(true);
        ChemicaObj[0].SetActive(true);
        ChemicaImage[0].SetActive(true);
    }
    //加溫
    public void AddTemp() {
        i++;
        i = Mathf.Clamp(i, 0, 2);
        Info[0].SetActive(false);
        YallowLight.GetComponent<Light>().color = Color.red;
        ChemicaImage[i-1].SetActive(false);
        ChemicaObj[i].SetActive(true);
        ChemicaImage[i].SetActive(true);
        TempText.text = "Temp:" + (i * 100);
        if (i == 2)
        {
            MaxTemp = true;
        }
    }
    //降溫
    public void ReduceTemp()
    {
        i--;
        i = Mathf.Clamp(i, 0, 2);
        Info[0].SetActive(false);
        YallowLight.GetComponent<Light>().color = Color.yellow;
        ChemicaImage[i+1].SetActive(false);
        ChemicaObj[i+1].SetActive(false);
        ChemicaImage[i].SetActive(true);
        TempText.text = "Temp:" + (i * 100);
        if (i == 0)
        {
            Info[1].SetActive(true);
            StartCoroutine(FinalCheck());
        }

    }
    IEnumerator FinalCheck()
    {
        yield return new WaitForSeconds(5f);
        MinTemp = true;

        if (MinTemp && MaxTemp)
        {
            Final.SetActive(true);
        }
    }
    public void ReButton() {
        for (int k = 0; k < ChemicaObj.Length; k++) {
            ChemicaObj[k].SetActive(false);
            ChemicaImage[k].SetActive(false);
        }
        YallowLight.GetComponent<Light>().color = Color.yellow;
        YallowLight.SetActive(false);
        TempUI.SetActive(false);
        Info[0].SetActive(false);
        Info[1].SetActive(false);

    }
}
