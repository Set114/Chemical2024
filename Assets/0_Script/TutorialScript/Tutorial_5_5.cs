using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_5_5 : MonoBehaviour
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
    [Tooltip("目前溫度")]
    [SerializeField] private float temperature = 20f;
    [Tooltip("每階段的溫度")]
    [SerializeField] private float[] temperatures;
    [Tooltip("目前濃度")]
    [SerializeField] private float concentration = 0f;
    [Tooltip("每階段的濃度")]
    [SerializeField] private float[] concentrations;
    [Tooltip("反應時間")]
    [SerializeField] private float[] reactionTime;  //[0] 灌入水的時間 [1]第一階段的時間 [2]第二階段的時間
    [Tooltip("水容量控制-燒杯")]
    [SerializeField] LiquidController Water;
    [Tooltip("水容量控制-倒入的水杯")]
    [SerializeField] LiquidController Water100ml;
    [Tooltip("燒杯內的葡萄糖")]
    [SerializeField] private GameObject glucosePowderInWater;
    [Tooltip("燒杯內的葡萄糖的初始比例")]
    [SerializeField] private float glucosePowderInitial = 0.4f;
    [Tooltip("燒杯內的葡萄糖的最終比例")]
    [SerializeField] private float glucosePowderFinal = 0.2f;

    private float timer = 0;
    public int Status = 0;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板

    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T5_5_1");
        scaleX = barA.localScale.x;
        valueA = valuesA[0];
        valueB = valuesB[0];
        temperature = temperatures[0];
        concentration = concentrations[0];

        glucosePowderInWater.transform.localScale = Vector3.one * glucosePowderInitial;

        barA.localScale = new Vector3(scaleX, valueA * barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
        temperatureText.text = temperature.ToString("0.0");
        concentrationText.text = concentration.ToString("0.0");
    }

    private void Update()
    {
        float t;
        switch (Status)
        {
            case 0: //等待水倒入
                break;
            case 1: //加水後，葡萄糖水濃度瞬間下降(由47%降至36%)
                // 計算經過的時間
                timer += Time.deltaTime;
                t = Mathf.Clamp01(timer / reactionTime[1]);
                // 使用 Lerp 計算新的數值
                valueB = Mathf.Lerp(valuesB[0], valuesB[1], t);
                concentration = Mathf.Lerp(concentrations[0], concentrations[1], t);
                if (timer >= reactionTime[1])
                {
                    valueB = valuesB[1];
                    concentration = concentrations[1];
                    hintManager.ShowNextButton(gameObject);
                    timer = 0f;
                    Status++;
                }
                break;
            case 2://濃度緩慢逐漸上升
                // 計算經過的時間
                timer += Time.deltaTime;
                t = Mathf.Clamp01(timer / reactionTime[2]);
                // 使用 Lerp 計算新的數值
                concentration = Mathf.Lerp(concentrations[1], concentrations[2], t);
                glucosePowderInWater.transform.localScale = Vector3.one * Mathf.Lerp(glucosePowderInitial, glucosePowderFinal, GetValueA(t));
                if (timer >= reactionTime[2])
                {
                    concentration = concentrations[2];
                    hintManager.SwitchStep("T5_5_3");
                    hintManager.ShowNextButton(gameObject);
                    timer = 0f;
                    Status++;
                }
                break;
        }

        barA.localScale = new Vector3(scaleX, valueA * barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
        temperatureText.text = temperature.ToString("0.0");
        concentrationText.text = concentration.ToString("0.0");
    }

    public void WaterPourIn()
    {
        if (Status == 0)
        {
            // 計算經過的時間
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / reactionTime[0]);
            //變更水位
            Water.currCapacity = Mathf.Lerp( 300f, 400f, t);

            if (timer >= reactionTime[0])
            {
                hintManager.SwitchStep("T5_5_2");
                Water.currCapacity = 400f;
                Water100ml.currCapacity = 0f;
                timer = 0f;
                Status++;
            }
        }
    }

    float GetValueA(float percentage)
    {
        float x = Mathf.Clamp01(percentage); // 確保比例範圍在 0-1 之間
        float k = 10.0f; // 曲線陡峭程度
        return 1.0f / (1.0f + Mathf.Exp(-k * (x - 0.5f))); // Sigmoid 曲線
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(1);
    }
}
