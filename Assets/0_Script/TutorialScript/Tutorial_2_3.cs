using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial_2_3 : MonoBehaviour
{
    [Header("寶特瓶")]
    [Tooltip("寶特瓶")]
    [SerializeField] private GameObject bottle;
    [Tooltip("氣球")]
    [SerializeField] private GameObject balloon;
    [Tooltip("瓶蓋位置")]
    [SerializeField] private Transform bottleCapPoint;
    [Tooltip("瓶內試管位置")]
    [SerializeField] private Transform testTubePoint;

    [Tooltip("寶特瓶液體")]
    [SerializeField] private LiquidController bottleLiquid;
    [Tooltip("寶特瓶液體反應後顏色")]
    [SerializeField] private Color bottleLiquidColor_final;
    private Color bottleLiquidColor;
    [Tooltip("變色速度")]
    [SerializeField] private float reactionTime = 3f;

    [Tooltip("設定搖晃判斷的角度閾值")]
    [SerializeField] private float shakeThreshold = 90f;
    private float bottleAngle = 0f;
    [Tooltip("變色速度")]
    [SerializeField] private float colorChangeSpeed = 3f;

    [Header("後續要隱藏的物件")]
    [Tooltip("前半部需要顯示的物件")]
    [SerializeField] private GameObject object_Step1;
    [Tooltip("後半部需要顯示的物件")]
    [SerializeField] private GameObject object_Step2;

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

    private float timer = 0;
    private bool firstTimeWarning = true;       // 第一次抓取危險物品的通知
    private bool bottleRotationWarning = false; //警告寶特瓶不可傾倒
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
        stepPage.SetActive(true);
        massPage.SetActive(false);
        particlePage.SetActive(false);
        object_Step1.SetActive(true);
        object_Step2.SetActive(false);
        foreach (GameObject checkImage in checkImages)
        {
            checkImage.SetActive(false);
        }

        hintManager.SwitchStep("T2_3_1");
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
            case 2: //待氣球套上寶特瓶
                break;
            case 3: //待套上橡皮筋
                break;
            case 4: //待寶特瓶放上電子秤
                if (!bottleRotationWarning)
                {
                    if (bottleAngle >= shakeThreshold * 0.5f)
                    {
                        audioManager.PlayVoice("W_BottleRotation");
                        print("現在請勿傾倒寶特瓶。");
                        bottleRotationWarning = true;
                    }
                }
                break;
            case 5: //待寶特瓶搖晃
                // 判斷是否達到搖晃的閾值
                if (bottleAngle >= shakeThreshold)
                {
                    timer = reactionTime;
                    bottleLiquidColor = bottleLiquid.liquidColor;
                    Status++;
                }
                break;
            case 6: //待反應結束

                //氣球膨脹

                // 使用 Lerp 混合顏色
                bottleLiquidColor = Color.Lerp(bottleLiquidColor, bottleLiquidColor_final, colorChangeSpeed);
                // 更新物件的顏色
                bottleLiquid.liquidColor = bottleLiquidColor;

                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    particlePage.SetActive(true);
                    hintManager.SwitchStep("T2_3_4");
                    Status++;
                }
                break;
            case 7: //待寶特瓶放上電子秤
                break;
            case 8: //待拿掉氣球
                break;
            case 9: //待寶特瓶放上電子秤
                break;
            case 10: //結論
                break;
        }
    }

    public void Reaction(GameObject sender)
    {
        if (sender.name== "KitchenScale")
        {
            switch (Status)
            {
                case 4: //寶特瓶放上電子秤
                    
                    stepPage.SetActive(false);
                    massPage.SetActive(true);
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                    massTexts[0].text = scaleVale.ToString("0") + "g";
                    hintManager.SwitchStep("T2_3_3");
                    Status++;
                    break;
                case 7: //搖晃後放上電子秤
                    //取得第二個數值
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                    massTexts[1].text = scaleVale.ToString("0") + "g";
                    //讓蓋子可以被打開
                    balloon.GetComponent<XRGrabInteractable>().enabled = true;
                    balloon.tag = "Pickable";
                    hintManager.SwitchStep("T2_3_5");
                    Status++;
                    break;
                case 9: //打開瓶蓋後放上電子秤
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
        if (sender.name == "Balloon" && Status == 8)
        {
            //打開瓶蓋
            balloon.transform.SetParent(transform);
            balloon.GetComponent<Rigidbody>().isKinematic = false;
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
                    checkImages[1].SetActive(true);
                    checkImages[2].SetActive(true);
                    //場上只剩下後半部物件
                    object_Step1.SetActive(false);
                    object_Step2.SetActive(true);
                    Status++;
                }
                break;
            case 2: //套上氣球
                print(sender.name);
                if (sender.name == "Balloon")
                {
                    sender.GetComponent<XRGrabInteractable>().enabled = false;
                    sender.GetComponent<Rigidbody>().isKinematic = true;
                    sender.transform.position = bottleCapPoint.position;
                    sender.transform.rotation = bottleCapPoint.rotation;
                    sender.transform.SetParent(bottleCapPoint);
                    sender.tag = "Untagged";
                    Status++;
                }
                break;
            case 3: //套上橡皮筋
                if (sender.name == "RubberBand")
                {
                    sender.gameObject.SetActive(false);

                    //畫面跳轉至測量質量頁面
                    stepPage.SetActive(false);
                    massPage.SetActive(true);

                    hintManager.SwitchStep("T2_3_2");
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
        if (fullLiquid == 2 && Status == 0)
        {
            checkImages[0].SetActive(true);
            Status++;
        }

    }

    //抓取鹽酸時觸發
    public void OnHCIGrabbed()
    {
        if (firstTimeWarning)
        {
            audioManager.PlayVoice("W_HCI");
            firstTimeWarning = false;
        }
    }
    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T2_3_6");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
