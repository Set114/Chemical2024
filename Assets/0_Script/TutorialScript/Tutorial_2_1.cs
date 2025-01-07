using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial_2_1 : MonoBehaviour
{
    [Header("木屑")]
    [Tooltip("用於縮放的parent")]
    [SerializeField] Transform woodPowder;
    [SerializeField] MeshRenderer meshRenderer_woodPowder;
    [Tooltip("木屑燒完的顏色")]
    [SerializeField] private Color burntColor_woodPowder;
    private Color color_woodPowder;
    [Tooltip("木屑燃燒效果")]
    [SerializeField] private ParticleSystem fire_woodPowder;
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText_woodPowder;

    [Tooltip("目前木屑重量")]
    [SerializeField] private float weight_woodPowder = 20f;
    [Tooltip("燃燒後木屑重量")]
    [SerializeField] private float finalWeight_woodPowder = 10f;
    private float weightChangeSpeed_woodPowder;          //  木屑改變重量速度
    [Tooltip("木屑需要持續加熱的時間")]
    [SerializeField] private float heatingTime_woodPowder = 3f;
    [Tooltip("木屑持續燃燒時間")]
    [SerializeField] private float burningTime_woodPowder = 10f;
    [Tooltip("木屑冷卻時間")]
    [SerializeField] private float coolingTime_woodPowder = 1f;
    [Tooltip("木屑燃燒後的尺寸")]
    [SerializeField] private float finalScale_woodPowder = 0.5f;
    private float scale_woodPowder;                     //  木屑尺寸
    private float scalingSpeed_woodPowder;              //  木屑縮小速度
    [Tooltip("木屑變色速度")]
    [SerializeField] private float colorChangeSpeed_woodPowder;

    private float timer_woodPowder = 0.0f;              //  計時器
    private int status_woodPowder = 0;                  //  木屑狀態

    [Header("鋼絲絨")]
    [SerializeField] MeshRenderer[] steelWools;         //  鋼絲絨
    private Color[] steelWoolColors;
    [Tooltip("鋼絲絨燃燒效果")]
    [SerializeField] private ParticleSystem fire_steelWool;
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText_steelWool;

    [Tooltip("目前鋼絲絨重量")]
    [SerializeField] private float weight_steelWool = 20f;
    [Tooltip("燃燒後鋼絲絨重量")]
    [SerializeField] private float finalWeight_steelWool = 30f;
    private float weightChangeSpeed_steelWool = 1f;     //  鋼絲絨改變重量速度
    [Tooltip("鋼絲絨需要持續加熱的時間")]
    [SerializeField] private float heatingTime_steelWool = 5f;
    [Tooltip("鋼絲絨持續燃燒時間")]
    [SerializeField] private float burningTime_steelWool = 10f;
    [Tooltip("鋼絲絨冷卻時間")]
    [SerializeField] private float coolingTime_steelWool = 3f;
    [Tooltip("鋼絲絨變色速度")]
    [SerializeField] private float colorChangeSpeed_steelWool;

    private float timer_steelWool = 0.0f;                //  計時器
    private int status_steelWool = 0;                    //  鋼絲絨狀態


    private bool firstTimeWarning = true;               //  第一次抓取危險物品的通知

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private HintManager hintManager;            //管理提示板

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T2_1_1");

        fire_woodPowder.Stop();
        // 計算改變重量速度
        weightChangeSpeed_woodPowder = (weight_woodPowder - finalWeight_woodPowder) / burningTime_woodPowder;
        // 計算木屑縮小速度
        scale_woodPowder = woodPowder.localScale.x;
        scalingSpeed_woodPowder = (scale_woodPowder - finalScale_woodPowder) / burningTime_woodPowder;
        color_woodPowder = meshRenderer_woodPowder.material.color;


        fire_steelWool.Stop();
        // 計算改變重量速度
        weightChangeSpeed_steelWool = (finalWeight_steelWool - weight_steelWool) / burningTime_steelWool;
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
        weightText_woodPowder.text = weight_woodPowder.ToString("0") + "g";
        weightText_steelWool.text = weight_steelWool.ToString("0") + "g";
        switch (status_woodPowder)
        {
            case 0:     //  待加熱
                if (timer_woodPowder >= heatingTime_woodPowder)
                {
                    timer_woodPowder = burningTime_woodPowder;
                    fire_woodPowder.Play();
                    status_woodPowder++;
                }
                break;
            case 1:     //  燃燒中
                timer_woodPowder -= Time.deltaTime;

                scale_woodPowder -= scalingSpeed_woodPowder * Time.deltaTime;
                scale_woodPowder = Mathf.Max(scale_woodPowder, finalScale_woodPowder);
                woodPowder.localScale = new Vector3(scale_woodPowder, scale_woodPowder, scale_woodPowder);

                //重量減輕
                weight_woodPowder -= weightChangeSpeed_woodPowder * Time.deltaTime;
                weight_woodPowder = Mathf.Max(weight_woodPowder, finalWeight_woodPowder);

                // 使用 Lerp 混合顏色
                color_woodPowder = Color.Lerp(color_woodPowder, burntColor_woodPowder, colorChangeSpeed_woodPowder);
                // 更新物件的顏色
                meshRenderer_woodPowder.material.color = color_woodPowder;

                if (timer_woodPowder <= 0f)
                {
                    timer_woodPowder = coolingTime_woodPowder;
                    fire_woodPowder.Stop();
                    status_woodPowder++;
                }
                break;
            case 2:     //  冷卻中
                timer_woodPowder -= Time.deltaTime;
                if (timer_woodPowder <= 0f)
                {
                    if (status_steelWool == 3)
                        EndTheTutorial();
                    status_woodPowder++;
                }
                break;
        }

        switch (status_steelWool)
        {
            case 0:     //  待加熱

                //處理變色
                steelWoolColors[1].a = timer_steelWool / heatingTime_steelWool;
                if (timer_steelWool >= heatingTime_steelWool)
                {
                    timer_steelWool = burningTime_steelWool;
                    fire_steelWool.Play();
                    steelWoolColors[1].a = 1f;
                    steelWoolColors[2].a = 1f;
                    steelWools[2].material.color = steelWoolColors[2];
                    status_steelWool++;
                }
                steelWools[1].material.color = steelWoolColors[1];

                break;
            case 1:     //  燃燒中
                timer_steelWool -= Time.deltaTime;

                //重量減輕
                weight_steelWool += weightChangeSpeed_steelWool * Time.deltaTime;
                weight_steelWool = Mathf.Max(weight_steelWool, finalWeight_steelWool);

                //處理變色
                steelWoolColors[1].a = timer_steelWool / burningTime_steelWool;

                if (timer_steelWool <= 0f)
                {
                    timer_steelWool = coolingTime_steelWool;
                    fire_steelWool.Stop();
                    steelWoolColors[1].a = 0f;
                    status_steelWool++;
                }
                steelWools[1].material.color = steelWoolColors[1];
                break;
            case 2:     //  冷卻中
                timer_steelWool -= Time.deltaTime;
                if (timer_steelWool <= 0f)
                {
                    if (status_woodPowder == 3)
                        EndTheTutorial();
                    status_steelWool++;
                }
                break;
        }
    }

    public void ReactionStay(GameObject obj)
    {
        if (obj.name == "WoodPowders" && status_woodPowder == 0)
        {
            //  開始加熱
            timer_woodPowder += Time.deltaTime;
        }
        else if (obj.name == "SteelWool" && status_steelWool == 0)
        {
            //  開始加熱
            timer_steelWool += Time.deltaTime;
        }
    }

    public void OnBlowtorchGrabbed()
    {
        if (firstTimeWarning)
        {
            audioManager.PlayVoice("W_Blowtorch");
            firstTimeWarning = false;
        }
    }

    void StartHeating(bool on)
    {

    }

    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T2_1_2");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
