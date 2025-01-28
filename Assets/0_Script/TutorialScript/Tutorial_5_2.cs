using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_5_2 : MonoBehaviour
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

    [Space]
    [Tooltip("互動按鈕")]
    [SerializeField] private GameObject questionMark;
    private float timer = 0;
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private MoleculaDisplay moleculaManager;    //管理分子螢幕
    private HintManager hintManager;            //管理提示板

    private bool isPC;                          //偵測是否是PC模式

    // Start is called before the first frame update
    void Start()
    {
        //判斷平台
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        isPC = true;
#else
        isPC = false;
#endif

        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T5_2_1");
        moleculaManager.ShowMoleculas(1);
        questionMark.SetActive(false);
        scaleX = barA.localScale.x;
        valueA = valuesA[0];
        valueB = valuesB[0];

        barA.localScale = new Vector3(scaleX, valueA * barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //等待打開瓶蓋
                break;
            case 1: //反應中
                // 計算經過的時間
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / reactionTime);
                // 使用 Lerp 計算新的數值
                valueA = Mathf.Lerp(valuesA[0], valuesA[1], t);
                valueB = Mathf.Lerp(valuesB[0], valuesB[1], t);
                if (timer >= reactionTime)
                {
                    questionMark.SetActive(true);
                    Status++;
                }
                break;
            case 2: //反應完
                break;
        }

        barA.localScale = new Vector3(scaleX, valueA * barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB * barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
    }

    public void ReactionExit(GameObject sender)
    {
        if (sender.name == "Cap_5-2")
        {
            //打開瓶蓋
            if(!isPC)
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
