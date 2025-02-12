using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial_5_3 : MonoBehaviour
{
    [Header("長條圖")]
    [Tooltip("長條1")]
    [SerializeField] private Transform barA;
    [Tooltip("長條2")]
    [SerializeField] private Transform barB;
    [Tooltip("長條長度比")]
    [SerializeField] private float barOffset = 1.0f;
    private float scaleX;
    [Tooltip("蒸發速率數值文字")]
    [SerializeField] private Text textA;
    [Tooltip("凝結速率數值文字")]
    [SerializeField] private Text textB;
    [Tooltip("溫度數值文字")]
    [SerializeField] private Text temperatureText;
    [Tooltip("濃度數值文字")]
    [SerializeField] private Text concentrationText;

    [Header("數值")]
    [Tooltip("目前蒸發速率")]
    [SerializeField] private float valueA;
    [Tooltip("每階段的蒸發速率")]
    [SerializeField] private float[] valuesA;
    [Tooltip("目前凝結速率")]
    [SerializeField] private float valueB;
    [Tooltip("每階段的凝結速率")]
    [SerializeField] private float[] valuesB;
    [Tooltip("倒入葡萄糖速度")]
    [SerializeField] private float glucosePourSpeed = 1f;
    [Tooltip("燒杯內的葡萄糖的融化速度")]
    [SerializeField] private float glucoseMeltingSpeed = 0.2f;
    [Tooltip("燒杯內的葡萄糖的最後剩餘比例")]
    [SerializeField] private float glucoseUnmelting = 0.2f;
    [Tooltip("目前溫度")]
    [SerializeField] private float temperature = 20f;
    [Tooltip("每階段的溫度")]
    [SerializeField] private float[] temperatures;
    [Tooltip("目前濃度")]
    [SerializeField] private float concentration = 0f;
    [Tooltip("每階段的濃度")]
    [SerializeField] private float[] concentrations;

    [Space]
    [Tooltip("盤子")]
    [SerializeField] private GameObject plate;
    [Tooltip("盤子內的葡萄糖")]
    [SerializeField] private GameObject glucosePowder;
    [Tooltip("葡萄糖粉效果")]
    [SerializeField] private GameObject particleSystem_glucose;
    [Tooltip("燒杯內的葡萄糖")]
    [SerializeField] private GameObject glucosePowderInWater;
    
    float glucosePowderInWaterAlready;   //已經放入水裡的葡萄糖
    float glucosePowderInWaterCurrent;   //目前水裡的葡萄糖
    float glucosePowderInWaterMeltingAlready;   //已經融化的葡萄糖

    public int Status = 0;

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private MoleculaDisplay moleculaManager;    //管理分子螢幕
    private HintManager hintManager;            //管理提示板

    private MouseController pcController;
    private bool isPC;

    // Start is called before the first frame update
    void Start()
    {
        //判斷平台
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        isPC = true;
        pcController = FindObjectOfType<MouseController>();
#else
        isPC = false;
#endif
        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T5_3_1");
        scaleX = barA.localScale.x;
        valueA = valuesA[0];
        valueB = valuesB[0];
        temperature = temperatures[0];
        concentration = concentrations[0];
        glucosePowderInWater.transform.localScale = new Vector3(0f, 0f, 0f);

        barA.localScale = new Vector3(scaleX, valueA * barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";

        temperatureText.text = temperature.ToString("0.0");
        concentrationText.text = concentration.ToString("0.0");

        glucosePowderInWaterAlready = glucosePowderInWater.transform.localScale.x;
        glucosePowderInWaterCurrent = glucosePowderInWaterAlready;
        glucosePowderInWaterMeltingAlready = 0.0f;
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //等待葡萄糖倒入
                glucosePowderInWaterCurrent -= Time.deltaTime * glucoseMeltingSpeed;  //葡萄糖融化
                //計算最小值，最後留指定比例作為沉澱物
                glucosePowderInWaterCurrent = Mathf.Clamp(glucosePowderInWaterCurrent, glucosePowderInWaterAlready * glucoseUnmelting, 1f);
                //紀錄已融化的葡萄糖
                glucosePowderInWaterMeltingAlready = glucosePowderInWaterAlready - glucosePowderInWaterCurrent;
                //依據現有葡萄糖來設定沉澱物
                glucosePowderInWater.transform.localScale = Vector3.one * glucosePowderInWaterCurrent;
                //濃度計算
                concentration = Mathf.Lerp(concentrations[0], concentrations[1], glucosePowderInWaterMeltingAlready / (1.0f - glucoseUnmelting));
                // 使用 Lerp 計算新的數值
                valueA = Mathf.Lerp(valuesA[1], valuesA[2], GetValueA(glucosePowderInWaterMeltingAlready / (1.0f - glucoseUnmelting)));
                valueB = Mathf.Lerp(valuesB[1], valuesB[2], GetValueB(glucosePowderInWaterMeltingAlready / (1.0f - glucoseUnmelting)));
                break;
            case 1: //數值緩慢下降
                glucosePowderInWaterCurrent -= Time.deltaTime * glucoseMeltingSpeed;  //葡萄糖融化
                //計算最小值，最後留指定比例作為沉澱物
                glucosePowderInWaterCurrent = Mathf.Clamp(glucosePowderInWaterCurrent, glucosePowderInWaterAlready * glucoseUnmelting, 1f);
                //紀錄已融化的葡萄糖
                glucosePowderInWaterMeltingAlready = glucosePowderInWaterAlready - glucosePowderInWaterCurrent;
                //依據現有葡萄糖來設定沉澱物
                glucosePowderInWater.transform.localScale = Vector3.one * glucosePowderInWaterCurrent;
                //濃度計算
                concentration = Mathf.Lerp(concentrations[0], concentrations[1], glucosePowderInWaterMeltingAlready / (1.0f - glucoseUnmelting));
                // 使用 Lerp 計算新的數值
                valueA = Mathf.Lerp(valuesA[1], valuesA[2], GetValueA(glucosePowderInWaterMeltingAlready / (1.0f - glucoseUnmelting)));
                valueB = Mathf.Lerp(valuesB[1], valuesB[2], GetValueB(glucosePowderInWaterMeltingAlready / (1.0f - glucoseUnmelting)));

                if ( (glucosePowderInWaterMeltingAlready >= (1.0f - glucoseUnmelting)) && glucosePowderInWaterAlready == 1.0f )
                {
                    
                    valueA = valuesA[2];
                    valueB = valuesB[2];
                    concentration = concentrations[1];
                    hintManager.SwitchStep("T5_3_2");
                    hintManager.ShowNextButton(gameObject);
                    Status++;
                }
                break;
            case 2: //溶液達飽和

                break;
        }

        barA.localScale = new Vector3(scaleX, valueA * barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";

        temperatureText.text = temperature.ToString("0.0");
        concentrationText.text = concentration.ToString("0.0");
    }

    public void PourGlucosePowder()
    {
        if (Status == 0)
        {
            glucosePowderInWaterAlready += Time.deltaTime * glucosePourSpeed;
            glucosePowderInWaterCurrent += Time.deltaTime * glucosePourSpeed;
            glucosePowderInWaterAlready = Mathf.Clamp(glucosePowderInWaterAlready, 0f, 1f);
            glucosePowderInWaterCurrent = Mathf.Clamp(glucosePowderInWaterCurrent, 0f, 1f);

            if (glucosePowderInWaterAlready >= 1f)
            {
                glucosePowder.SetActive(false);
                glucosePowderInWater.SetActive(true);
                particleSystem_glucose.SetActive(false);
                moleculaManager.ShowMoleculas(2);
                moleculaManager.PlayMoleculasAnimation();

                if (isPC)
                    pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                plate.SendMessage("Return", SendMessageOptions.DontRequireReceiver);
                plate.GetComponent<XRGrabInteractable>().enabled = false;
                plate.tag = "Untagged";
                Status++;
            }
        }
    }

    /*
    public void PourGlucosePowder()
    {
        if (Status == 0)
        {
            float size = glucosePowderInWater.transform.localScale.x;
            size += Time.deltaTime * glucosePourSpeed;
            size = Mathf.Clamp(size, 0f, 1f);
            // 使用 Lerp 計算新的數值
            valueA = Mathf.Lerp(valuesA[0], valuesA[1], size);
            valueB = Mathf.Lerp(valuesB[0], valuesB[1], size);
            concentration = Mathf.Lerp(concentrations[0], concentrations[1], size);

            glucosePowderInWater.transform.localScale = new Vector3(size, size, size);
            if (size >= 1f)
            {
                valueA = valuesA[1];
                valueB = valuesB[1];
                concentration = concentrations[1];
                glucosePowder.SetActive(false);
                glucosePowderInWater.SetActive(true);
                particleSystem_glucose.SetActive(false);
                timer = 0f;
                moleculaManager.ShowMoleculas(2);
                moleculaManager.PlayMoleculasAnimation();
                Status++;
            }
        }
    }
    */

    float GetValueA(float percentage)
    {
        float x = Mathf.Clamp01(percentage); // 確保比例範圍在 0-1 之間
        float k = 10.0f; // 曲線陡峭程度
        return 1.0f / (1.0f + Mathf.Exp(-k * (x - 0.5f))); // Sigmoid 曲線
    }

    float GetValueB(float percentage)
    {
        float x = Mathf.Clamp01(percentage); // 確保比例範圍在 0-1 之間

        if (x <= 0.3f)
        {
            // 前段：緩慢上升
            return Mathf.Pow(x / 0.3f, 0.5f) * 0.3f; // 平方根曲線
        }
        else if (x <= 0.8f)
        {
            // 中段：線性上升
            return 0.3f + (x - 0.3f) * 1.4f; // 斜率為 1.4
        }
        else
        {
            // 後段：逐漸放緩
            return 0.95f + (x - 0.8f) * 0.25f; // 緩慢趨近 1.0
        }
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
