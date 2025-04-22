using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exam_2_3 : MonoBehaviour
{
    [Header("實驗道具")]
    [Tooltip("試管")]
    private GameObject testTube;
    [Tooltip("試管內粉末")]
    private LiquidController testTubePowder;
    [Tooltip("試管內碳粉")]
    [SerializeField] private LiquidController testTubeToner;
    [Tooltip("試管內氧化銅粉")]
    [SerializeField] private LiquidController testTubeCopperOxide;
    [Tooltip("試管內粉末顏色")]
    private Color testTubePowderColor;
    [Tooltip("試管內粉末反應後顏色")]
    [SerializeField] private Color testTubePowderColor_final;
    [Tooltip("試管架位置")]
    [SerializeField] private Transform testTubePoint;
    [Tooltip("試管架Trigger")]
    private BoxCollider testTubePointTrigger;
    [Tooltip("盤子內的碳粉")]
    [SerializeField] private GameObject tonerPowder;
    [Tooltip("盤子內的氧化銅粉")]
    [SerializeField] private GameObject copperOxidePowder;
    [Tooltip("碳粉效果")]
    [SerializeField] private GameObject particleSystem_toner;
    [Tooltip("氧化銅粉效果")]
    [SerializeField] private GameObject particleSystem_copperOxide;
    [Tooltip("酒精燈蓋子")]
    [SerializeField] private Animator cap;
    [Tooltip("酒精燈火焰")]
    [SerializeField] private GameObject fire;

    [Header("質量設定")]
    [Tooltip("磅秤文字")]
    [SerializeField] Text weightText;
    [Tooltip("磅秤文字介面")]
    [SerializeField] Text weightTextDisplay;
    [Tooltip("磅秤目前數值")]
    [SerializeField] private float scaleVale = 0f;
    [Tooltip("碳粉重量")]
    [SerializeField] private float weight_toner = 50f;
    [Tooltip("氧化銅粉重量")]
    [SerializeField] private float weight_copperOxide = 50f;
    [Tooltip("反應後的重量")]
    [SerializeField] private float weight_finsh = 80f;

    [Header("UI")]
    [Tooltip("質量文字")]
    [SerializeField] private Text[] massTexts;

    [Tooltip("反應時間")]
    [SerializeField] private float reactionTime = 5f;
    [Tooltip("答題延遲")]
    [SerializeField] private float answerDelay = 3f;
    private float timer = 0f;
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板
    private QuestionManager questionManager;    //管理題目介面
    private AudioManager audioManager;          //音樂管理

    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        audioManager = FindObjectOfType<AudioManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SetTotalStep(4);
        hintManager.SwitchStep("E2_3_1");
        weightText.text = "0g";
        weightTextDisplay.text = "0g";
        testTube = testTubeToner.gameObject;
        testTubePointTrigger = testTubePoint.GetComponent<BoxCollider>();
        testTubePointTrigger.enabled = false;
    }

    private void Update()
    {
        switch (Status)
        {
            case 0://待粉末裝滿
                break;
            case 1://待點燃酒精燈
                break;
            case 2://待夾起試管離開試管架
                float distance = Vector3.Distance(testTube.transform.position, testTubePoint.position);
                if (distance > 0.01f)
                {
                    scaleVale = -10f;
                    weightText.text = scaleVale.ToString("0") + "g";
                    weightTextDisplay.text = scaleVale.ToString("0") + "g";
                    Status++;
                }
                break;
            case 3://待試管加熱完成
                break;
            case 4://待試管放置於架上
                break;
            case 5://回答第一題
                break;
            case 6://回答第二題
                break;
            case 7://回答第三題
                break;
        }
    }

    void CloseHint()    //關閉提示視窗
    {

    }

    public void ReactionStay(GameObject sender)
    {
        switch (Status)
        {
            case 3://試管加熱
                if (sender.name == "TestTube_Mixed_2-6_Clamp")
                {
                    timer += Time.deltaTime;
                    //計算比例 0% > 100%
                    float processPercent = timer / reactionTime;

                    // 使用 Lerp 混合顏色
                    testTubePowderColor = Color.Lerp(testTubePowderColor, testTubePowderColor_final, processPercent);
                    // 更新物件的顏色
                    testTubePowder.liquidColor = testTubePowderColor;
                    testTube.transform.localRotation = Quaternion.Euler(0f, 0f, -30f);
                    if (timer >= reactionTime)
                    {
                        testTube.GetComponent<CollisionDetection>().targetName = "TestTube_point";
                        testTubePointTrigger.enabled = true;
                        hintManager.SwitchStep("E2_3_4");
                        timer = 0f;
                        testTube.transform.localRotation = Quaternion.identity;
                        Status++;
                    }
                }
                break;
            case 4://加熱完的試管放置於架上
                if (sender.name == "TestTube_Mixed_2-6")
                {
                    testTube.tag = "Untagged";
                    testTube.transform.SetParent(testTubePoint);
                    testTube.transform.localPosition = new Vector3(0f, 0f, 0f);
                    testTube.transform.localRotation = Quaternion.identity;

                    //記錄反應後重量
                    scaleVale = weight_finsh;
                    weightText.text = scaleVale.ToString("0") + "g";
                    weightTextDisplay.text = scaleVale.ToString("0") + "g";
                    massTexts[1].text = scaleVale.ToString("0") + "g";
                    questionManager.ShowExamWithDelay(2, answerDelay, gameObject);

                    Status++;
                }
                break;
        }
    }

    //  粉末倒滿時觸發
    public void PowderFull(GameObject sender)
    {
        if (Status != 0)
            return;

        if (testTubeToner.isFull && testTubeCopperOxide.isFull)
        {
            testTube.GetComponent<CollisionDetection>().targetName = "Alcohol Lamp Flame";
            testTube.name = "TestTube_Mixed_2-6";
            testTubePowderColor = testTubePowder.liquidColor;

            tonerPowder.SetActive(false);
            particleSystem_toner.SetActive(false);
            copperOxidePowder.SetActive(false);
            particleSystem_copperOxide.SetActive(false);

            scaleVale = weight_toner + weight_copperOxide;
            //記錄反應前重量
            massTexts[0].text = scaleVale.ToString("0") + "g";
            hintManager.SwitchStep("E2_3_2");
            Status++;
        }
        else
        {
            if (sender == testTubeToner.gameObject)
            {
                print("testTubeToner full");
                testTubePowder = testTubeCopperOxide;
                testTubeCopperOxide.targerRatio = 0f;
                testTubeCopperOxide.currCapacity += testTubeToner.currCapacity;
                testTubeToner.currCapacity = 0f;
                testTubeToner.isFull = true;
                testTubeToner.gameObject.SetActive(false);

                tonerPowder.SetActive(false);
                particleSystem_toner.SetActive(false);
                testTube = testTubeCopperOxide.gameObject;
            }
            else if (sender == testTubeCopperOxide.gameObject)
            {
                print("testTubeCopperOxide full");
                testTubePowder = testTubeToner;
                testTubeToner.targerRatio = 0f;
                testTubeToner.currCapacity += testTubeCopperOxide.currCapacity;
                testTubeCopperOxide.currCapacity = 0f;
                testTubeCopperOxide.isFull = true;
                testTubeCopperOxide.gameObject.SetActive(false);

                copperOxidePowder.SetActive(false);
                particleSystem_copperOxide.SetActive(false);
                testTube = testTubeToner.gameObject;
            }
            testTube.SetActive(true);
            scaleVale = testTubeToner.currCapacity + testTubeCopperOxide.currCapacity;
        }
        weightText.text = scaleVale.ToString("0") + "g";
        weightTextDisplay.text = scaleVale.ToString("0") + "g";
    }

    //抓取物件時觸發
    public void Grab(GameObject obj)
    {
        switch (Status)
        {
            case 0: //待粉末裝滿
                if (!testTubeToner.isFull || !testTubeCopperOxide.isFull)
                {
                    if (obj.name == "Toner_2-6")
                    {
                        testTubeCopperOxide.gameObject.SetActive(false);
                        testTubeToner.currCapacity += testTubeCopperOxide.currCapacity;
                        testTubeCopperOxide.currCapacity = 0f;
                        testTube = testTubeToner.gameObject;
                    }
                    else if (obj.name == "CopperOxide_2-6")
                    {
                        testTubeToner.gameObject.SetActive(false);       
                        testTubeCopperOxide.currCapacity += testTubeToner.currCapacity;
                        testTubeToner.currCapacity = 0f;
                        testTube = testTubeCopperOxide.gameObject;
                    }
                    testTube.SetActive(true);
                }
                break;
        }
    }
    public void OnAlcoholLampTouched()
    {
        if (Status == 1)
        {
            audioManager.PlayVoice("W_AlcoholLamp");
            cap.SetBool("cover", true);
            fire.SetActive(true);

            hintManager.SwitchStep("E2_3_3");
            testTube.tag = "TweezersClamp";
            Status++;
        }
    }

    //答題完畢
    public void FinishExam()
    {
        switch (Status)
        {
            case 5://回答第一題
                questionManager.ShowExamWithDelay(3, answerDelay, gameObject);
                Status++;
                break;
            case 6://回答第二題
                questionManager.ShowExamWithDelay(4, answerDelay, gameObject);
                Status++;
                break;
            case 7://回答第三題
                levelObjManager.LevelClear(2);
                break;
        }
    }
}
