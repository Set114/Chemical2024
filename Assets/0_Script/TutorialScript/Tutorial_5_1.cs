using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_5_1 : MonoBehaviour
{
    [Header("長條圖")]
    [Tooltip("長條1")]
    [SerializeField] private Transform bar1;
    [Tooltip("長條2")]
    [SerializeField] private Transform bar2;
    [Tooltip("比例")]
    [SerializeField] private float ratio1, ratio2;
    private float scaleX;
    [SerializeField] private Text text1;
    [Tooltip("長條2")]
    [SerializeField] private Text text2;

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
        hintManager.SwitchStep("T5_1_1");
        scaleX = bar1.localScale.x;
    }

    private void Update()
    {
        bar1.localScale = new Vector3(scaleX, ratio1, scaleX);
        bar2.localScale = new Vector3(scaleX, ratio2, scaleX);
        text1.text = ratio1.ToString("0.00") + "mg/s";
        text2.text = ratio2.ToString("0.00") + "mg/s";
    }

    public void ShowMoleculas()
    {
        moleculaManager.ShowMoleculas(0);
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
