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

    public GameObject ObjTips;

    public GameObject ChemicaObjHot, ChemicaObjCold;
    public GameObject ChemicaImageHot, ChemicaImageCold;

    public Material CarbonMaterial1;
    public GameObject FinishedUI;
    public Sprite FinishedSprite;
    //溫度跳動
    int startValue =0;
     int endValue = 0;
    public float duration = 1.5f; // 數字變動時間（秒）

    // Start is called before the first frame update
    void AWake()
    {
        FindObjectOfType<Shine_GM>().StartTimes6_L[1] = System.DateTime.Now.ToString();
        Info[0].SetActive(true);
        OriginalTemp();
    }
    public void OriginalTemp()
    {
        CarbonMaterial1.color = new Color32(255, 255, 255, 255);
        //TempText.text = 25 + "<sup>o</sup>C";
        if (startValue == 25)
        {
            startValue = 100;
            endValue = 25;
            StartCounting();
        }
    }
    public void HotTemp()
    {
        CarbonMaterial1.color = new Color32(255, 0, 0, 255);
        //TempText.text = 100 + "<sup>o</sup>C";
        startValue = 25;
        endValue = 100;
         StartCounting();
    }
    public void StartCounting()
    {
        StartCoroutine(CountToTarget());
    }

    IEnumerator CountToTarget()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));

            if (TempText != null)
                TempText.text = currentValue.ToString() + "<sup>o</sup>C";



            yield return null;
        }
            TempText.text = endValue.ToString() + "<sup>o</sup>C";

    }
    // Update is called once per frame
    void Update()
    {
        
    
   }
    //開啟通氣閥
    public void Open() {
        if (!YallowLight.active)
        {
            YallowLight.SetActive(true);
            ChemicaObj[0].SetActive(true);
            ChemicaImage[0].SetActive(true);


        }
    }
    //加溫
    public void AddTemp() {
        if (!Final.active)
        {
            i = 1;
            Info[0].SetActive(false);
            YallowLight.GetComponent<Light>().color = Color.red;
            ChemicaImage[0].SetActive(false);
            ChemicaObj[0].SetActive(false);

            Destroy(ChemicaObjHot);
            Destroy(ChemicaObjCold);
            Destroy(ChemicaImageHot);
            Destroy(ChemicaImageCold);

            ChemicaObjHot = Instantiate(ChemicaObj[1]) as GameObject;
            ChemicaObjHot.transform.parent = ChemicaObj[0].transform.parent;
            ChemicaObjHot.transform.localScale = Vector3.one;
            ChemicaObjHot.transform.localRotation = ChemicaObj[0].transform.localRotation;
            ChemicaObjHot.transform.localPosition = ChemicaObj[0].transform.localPosition;
            ChemicaObjHot.SetActive(true);

            ChemicaImageHot = Instantiate(ChemicaImage[1]) as GameObject;
            ChemicaImageHot.transform.parent = ChemicaImage[0].transform.parent;
            ChemicaImageHot.transform.localScale = Vector3.one;
            ChemicaImageHot.transform.localRotation = ChemicaImage[0].transform.localRotation;
            ChemicaImageHot.transform.localPosition = ChemicaImage[0].transform.localPosition;
            ChemicaImageHot.SetActive(true);

            //ChemicaObj[1].SetActive(true);
            //ChemicaImage[1].SetActive(true);
            HotTemp();
        }
    }
    //降溫
    public void ReduceTemp()
    {
        if (!Final.active)
        {
            i = 2;
            //i = Mathf.Clamp(i, 0, 2);
            Info[0].SetActive(false);
            Info[1].SetActive(true);

            YallowLight.GetComponent<Light>().color = Color.yellow;
            ChemicaImage[0].SetActive(false);
            ChemicaObj[0].SetActive(false);

            Destroy(ChemicaObjHot);
            Destroy(ChemicaObjCold);
            Destroy(ChemicaImageHot);
            Destroy(ChemicaImageCold);

            ChemicaObjCold = Instantiate(ChemicaObj[2]) as GameObject;
            ChemicaObjCold.transform.parent = ChemicaObj[0].transform.parent;
            ChemicaObjCold.transform.localScale = Vector3.one;
            ChemicaObjCold.transform.localRotation = ChemicaObj[0].transform.localRotation;
            ChemicaObjCold.transform.localPosition = ChemicaObj[0].transform.localPosition;
            ChemicaObjCold.SetActive(true);

            ChemicaImageCold = Instantiate(ChemicaImage[2]) as GameObject;
            ChemicaImageCold.transform.parent = ChemicaImage[0].transform.parent;
            ChemicaImageCold.transform.localScale = Vector3.one;
            ChemicaImageCold.transform.localRotation = ChemicaImage[0].transform.localRotation;
            ChemicaImageCold.transform.localPosition = ChemicaImage[0].transform.localPosition;
            ChemicaImageCold.SetActive(true);

            OriginalTemp();
            StartCoroutine(FinalCheck());
        }
/*        if (i == 0)
        {
            Info[1].SetActive(true);
            StartCoroutine(FinalCheck());
        }
*/
    }
    IEnumerator FinalCheck()
    {
        yield return new WaitForSeconds(3f);
        MinTemp = true;

       // if (MinTemp && MaxTemp)
        //{
            Final.SetActive(true);
            FindObjectOfType<Shine_GM>().EndTimes6_L[1] = System.DateTime.Now.ToString();
        FinishedUI.GetComponent<Image>().sprite = FinishedSprite;

        //}
    }
    public void ReButton() {
        for (int k = 0; k < ChemicaObj.Length; k++) {
            ChemicaObj[k].SetActive(false);
            ChemicaImage[k].SetActive(false);
        }
        YallowLight.GetComponent<Light>().color = Color.yellow;
        YallowLight.SetActive(false);
        TempText.text = 25 + "C<sup>o</sup>";

        TempUI.SetActive(false);
        Info[0].SetActive(true);
        Info[1].SetActive(false);
        ObjTips.SetActive(true);
        Destroy(ChemicaObjHot);
        Destroy(ChemicaObjCold);
        Destroy(ChemicaImageHot);
        Destroy(ChemicaImageCold);
}
}
