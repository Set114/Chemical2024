using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Tutorial_2_2 : MonoBehaviour
{
    [Header("寶特瓶")]
    [Tooltip("寶特瓶")]
    [SerializeField] private GameObject bottle;
    [Tooltip("寶特瓶起始位置")]
    [SerializeField] private Transform bottleStartPoint;
    [Tooltip("寶特瓶傾斜位置")]
    [SerializeField] private Transform bottleTiltPoint;
    [Tooltip("寶特瓶蓋")]
    [SerializeField] private GameObject bottleCap;
    [Tooltip("瓶蓋位置")]
    [SerializeField] private Transform bottleCapPoint;
    [Tooltip("瓶內試管位置")]
    [SerializeField] private Transform testTubePoint;
    [Tooltip("瓶內碰撞牆物件")]
    [SerializeField] private GameObject bottleCollisionInside;

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
    [Tooltip("碳酸鈉燒杯")]
    [SerializeField] private GameObject Beaker_SodiumCarbonate;
    [Tooltip("氯化鈣燒杯")]
    [SerializeField] private GameObject Beaker_CalciumChloride;
    [Tooltip("試管架")]
    [SerializeField] private GameObject testTubeRack;
    [Tooltip("鑷子")]
    [SerializeField] private Tweezers tweezers;

    [Header("質量設定")]
    [Tooltip("磅秤")]
    [SerializeField] private GameObject kitchenScale;
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText;
    [Tooltip("磅秤文字_放大螢幕")]
    [SerializeField] Text weightTextDisplay;
    [Tooltip("磅秤目前數值")]
    [SerializeField] private float scaleVale = 0f;
    [Tooltip("寶特瓶重量")]
    [SerializeField] private float weight_Bottle = 30f;
    [Tooltip("瓶蓋重量")]
    [SerializeField] private float weight_BottleCap = 10f;
    [Tooltip("試管重量")]
    [SerializeField] private float weight_TestTube = 40f;

    [Header("UI")]
    [Tooltip("質量文字")]
    [SerializeField] private Text[] massTexts;

    private float timer = 0;
    private int Status = 0;

    private GameManager gm;
    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private HintManager hintManager;            //管理提示板
    private MoleculaDisplay moleculaManager;    //管理分子螢幕

    private MouseController pcController;       //PC的控制器
    private bool isPC;                          //偵測是否是PC模式

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
        gm = FindObjectOfType<GameManager>();
        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();

        hintManager.gameObject.SetActive(true);

        hintManager.SetTotalStep(8);
        hintManager.SwitchStep("T2_2_1");
        kitchenScale.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        // 計算寶特瓶Z軸與世界向上的夾角
        bottleAngle = Vector3.Angle(bottle.transform.up, Vector3.up);
        switch (Status)
        {
            case 0: //等待寶特瓶液體倒滿
                break;
            case 1: //等待試管液體倒滿
                break;
            case 2: //兩邊的液體已倒入，待試管放進寶特瓶內
            case 3: //待鎖上寶特瓶蓋後， 畫面跳轉至測量質量頁面。
            case 4: //待寶特瓶放上電子秤
                if (bottleAngle >= shakeThreshold)
                {
                    audioManager.PlayVoice("W_BottleRotation");
                    gm.GetMistake();
                    print("現在請勿傾倒寶特瓶。");
                    bottle.SendMessage("Return", SendMessageOptions.DontRequireReceiver);
                    if (isPC)
                        pcController.SendMessage("Reset");
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

                // 使用 Lerp 混合顏色
                bottleLiquidColor = Color.Lerp(bottleLiquidColor, bottleLiquidColor_final, colorChangeSpeed);
                // 更新物件的顏色
                bottleLiquid.liquidColor = bottleLiquidColor;
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    moleculaManager.ShowMoleculas(0);
                    moleculaManager.PlayMoleculasAnimation();
                    hintManager.SwitchStep("T2_2_6");
                    Status++;
                }
                break;
            case 7: //待寶特瓶放上電子秤
                break;
            case 8: //待打開瓶蓋
                float distance = Vector3.Distance(bottleCap.transform.position, bottleCapPoint.position);
                if (distance > 0.01f)
                {
                    //打開瓶蓋
                    if(!isPC)
                        bottleCap.transform.SetParent(transform);
                    bottleCap.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    scaleVale = weight_Bottle + weight_TestTube;
                    Status++;
                }
                break;
            case 9: //待瓶蓋放上電子秤
                break;
            case 10: //結論
                break;
        }
    }

    public void Reaction(GameObject sender)
    {
        switch (Status)
        {
            case 2: //待試管放進寶特瓶內
                if (isPC && sender.name == "TestTube_2-2_Clamp")
                {
                    bottle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    bottle.transform.position = bottleTiltPoint.position;
                    bottle.transform.rotation = bottleTiltPoint.rotation;

                }
                break;
            case 3: //鎖上寶特瓶蓋
                if (sender.name == "Cap_2-2")
                {
                    bottle.GetComponent<Rigidbody>().isKinematic = true;

                    if(isPC)
                        pcController.SendMessage("Reset",SendMessageOptions.DontRequireReceiver);

                    //bottleCap.GetComponent<Rigidbody>().isKinematic = true;
                    bottleCap.GetComponent<XRGrabInteractable>().enabled = false;
                    bottleCap.tag = "Untagged";
                    bottleCap.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    bottleCap.transform.position = bottleCapPoint.position;
                    bottleCap.transform.rotation = bottleCapPoint.rotation;
                    bottleCap.transform.SetParent(bottleCapPoint);

                    bottle.GetComponent<Rigidbody>().isKinematic = false;

                    //場上只剩下電子秤跟寶特瓶
                    Beaker_SodiumCarbonate.SetActive(false);
                    Beaker_CalciumChloride.SetActive(false);
                    testTubeRack.SetActive(false);
                    tweezers.gameObject.SetActive(false);

                    kitchenScale.SetActive(true);
                    scaleVale = 0f;
                    hintManager.SwitchStep("T2_2_4");
                    Status++;
                }
                break;
            case 4: //寶特瓶放上電子秤
                if (sender.name == "KitchenScale")
                {
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                    massTexts[0].text = scaleVale.ToString("0") + "g";
                    hintManager.SwitchStep("T2_2_5");
                    Status++;
                }
                break;
            case 5: //待寶特瓶搖晃
            case 6: //待反應結束
                if (sender.name == "KitchenScale")
                {
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                }
                break;
            case 7: //搖晃後放上電子秤
                if (sender.name == "KitchenScale")
                {
                    //取得第二個數值
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                    massTexts[1].text = scaleVale.ToString("0") + "g";

                    //讓寶特瓶不可被拿起
                    bottle.GetComponent<XRGrabInteractable>().enabled = false;
                    bottle.tag = "Untagged";
                    bottle.GetComponent<Rigidbody>().isKinematic = true;

                    //讓蓋子可以被打開
                    bottleCap.GetComponent<XRGrabInteractable>().enabled = true;
                    bottleCap.tag = "Pickable";
                    hintManager.SwitchStep("T2_2_7");

                    //磅秤改為偵測瓶蓋
                    sender.GetComponent<CollisionDetection>().targetName = "Cap_2-2";

                    Status++;
                }
                break;
            case 9: //瓶蓋放上電子秤
                if (sender.name == "KitchenScale")
                {
                    //取得第三個數值
                    scaleVale = weight_Bottle + weight_BottleCap + weight_TestTube;
                    massTexts[2].text = scaleVale.ToString("0") + "g";
                    
                    //讓瓶蓋不可被拿起
                    if(isPC)
                        pcController.SendMessage("Reset",SendMessageOptions.DontRequireReceiver);
                    bottleCap.GetComponent<XRGrabInteractable>().enabled = false;
                    bottleCap.tag = "Untagged";                    
                    
                    EndTheTutorial();
                    Status++;
                }
                break;
        }
        weightText.text = scaleVale.ToString("0") + "g";
        weightTextDisplay.text = scaleVale.ToString("0") + "g";
    }
    public void ReactionStay(GameObject sender)
    {
        if (Status == 2 && sender.name == "TestTube_2-2")
        {
            sender.tag = "Untagged";
            sender.transform.position = testTubePoint.position;
            sender.transform.rotation = testTubePoint.rotation;
            sender.transform.SetParent(testTubePoint);
            sender.layer = 3;   //設定其碰撞牆與內部碰撞牆一樣
            sender.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;   //取消物理限制
            sender.GetComponent<Collider>().isTrigger = true;
            sender.GetComponent<Rigidbody>().isKinematic = true;
            bottleCollisionInside.SetActive(true);  //開啟瓶內碰撞器
            hintManager.SwitchStep("T2_2_3");
            if (isPC)
            {
                bottle.transform.position = bottleStartPoint.position;
                bottle.transform.rotation = bottleStartPoint.rotation;
                bottle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            Status++;
        }
    }
    public void ReactionExit(GameObject sender)
    {
        switch (Status)
        {
            case 5: //待寶特瓶搖晃
            case 6: //待反應結束
                if (sender.name == "KitchenScale")
                {
                    scaleVale = 0f;
                }
                break;
        }
        weightText.text = scaleVale.ToString("0") + "g";
        weightTextDisplay.text = scaleVale.ToString("0") + "g";
    }
    //  注入錯液體時通知
    public void InjectWrongFluid(GameObject obj)
    {
        switch (obj.name)
        {
            case "WaterBottle_2-2":
            case "TestTube_2-2":
                obj.SendMessage("Return", SendMessageOptions.DontRequireReceiver);
                audioManager.PlayVoice("W_WrongLiquid");
                gm.GetMistake();
                break;
        }
    }
    //  液體裝滿時通知
    public void LiquidFull(GameObject obj)
    {
        switch (obj.name)
        {
            case "WaterBottle_2-2":
            case "TestTube_2-2":
                obj.SendMessage("Return", SendMessageOptions.DontRequireReceiver);
                if (Status < 2) {
                    Status++; 
                    if(Status == 2)
                        hintManager.SwitchStep("T2_2_2");
                }                    
                break;
        }
    }
    //抓取物件時觸發
    public void Grab(GameObject obj)
    {
        if (obj == tweezers.gameObject)
        {
            tweezers.Grab(true);
        }
        //該步驟如果去觸碰其他物件，給予警告語音。
        switch (Status)
        {
            case 0: //等待寶特瓶液體倒滿
            case 1: //等待試管液體倒滿
                if (obj.name != "WaterBottle_2-2" && obj.name != "SodiumCarbonate_2-2" && obj.name != "TestTube_2-2" && obj.name != "CalciumChloride_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case 2: //兩邊的液體已倒入，待試管放進寶特瓶內
                if (obj.name != "WaterBottle_2-2" && obj.name != "Tweezers_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case 3: //待鎖上寶特瓶蓋後， 畫面跳轉至測量質量頁面。
                if (obj.name != "WaterBottle_2-2" && obj.name != "Cap_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case 4: //待寶特瓶放上電子秤
                if (obj.name != "WaterBottle_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case 5: //待寶特瓶搖晃
                if (obj.name != "WaterBottle_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case 7: //待寶特瓶放上電子秤
                if (obj.name != "WaterBottle_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case 8: //待打開瓶蓋
                if (obj.name != "WaterBottle_2-2" && obj.name != "Cap_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
            case 9: //待瓶蓋放上電子秤
                if (obj.name != "WaterBottle_2-2" && obj.name != "Cap_2-2")
                {
                    audioManager.PlayVoice("W_WrongObject");
                    gm.GetMistake();
                    if (isPC)
                        pcController.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
                }
                break;
        }
    }

    //鬆開物件時觸發
    public void Release(GameObject obj)
    {
        if (obj == tweezers.gameObject)
        {
            tweezers.Grab(false);
        }
    }

    private void EndTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T2_2_8");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
        hintManager.SetTotalStep(0);
    }
}
