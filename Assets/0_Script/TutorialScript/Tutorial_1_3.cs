using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_1_3 : MonoBehaviour
{
    [SerializeField] private Transform cupRim;         //杯口
    [SerializeField] private Transform iron;

    [Tooltip("靠近杯口發出警示的距離")]
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float maxDistance = 0.75f;
    private float distance;
    private bool near = false;
    private bool firstTimeWarning = true;              // 第一次抓取危險物品的通知
    private bool isEnd = false;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板
    private MoleculaDisplay moleculaManager;    //管理分子螢幕

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_3_1");

        moleculaManager.ShowMoleculas(2);
    }

    private void Update()
    {
        if (isEnd)
            return;

        distance = Vector3.Distance(iron.position, cupRim.position);

        if (distance < minDistance&&!near)
        {
            near = true;
            NearToHCI();
        }
        else if (distance > maxDistance)
        {
            near = false;
        }
    }

    // 靠近鹽酸
    private void NearToHCI()
    {
        if (firstTimeWarning)
        {
            hintManager.PlayWarningHint("W_HCI");
            firstTimeWarning = false;
        }
    }

    public void ChangeColor()
    {
        //粒子視窗動畫
        moleculaManager.PlayMoleculasAnimation();
    }

    public void EndTheTutorial()   
    {
        hintManager.SwitchStep("T1_3_2");
        hintManager.showNextButton(this.gameObject);
        isEnd = true;
    }
    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
