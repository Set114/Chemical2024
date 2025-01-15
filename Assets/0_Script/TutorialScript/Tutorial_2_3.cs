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

    [Header("氣球")]
    [Tooltip("未充氣氣球")]
    [SerializeField] private GameObject balloon;
    [Tooltip("未充氣氣球")]
    [SerializeField] private GameObject balloon_flat;
    [Tooltip("充氣用氣球")]
    [SerializeField] private GameObject balloon_inflated;
    [Tooltip("瓶口橡皮筋")]
    [SerializeField] private GameObject rubberBand_Cap;
    [Tooltip("充氣速度")]
    [SerializeField] private float inflationSpeed = 1f;
    [Tooltip("最大尺寸")]
    [SerializeField] private float balloonMaxSize = 1f;

    [Header("要隱藏的物件")]
    [Tooltip("前半部需要顯示的物件")]
    [SerializeField] private GameObject object_Step1;
    [Tooltip("後半部需要顯示的物件")]
    [SerializeField] private GameObject object_Step2;   
    [Tooltip("鑷子")]
    [SerializeField] private GameObject tweezers;

    [Header("質量設定")]
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText;
    [Tooltip("磅秤目前數值")]
    [SerializeField] private float scaleVale = 0f;
    [Tooltip("寶特瓶重量")]
    [SerializeField] private float weight_Bottle = 30f;
    [Tooltip("氣球重量")]
    [SerializeField] private float weight_Balloon = 10f;
    [Tooltip("試管重量")]
    [SerializeField] private float weight_TestTube = 40f;

    [Header("UI")]
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
        particlePage.SetActive(false);
        object_Step1.SetActive(true);
        object_Step2.SetActive(false);

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
                float size = balloon_inflated.transform.localScale.x;
                size = Mathf.Clamp(size += Time.deltaTime * inflationSpeed, size, balloonMaxSize);
                balloon_inflated.transform.localScale = new Vector3(size, size, size);

                // 使用 Lerp 混合顏色
                bottleLiquidColor = Color.Lerp(bottleLiquidColor, bottleLiquidColor_final, colorChangeSpeed);
                // 更新物件的顏色
                bottleLiquid.liquidColor = bottleLiquidColor;

                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    particlePage.SetActive(true);
                    hintManager.SwitchStep("T2_3_6");
                    Status++;
                }
                break;
            case 7: //待寶特瓶放上電子秤
                break;
            case 8: //待拿掉氣球
                break;
            case 9: //待氣球放上電子秤
                break;
            case 10: //結論
                break;
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
                if(sender.name== "TestTube_2-3")
                {
                    sender.tag = "Untagged";
                    sender.transform.position = testTubePoint.position;
                    sender.transform.rotation = testTubePoint.rotation;
                    sender.transform.SetParent(testTubePoint);
                    sender.GetComponent<Rigidbody>().isKinematic = true;
                    hintManager.SwitchStep("T2_3_3");
                    //場上只剩下後半部物件
                    object_Step1.SetActive(false);
                    object_Step2.SetActive(true);
                    tweezers.transform.SetParent(object_Step1.transform);
                    Status++;
                }
                break;
            case 2: //套上氣球
                if (sender.name == "Balloon_2-3")
                {
                    bottle.GetComponent<Rigidbody>().isKinematic = true;

                    balloon.GetComponent<XRGrabInteractable>().enabled = false;
                    balloon.tag = "Untagged";
                    balloon.GetComponent<Rigidbody>().isKinematic = true;
                    balloon.transform.position = bottleCapPoint.position;
                    balloon.transform.rotation = bottleCapPoint.rotation;
                    balloon.transform.SetParent(bottleCapPoint);
                    balloon_flat.SetActive(false);
                    balloon_inflated.SetActive(true);
                    Status++;
                }
                break;
            case 3: //套上橡皮筋
                if (sender.name == "RubberBand_2-3")
                {
                    sender.gameObject.SetActive(false);
                    rubberBand_Cap.SetActive(true);
                    bottle.GetComponent<Rigidbody>().isKinematic = false;
                    //畫面跳轉至測量質量頁面
                    hintManager.SwitchStep("T2_3_4");
                    Status++;
                }
                break;
            case 4: //寶特瓶放上電子秤
                if (sender.name == "KitchenScale")
                {
                    scaleVale = weight_Bottle + weight_Balloon + weight_TestTube;
                    massTexts[0].text = scaleVale.ToString("0") + "g";
                    hintManager.SwitchStep("T2_3_5");
                    Status++;
                }
                break;
            case 7: //搖晃後放上電子秤
                if (sender.name == "KitchenScale")
                {
                    //取得第二個數值
                    scaleVale = weight_Bottle + weight_Balloon + weight_TestTube;
                    massTexts[1].text = scaleVale.ToString("0") + "g";

                    //讓寶特瓶不可被拿起
                    bottle.GetComponent<XRGrabInteractable>().enabled = false;
                    bottle.tag = "Untagged";
                    bottle.GetComponent<Rigidbody>().isKinematic = true;

                    //讓氣球可以被拿起
                    balloon.GetComponent<XRGrabInteractable>().enabled = true;
                    balloon.tag = "Pickable";
                    hintManager.SwitchStep("T2_3_7");

                    //磅秤改為偵測氣球
                    sender.GetComponent<CollisionDetection>().targetName = "Balloon_2-3";

                    Status++;
                }
                break;
            case 9: //氣球放上電子秤
                if (sender.name == "KitchenScale")
                {
                    //取得第三個數值
                    scaleVale = weight_Bottle + weight_Balloon + weight_TestTube;
                    massTexts[2].text = scaleVale.ToString("0") + "g";

                    //讓氣球不可被拿起
                    balloon.GetComponent<XRGrabInteractable>().enabled = false;
                    balloon.tag = "Untagged";

                    EndTheTutorial();
                    Status++;
                }
                break;
        }
        weightText.text = scaleVale.ToString("0") + "g";
    }
    //  液體裝滿時通知
    public void LiquidFull(GameObject obj)
    {
        switch (obj.name)
        {
            case "WaterBottle_2-3":
                if (Status == 0)
                {
                    fullLiquid++;
                }
                break;
            case "TestTube_2-3":
                if (Status == 0)
                {
                    fullLiquid++;
                }
                break;
        }
        if (fullLiquid == 2 && Status == 0)
        {
            hintManager.SwitchStep("T2_3_2");
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
    //抓取氣球時觸發
    public void OnBalloonGrabbed()
    {
        if (Status == 8)
        {
            //拿掉氣球
            balloon.transform.SetParent(transform);
            balloon.GetComponent<Rigidbody>().isKinematic = false;

            balloon_flat.SetActive(true);
            balloon_inflated.SetActive(false);
            rubberBand_Cap.SetActive(false);
            scaleVale = weight_Bottle + weight_TestTube;
            weightText.text = scaleVale.ToString("0") + "g";
            Status++;
        }
    }
    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T2_3_8");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(1);
    }
}
