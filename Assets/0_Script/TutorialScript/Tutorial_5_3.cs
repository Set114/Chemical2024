using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_5_3 : MonoBehaviour
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
    [Tooltip("反應時間")]
    [SerializeField] private float reactionTime = 0f;
    [Tooltip("倒入葡萄糖速度")]
    [SerializeField] private float glucosePourSpeed = 1f;
    [Tooltip("目前溫度")]
    [SerializeField] private float temperature = 20f;
    [Tooltip("每階段的溫度")]
    [SerializeField] private float[] temperatures;
    [Tooltip("目前濃度")]
    [SerializeField] private float concentration = 0f;
    [Tooltip("每階段的濃度")]
    [SerializeField] private float[] concentrations;

    [Space]
    [Tooltip("盤子內的葡萄糖")]
    [SerializeField] private GameObject glucosePowder;
    [Tooltip("葡萄糖粉效果")]
    [SerializeField] private GameObject particleSystem_glucose;
    [Tooltip("燒杯內的葡萄糖")]
    [SerializeField] private GameObject glucosePowderInWater;

    private float timer = 0;
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private MoleculaDisplay moleculaManager;    //管理分子螢幕
    private HintManager hintManager;            //管理提示板


    // Start is called before the first frame update
    void Start()
    {
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
    }

    private void Update()
    {
        float t = Mathf.Clamp01(timer / reactionTime);
        switch (Status)
        {
            case 0: //等待葡萄糖倒入
                break;
            case 1: //數值緩慢下降
                // 計算經過的時間
                timer += Time.deltaTime;
                // 使用 Lerp 計算新的數值
                valueA = Mathf.Lerp(valuesA[1], valuesA[2], t);
                valueB = Mathf.Lerp(valuesB[1], valuesB[2], t);

                if (timer >= reactionTime)
                {
                    valueA = valuesA[2];
                    valueB = valuesB[2];
                    hintManager.SwitchStep("T5_3_2");
                    hintManager.ShowNextButton(gameObject);
                    Status++;
                }
                break;
            case 2: //溶液達飽和

                break;
        }

        barA.localScale = new Vector3(scaleX, valueA * 0.002f, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * 0.002f, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
        temperatureText.text = temperature.ToString("0");
        concentrationText.text = concentration.ToString("0");
    }

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

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
