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
    [SerializeField] private float[] reactionTime;

    private float timer = 0;
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板

    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T5_4_1");
        scaleX = barA.localScale.x;
        valueA = valuesA[0];
        valueB = valuesB[0];
        temperature = temperatures[0];
        concentration = concentrations[0];
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //等待水倒入
                break;
            case 1: //濃度緩慢逐漸上升
                    // 計算經過的時間
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / reactionTime[1]);
                // 使用 Lerp 計算新的數值
                concentration = Mathf.Lerp(concentrations[1], concentrations[2], t);
                if (timer >= reactionTime[1])
                {
                    concentration = concentrations[2];
                    hintManager.SwitchStep("T5_5_2");
                    hintManager.ShowNextButton(gameObject);
                    timer = 0f;
                    Status++;
                }
                break;
        }

        barA.localScale = new Vector3(scaleX, valueA * 0.002f, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * 0.002f, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
        temperatureText.text = temperature.ToString("0");
        concentrationText.text = concentration.ToString("0");
    }

    public void WaterPourIn()
    {
        if (Status == 0)
        {
            // 計算經過的時間
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / reactionTime[0]);
            // 使用 Lerp 計算新的數值
            valueB = Mathf.Lerp(valuesB[0], valuesB[1], t);
            concentration = Mathf.Lerp(concentrations[0], concentrations[1], t);
            if (timer >= reactionTime[0])
            {
                valueB = valuesB[1];
                concentration = concentrations[1];
                timer = 0f;
                Status++;
            }
        }
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(1);
    }
}
