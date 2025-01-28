using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_5_1 : MonoBehaviour
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
    [Tooltip("蒸發速率")]
    [SerializeField] private float valueA;
    [Tooltip("凝結速率")]
    [SerializeField] private float valueB;

    int Status = 0;                 //狀態值
    float nextStepTimer = 0.0f;     //切換到完結的時間

    [Space]
    [Tooltip("互動按鈕")]
    [SerializeField] private GameObject questionMark;

    private LevelObjManager levelObjManager;
    private MoleculaDisplay moleculaManager;    //管理分子螢幕
    private HintManager hintManager;            //管理提示板


    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T5_1_1");
        
        questionMark.SetActive(true);
        scaleX = barA.localScale.x;

        barA.localScale = new Vector3(scaleX, valueA* barOffset, scaleX);
        barB.localScale = new Vector3(scaleX, valueB* barOffset, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //待機
                break;
            case 1: //計時中 
                if (Time.time > nextStepTimer)
                {
                    hintManager.SwitchStep("T5_1_2");
                    hintManager.ShowNextButton(gameObject);
                    Status++;
                }
                break;
            case 2: //完結
                break;
        }        
    }

    public void ShowMoleculas()
    {
        moleculaManager.ShowMoleculas(0);
        moleculaManager.PlayMoleculasAnimation();
        nextStepTimer = Time.time + 3.0f;        
        questionMark.SetActive(false);
        Status = 1;
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
