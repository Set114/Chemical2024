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
        moleculaManager.ShowMoleculas(0);
        questionMark.SetActive(true);
        scaleX = barA.localScale.x;
    }

    private void Update()
    {
        barA.localScale = new Vector3(scaleX, valueA, scaleX);
        barB.localScale = new Vector3(scaleX, valueB, scaleX);
        textA.text = valueA.ToString("0.00") + "mg/s";
        textB.text = valueB.ToString("0.00") + "mg/s";
    }

    public void ShowMoleculas()
    {
        moleculaManager.PlayMoleculasAnimation();
        hintManager.SwitchStep("T5_1_2");
        hintManager.ShowNextButton(gameObject);
        questionMark.SetActive(false);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
