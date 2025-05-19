using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_1_2 : MonoBehaviour
{
    [SerializeField] private Blowtorch gasCan;         //噴槍
    [SerializeField] MeshRenderer[] irons;              //軟鐵片
    private TextureScaler iron2TextureScaler;           //控制鐵片貼圖縮放
    private Color[] ironColors;
    [SerializeField] Image fillBar;             // 進度條
    [Tooltip("加熱速度")]
    [SerializeField] private float heatingSpeed = 0.5f;
    [Tooltip("冷卻速度")]
    [SerializeField] private float coolingSpeed = 1f;
    [Tooltip("鐵片加熱最高溫")]
    [SerializeField] private float maxTemp = 700f;
    [Tooltip("室溫")]
    [SerializeField] private float roomTemp = 25f;
    [Tooltip("目前鐵片溫度")]
    [SerializeField] private float ironTemp = 1f;
    private float currMaxIronTemp = 0f;
    private float timer = 0.0f;                         //計時器

    private bool heating = false;                       //  加熱中
    private bool isBurn = false;                        //  燃燒完畢
    private bool firstTimeWarning = true;              // 第一次抓取危險物品的通知
    private bool isEnd = false;

    private Color heatingColor;
    private Color coolingColor;

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private HintManager hintManager;            //管理提示板
    private MoleculaDisplay moleculaManager;    //管理分子螢幕

    // Start is called before the first frame update
    void OnEnable()
    {
        gasCan = GameObject.Find("GAS").GetComponent<Blowtorch>();

        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_2_1");

        moleculaManager.ShowMoleculas(1);
        ironTemp = roomTemp;
        currMaxIronTemp = ironTemp;
        ironColors = new Color[irons.Length];
        for (int i = 0; i < irons.Length; i++)
        {
            ironColors.SetValue(irons[i].material.color, i);
            //  隱藏其他鐵片
            if (i > 0)
            {
                Color invisible = ironColors[i];
                invisible.a = 0f;
                irons[i].material.color = invisible;
            }
        }
        iron2TextureScaler = irons[1].GetComponent<TextureScaler>();
        iron2TextureScaler.SetRatio(0f);
        fillBar.fillAmount = 0;
    }

    void Update()
    {
        if (isEnd)
            return;

        //  紀錄最高溫度
        if (ironTemp > currMaxIronTemp)
        {
            currMaxIronTemp = ironTemp;
            moleculaManager.PlayMoleculasAnimation();
            //moleculaManager.SetMoleculasAnimationSpeed(heatingSpeed / maxTemp);
        }
        else
        {
            moleculaManager.StopMoleculasAnimation();
        }

        if (heating)
        {
            if (ironTemp < maxTemp)
            {
                ironTemp += Time.deltaTime* heatingSpeed;
            }
            else if(!isBurn)
            {
                isBurn = true;
                ironTemp = maxTemp;
            }

            //處理變色
            heatingColor = ironColors[1];
            heatingColor.a = ironTemp / maxTemp;
            irons[1].material.color = heatingColor;
            iron2TextureScaler.SetRatio(ironTemp / maxTemp);
            coolingColor = ironColors[2];
            coolingColor.a = roomTemp / ironTemp;
            irons[2].material.color = coolingColor;
        }
        else
        {
            if (ironTemp > roomTemp)
            {
                ironTemp -= Time.deltaTime * coolingSpeed;
                if (isBurn)
                {
                    //處理變色
                    coolingColor = ironColors[2];
                    coolingColor.a = roomTemp / ironTemp;
                    irons[2].material.color = coolingColor;
                }
            }
            else
            {
                ironTemp = roomTemp;
                if (isBurn && !isEnd)
                {
                    timer += Time.deltaTime;
                    if (timer >= 1.5f)
                    {
                        EndTheTutorial();
                    }
                }
            }

            //處理變色
            heatingColor = ironColors[1];
            heatingColor.a = ironTemp / maxTemp;
            irons[1].material.color = heatingColor;
            //iron2TextureScaler.SetRatio(ironTemp / maxTemp);
        }
        fillBar.fillAmount = (ironTemp - roomTemp) / maxTemp;
    }

    public void StartHeating(bool on)
    {
        if (isEnd)
            return;
        heating = on;
    }

    //抓取物件時觸發
    public void Grab(GameObject obj)
    {
        if (obj == gasCan.gameObject)
        {
            if (firstTimeWarning)
            {
                audioManager.PlayVoice("W_Blowtorch");
                firstTimeWarning = false;
            }
            gasCan.Grab(true);
        }
    }
    //鬆開物件時觸發
    public void Release(GameObject obj)
    {
        if (obj == gasCan.gameObject)
        {
            gasCan.Grab(false);
        }
    }
    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T1_2_2");
        hintManager.ShowNextButton(this.gameObject);
        isEnd = true;
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}