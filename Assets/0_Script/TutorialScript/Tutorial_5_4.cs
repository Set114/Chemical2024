using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_5_4 : MonoBehaviour
{
    [Header("長條圖")]
    [Tooltip("長條1")]
    [SerializeField] private Transform bar1;
    [Tooltip("長條2")]
    [SerializeField] private Transform bar2;
    private float scaleX;
    [Tooltip("蒸發速率數值文字")]
    [SerializeField] private Text text1;
    [Tooltip("凝結速率數值文字")]
    [SerializeField] private Text text2;

    [Header("數值")]
    [Tooltip("蒸發速率")]
    [SerializeField] private float evaporationRate;
    [Tooltip("每階段的蒸發速率")]
    [SerializeField] private float[] evaporationRates;
    [Tooltip("凝結速率")]
    [SerializeField] private float coagulationRate;
    [Tooltip("每階段的凝結速率")]
    [SerializeField] private float[] coagulationRates;
    [Tooltip("反應時間")]
    [SerializeField] private float reactionTime = 0f;

    [Space]
    [Tooltip("互動按鈕")]
    [SerializeField] private GameObject questionMark;
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
        hintManager.SwitchStep("T5_2_1");
        moleculaManager.ShowMoleculas(1);
        questionMark.SetActive(false);
        scaleX = bar1.localScale.x;
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //等待打開瓶蓋
                evaporationRate = evaporationRates[0];
                coagulationRate = coagulationRates[0];
                break;
            case 1: //反應中
                // 計算經過的時間
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / reactionTime);
                // 使用 Lerp 計算新的數值
                evaporationRate = Mathf.Lerp(evaporationRates[0], evaporationRates[1], t);
                coagulationRate = Mathf.Lerp(coagulationRates[0], coagulationRates[1], t);
                if (timer >= reactionTime)
                {
                    questionMark.SetActive(true);
                    Status++;
                }
                break;
            case 2: //反應完
                break;
        }

        bar1.localScale = new Vector3(scaleX, evaporationRate, scaleX);
        bar2.localScale = new Vector3(scaleX, coagulationRate, scaleX);
        text1.text = evaporationRate.ToString("0.00") + "mg/s";
        text2.text = coagulationRate.ToString("0.00") + "mg/s";
    }

    public void ReactionExit(GameObject sender)
    {
        if (sender.name == "Cap_5-2")
        {
            //打開瓶蓋
            sender.GetComponent<Rigidbody>().isKinematic = false;
            moleculaManager.PlayMoleculasAnimation();
            timer = 0;
            Status++;
        }
    }

    public void OnQuestionMarkClicked()
    {
        hintManager.SwitchStep("T5_2_2");
        hintManager.ShowNextButton(gameObject);
        questionMark.SetActive(false);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
