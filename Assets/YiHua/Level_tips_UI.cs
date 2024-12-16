using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Level_tips_UI : MonoBehaviour
{
    //參考IceBlockCollision.cs
    public string iceTag = "ice";
    public string glassTag = "glass";
    public string ragTag = "rag";

    //參考WaterandGlass.cs
    private bool isIceEntered = false;  // 記錄小蘇打進入
    private bool isGlassEnteredAfterIce = false;  // 記錄玻璃棒進入
    private bool ragTime = false;  // 記錄玻璃棒進入
    public GameObject cone;
    public GameObject rag;

    [Header("Mark")] //獲得問題球 物件
    public GameObject q1questionMark;

    [Header("UI")] //獲得UI物件
    public GameObject UI01; // 整個 UI，保持開啟
    public GameObject UI01_Text;  // 只顯示倒數文字的部分（TextMeshPro）
    public GameObject UI02;
    public GameObject UI03;



    [Header("Button")] //獲得問題球 按鈕
    public Button q1question_btn;
    public Button Close_btn;

    [Header("LoadingSign")]
    [SerializeField] GameObject loading_sign;

    public LevelEndSequence levelEndSequence;
    public TestDataManager testDataManager;
    public IceBlockCollisionStage1UI iceBlockCollisionStage1UI;
    int count = 0;

    // 用於倒數的變數
    private float countdownTime = 10f;  // 設定倒數時間
    private bool isCounting = false;   // 判斷是否正在倒數
    private float currentTime;         // 當前倒數時間
    private bool hasStartedCountdown = false; // 確保倒數只啟動一次

    void Start()
    {
        q1question_btn.onClick.AddListener(Q1question);
        Close_btn.onClick.AddListener(Close_controler);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(iceTag)) //如果小蘇打加入水中 觸發
        {


            if (count == 0)
            {
                isIceEntered = true;  //標記小蘇打進入
                UI01.SetActive(true); //開啟UI01物件 提示要用玻璃棒攪拌
                //Debug.Log("小蘇打碰到水");

            }
        }

        if (other.CompareTag(glassTag) && isIceEntered) //如果玻璃棒碰到水中 觸發
        {
            if (count == 0 && !hasStartedCountdown)
            {
                isGlassEnteredAfterIce = true; // 標記玻璃棒進入
                UI01.SetActive(true); // 保持UI01顯示
                UI01_Text.SetActive(true); // 顯示倒數文本

                // 開始倒數計時
                StartCountdown();
                //提示UI 開始倒數10秒  
                //小蘇打粉越來越消失 10秒後 跳出問題一的球Q1 然後出現一坨小人講話 選題  然後結束小人Q1 繼續 做抹布
                hasStartedCountdown = true; // 確保倒數只啟動一次
                TestDataStart(0);
                //Debug.Log("玻璃棒碰到水");

            }
        }

        if (other.CompareTag(ragTag) && isGlassEnteredAfterIce && ragTime) //如果抹布碰到水 觸發   是不是應該再油污上面也做一個 如果被有沾過水的抹布碰到 過3秒消失
        {
            //Debug.Log("抹布碰到水");

            // 修改 rag 物件的標籤為 "rag_water"
            if (rag != null)
            {
                rag.tag = "rag_water";
                //Debug.Log("rag物件的標籤已經更改為 rag_water");
                UI02.SetActive(false);
                UI03.SetActive(true);
            }
            TestDataStart(0);
        }
    }

    // 開始倒數計時 10 秒
    private void StartCountdown()
    {
        currentTime = countdownTime;  // 重置倒數時間
        isCounting = true;  // 開始倒數
        StartCoroutine(CountdownCoroutine());  // 啟動倒數協程
    }

    // 倒數計時協程
    private IEnumerator CountdownCoroutine()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;  // 減少時間
            UpdateCountdownText();  // 更新顯示的倒數時間
            yield return null;  // 等待下一幀
        }

        // 當倒數結束時
        OnCountdownFinished();
    }

    // 更新 UI 中顯示的倒數時間
    private void UpdateCountdownText()
    {
        if (UI01_Text != null)
        {
            TextMeshProUGUI countdownText = UI01_Text.GetComponent<TextMeshProUGUI>();
            countdownText.text = "請攪拌" + Mathf.Ceil(currentTime) + "秒";  // 顯示倒數時間
        }
    }

    // 倒數結束後的處理
    private void OnCountdownFinished()
    {
        // 倒數完
        isCounting = false;  // 停止倒數
        cone.SetActive(false);
        Q1MarkShow(); //觸發問題Q1
        UI01.SetActive(false); // UI01顯示關閉
        UI01_Text.SetActive(false); // 倒數文本關閉

    }



    //第Q1Mark開始
    public void Q1MarkShow()
    {
        q1questionMark.SetActive(true);        //顯示 q1questionMark 物件
    }

    void Q1question()
    {
        q1questionMark.SetActive(false);       //隱藏 q1questionMark 物件
        iceBlockCollisionStage1UI.ShowQuestionUI(count); //呼叫 iceBlockCollisionStage1UI 這個物件中的 ShowQuestionUI 方法，並且傳入參數 count
    }



    //控制結算
    void Close_controler()
    {
        StartCoroutine(WaitAndStartNextLevel());
    }

    private IEnumerator WaitAndStartNextLevel()
    {
        loading_sign.SetActive(true);  // 顯示加載標誌
        TestDataEnd(() =>
        {
            TestDataStart(1);
            loading_sign.SetActive(false); // 隱藏加載標誌
            UI02.SetActive(true);
            ragTime = true;
        });

        yield return null;
    }

    public void TestDataStart(int countindex)
    {
        testDataManager.StartLevel();
        testDataManager.GetsId(countindex);
    }

    public void TestDataEnd(Action callback)
    {
        testDataManager.CompleteLevel();
        testDataManager.EndLevelWithCallback(callback);
        // testDataManager.EndLevel();
    }
}
