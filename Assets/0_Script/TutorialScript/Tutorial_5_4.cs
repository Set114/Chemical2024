using UnityEngine;
using UnityEngine.UI;
public class Tutorial_5_4 : MonoBehaviour
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
    [SerializeField] private float reactionTime = 10f;

    [Space]
    [Tooltip("酒精燈蓋子")]
    [SerializeField] private Animator cap;
    [Tooltip("酒精燈火焰")]
    [SerializeField] private GameObject fire;
    [Tooltip("燒杯內的葡萄糖")]
    [SerializeField] private GameObject glucosePowderInWater;
    [Tooltip("燒杯內的葡萄糖的初始比例")]
    [SerializeField] private float glucosePowderInitial = 0.4f;
    [Tooltip("燒杯內的葡萄糖的最終比例")]
    [SerializeField] private float glucosePowderFinal = 0.1f;
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
        hintManager.SwitchStep("T5_4_1");
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
        moleculaManager.ShowMoleculas(3);
    }

    private void Update()
    {
        float t = Mathf.Clamp01(timer / reactionTime);
        switch (Status)
        {
            case 0: //等待酒精燈點燃
                break;
            case 1: //加熱中
                // 計算經過的時間
                timer += Time.deltaTime;
                // 使用 Lerp 計算新的數值
                valueA = Mathf.Lerp(valuesA[0], valuesA[1], GetValueA(t));
                valueB = Mathf.Lerp(valuesB[0], valuesB[1], GetValueA(t));
                temperature = Mathf.Lerp(temperatures[0], temperatures[1], t);
                concentration = Mathf.Lerp(concentrations[0], concentrations[1], t);
                glucosePowderInWater.transform.localScale = Vector3.one * Mathf.Lerp(glucosePowderInitial, glucosePowderFinal, GetValueA(t));
                if (timer >= reactionTime)
                {
                    valueA = valuesA[1];
                    valueB = valuesB[1];
                    temperature = temperatures[1];
                    concentration = concentrations[1];
                    hintManager.SwitchStep("T5_4_3");
                    hintManager.ShowNextButton(gameObject);
                    Status++;
                }
                break;
            case 2: //加熱完
                break;
        }

        barA.localScale = new Vector3(scaleX, valueA * barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
        temperatureText.text = temperature.ToString("0.0");
        concentrationText.text = concentration.ToString("0.0");
    }

    public void OnAlcoholLampTouched()
    {
        if (Status == 0)
        {
            audioManager.PlayVoice("W_AlcoholLamp");
            cap.SetBool("cover", true);
            fire.SetActive(true);
            hintManager.SwitchStep("T5_4_2");
            moleculaManager.PlayMoleculasAnimation();
            Status++;
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
        levelObjManager.LevelClear(0);
    }
}
