using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_2_1 : MonoBehaviour
{

    [SerializeField] private GameObject gasCan;         //噴槍
    [SerializeField] MeshRenderer[] woodPowders;        //木屑
    [SerializeField] MeshRenderer[] steelWools;         //鋼絲絨
    private Color[] woodPowderColors;
    private Color[] steelWoolColors;

    [Tooltip("加熱速度")]
    [SerializeField] private float heatingSpeed = 0.5f;
    [Tooltip("冷卻速度")]
    [SerializeField] private float coolingSpeed = 1f;
    [Tooltip("鋼絲絨加熱最高溫")]
    [SerializeField] private float maxTemp = 700f;
    [Tooltip("室溫")]
    [SerializeField] private float roomTemp = 25f;
    [Tooltip("目前鋼絲絨溫度")]
    [SerializeField] private float steelWoolTemp = 1f;
    private float timer = 0.0f;                         //計時器

    private bool heating = false;                       //  加熱中
    private bool isBurn = false;                        //  燃燒完畢
    private bool firstTimeWarning = true;               // 第一次抓取危險物品的通知
    private bool isSteelWoolsCooling = false;

    private Color heatingColor;
    private Color coolingColor;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T2_1_1");

        steelWoolTemp = roomTemp;
        steelWoolColors = new Color[steelWools.Length];
        for (int i = 0; i < steelWools.Length; i++)
        {
            steelWoolColors.SetValue(steelWools[i].material.color, i);
            //  隱藏其他鋼絲絨
            if (i > 0)
            {
                Color invisible = steelWoolColors[i];
                invisible.a = 0f;
                steelWools[i].material.color = invisible;
            }
        }
    }

    void Update()
    {
        if (isSteelWoolsCooling)
            return;

        if (heating)
        {
            if (steelWoolTemp < maxTemp)
            {
                steelWoolTemp += Time.deltaTime * heatingSpeed;
            }
            else if (!isBurn)
            {
                isBurn = true;
                steelWoolTemp = maxTemp;
            }

            //處理變色
            heatingColor = steelWoolColors[1];
            heatingColor.a = steelWoolTemp / maxTemp;
            steelWools[1].material.color = heatingColor;

            coolingColor = steelWoolColors[2];
            coolingColor.a = roomTemp / steelWoolTemp;
            steelWools[2].material.color = coolingColor;
        }
        else
        {
            if (steelWoolTemp > roomTemp)
            {
                steelWoolTemp -= Time.deltaTime * coolingSpeed;
                if (isBurn)
                {
                    //處理變色
                    coolingColor = steelWoolColors[2];
                    coolingColor.a = roomTemp / steelWoolTemp;
                    steelWools[2].material.color = coolingColor;
                }
            }
            else
            {
                steelWoolTemp = roomTemp;
                if (isBurn && !isSteelWoolsCooling)
                {
                    timer += Time.deltaTime;
                    if (timer >= 3f)
                    {
                        //EndTheTutorial();
                        for (int i = 0; i < steelWools.Length - 1; i++)
                        {
                            Color invisible = steelWoolColors[i];
                            invisible.a = 0f;
                            steelWools[i].material.color = invisible;
                        }
                        isSteelWoolsCooling = true;
                    }
                }
            }

            //處理變色
            heatingColor = steelWoolColors[1];
            heatingColor.a = steelWoolTemp / maxTemp;
            steelWools[1].material.color = heatingColor;
        }
    }

    public void StartHeating(bool on)
    {
        if (isSteelWoolsCooling)
            return;
        heating = on;
    }

    public void OnBlowtorchGrabbed()
    {
        if (firstTimeWarning)
        {
            hintManager.PlayWarningHint("W_Blowtorch");
            firstTimeWarning = false;
        }
    }

    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T2_1_2");
        hintManager.ShowNextButton(this.gameObject);
        gasCan.SendMessage("BackToInitial");
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
