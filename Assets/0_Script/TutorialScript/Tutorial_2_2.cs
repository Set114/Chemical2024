using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial_2_2 : MonoBehaviour
{
    [Header("寶特瓶")]
    [Tooltip("寶特瓶")]
    [SerializeField] private GameObject bottle;
    [Tooltip("寶特瓶蓋")]
    [SerializeField] private GameObject bottleCap;
    [Tooltip("瓶蓋位置")]
    [SerializeField] private Transform bottleCapPoint;
    [Tooltip("瓶內試管位置")]
    [SerializeField] private Transform testTubePoint;

    [Tooltip("設定搖晃判斷的角度閾值")]
    [SerializeField] private float shakeThreshold = 90f;
    private float bottleAngle = 0f;

    [Header("後續要隱藏的物件")]
    [Tooltip("碳酸鈉燒杯")]
    [SerializeField] private GameObject Beaker_SodiumCarbonate;
    [Tooltip("氯化鈣燒杯")]
    [SerializeField] private GameObject Beaker_CalciumChloride;
    [Tooltip("試管架")]
    [SerializeField] private GameObject testTubeRack;
    [Tooltip("鑷子")]
    [SerializeField] private GameObject tweezers;

    [Header("質量設定")]
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText;
    [Tooltip("磅秤目前數值")]
    [SerializeField] private float scaleVale = 0f;
    [Tooltip("寶特瓶重量")]
    [SerializeField] private float weight_Bottle = 30f;
    [Tooltip("瓶蓋重量")]
    [SerializeField] private float weight_BottleCap = 10f;
    [Tooltip("試管重量")]
    [SerializeField] private float weight_TestTube = 40f;

    [Header("UI")]
    [Tooltip("步驟頁面")]
    [SerializeField] private GameObject stepPage;
    [Tooltip("步驟打勾圖示")]
    [SerializeField] private GameObject[] checkImages;
    [Tooltip("測量質量頁面")]
    [SerializeField] private GameObject massPage;
    [Tooltip("步驟打勾圖示")]
    [SerializeField] private Text[] massTexts;
    [Tooltip("粒子畫面")]
    [SerializeField] private GameObject particlePage;

    private int fullLiquid = 0;                 //已裝滿的容器
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private HintManager hintManager;            //管理提示板

    private MouseController pcController;
    private bool isPC;

    // Start is called before the first frame update
    void Start()
    {
        //判斷平台
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        isPC = true;
        pcController = FindObjectOfType<MouseController>();
#else
        isPC = false;
#endif
        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T2_2_1");
        stepPage.SetActive(true);
        massPage.SetActive(false);
        particlePage.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        // 計算寶特瓶Z軸與世界向上的夾角
        bottleAngle = Vector3.Angle(bottle.transform.up, Vector3.up);
        switch (Status)
        {
            case 0: //等待兩邊的液體倒入
                break;
            case 1: //兩邊的液體已倒入，待試管放進寶特瓶內
                break;
            case 2: //待鎖上寶特瓶蓋後， 畫面跳轉至測量質量頁面。
                break;
            case 3: //待寶特瓶放上電子秤
                if (bottleAngle >= shakeThreshold * 0.5f)
                {
                    Debug.Log("不要搖晃寶特瓶！");
                }
                break;
            case 4: //待寶特瓶搖晃
                // 判斷是否達到搖晃的閾值
                if (bottleAngle >= shakeThreshold)
                {
                    Debug.Log("物體已經搖晃過！");
                    particlePage.SetActive(true);
                    Status++;
                }
                break;
            case 5: //待寶特瓶放上電子秤
                break;
            case 6: //待打開瓶蓋
                break;
            case 7: //待寶特瓶放上電子秤
                break;
            case 8: //結論
                break;
        }
    }

    public void Reaction(GameObject sender)
    {
        if (sender.name== "KitchenScale")
        {
            switch (Status)
            {
                case 3: //寶特瓶放上電子秤
                    
                    stepPage.SetActive(false);
                    massPage.SetActive(true);
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                    massTexts[0].text = scaleVale.ToString("0") + "g";
                    Status++;
                    break;
                case 5: //搖晃後放上電子秤
                    //取得第二個數值
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                    massTexts[1].text = scaleVale.ToString("0") + "g";
                    //bottle.GetComponent<XRGrabInteractable>().enabled = false;
                    //bottle.tag = "Untagged";
                    //讓蓋子可以被打開
                    bottleCap.GetComponent<XRGrabInteractable>().enabled = true;
                    bottleCap.tag = "Pickable";
                    Status++;
                    break;
                case 7: //打開瓶蓋後放上電子秤
                        //取得第三個數值
                    scaleVale = weight_Bottle + weight_TestTube;
                    massTexts[2].text = scaleVale.ToString("0") + "g";
                    EndTheTutorial();
                    Status++;
                    break;
            }
        }
        weightText.text = scaleVale.ToString("0") + "g";
    }

    public void ReactionExit(GameObject sender)
    {
        if (sender.name == "KitchenScale")
        {
            scaleVale = 0f;
            weightText.text = scaleVale.ToString("0") + "g";
        }
        if (sender.name == "Cap" && Status == 6)
        {
            //打開瓶蓋
            bottleCap.transform.SetParent(transform);
            bottleCap.GetComponent<Rigidbody>().isKinematic = false;
            Status++;
        }
    }

    public void ReactionStay(GameObject sender)
    {
        if (isPC)
        {
            if (pcController.selectedObject)
            {
                return;
            }
        }
        switch (Status)
        {
            case 1: //試管放進寶特瓶
                if(sender.name== "TestTube")
                {
                    sender.tag = "Untagged";
                    sender.transform.position = testTubePoint.position;
                    sender.transform.rotation = testTubePoint.rotation;
                    sender.transform.SetParent(testTubePoint);
                    sender.GetComponent<Rigidbody>().isKinematic = true;
                    Status++;
                }
                break;
            case 2: //鎖上寶特瓶蓋
                if (sender.name == "Cap")
                {
                    bottleCap.GetComponent<XRGrabInteractable>().enabled = false;
                    bottleCap.GetComponent<Rigidbody>().isKinematic = true;   
                    bottleCap.transform.position = bottleCapPoint.position;
                    bottleCap.transform.rotation = bottleCapPoint.rotation;
                    bottleCap.transform.SetParent(bottleCapPoint);
                    bottleCap.tag = "Untagged";

                    //畫面跳轉至測量質量頁面
                    stepPage.SetActive(false);
                    massPage.SetActive(true);
                    //場上只剩下電子秤跟寶特瓶
                    Beaker_SodiumCarbonate.SetActive(false);
                    Beaker_CalciumChloride.SetActive(false);
                    testTubeRack.SetActive(false);
                    tweezers.SetActive(false);
                    Status++;
                }
                break;
        }
    }
    //  液體裝滿時通知
    public void LiquidFull(GameObject obj)
    {
        switch (obj.name)
        {
            case "WaterBottle":
                if (Status == 0)
                {
                    fullLiquid++;
                }
                break;
            case "TestTube":
                if (Status == 0)
                {
                    fullLiquid++;
                }
                break;
        }
        if (fullLiquid == 2)
        {
            Debug.Log("溶液都倒完");
            Status++;
        }

    }

    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T2_2_2");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
