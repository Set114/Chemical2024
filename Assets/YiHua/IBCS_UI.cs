using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;


public class IBCS_UI : MonoBehaviour
{
    //參考IceBlockCollision.cs
    public string iceTag = "ice";


    //參考WaterandGlass.cs
    private bool isIceEntered = false;  // 記錄小蘇打進入
    public GameObject cone;


    [Header("Mark")] //獲得問題球 物件
    public GameObject q1questionMark;

    [Header("GameObject")] //獲得物件
    public GameObject UI01; // 提示 將粉放入水中
    public GameObject UI02; //泡泡特效
    public GameObject Q1_UI; //選擇題
    public GameObject Canvas_Test; //Canvas_Test
    public GameObject Canvas_Test_UI; //Canvas_Test 的UI


    [Header("level")]
    //public GameObject next_level; //下一個場景第一個要出現的東西
    public GameObject next_level203;    //下一關卡的全部物件
    public GameObject next_level202;    //當前關卡的全部物件
    //public GameObject next_level_Canvas_Test;   //當前關卡物件的上一層




    [Header("Button")] //獲得問題球 按鈕
    public Button q1question_btn;
    public Button Close_btn;

    [Header("LoadingSign")]
    [SerializeField] GameObject loading_sign;

    public LevelEndSequence levelEndSequence;
    public TestDataManager testDataManager;

    int count = 0;

    // 用於倒數的變數
    private float countdownTime = 10f;  // 設定倒數時間
    private bool isCounting = false;   // 判斷是否正在倒數
    private float currentTime;         // 當前倒數時間
    private bool hasStartedCountdown = false; // 確保倒數只啟動一次

    private LevelObjManager levelObjManager;

    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        q1question_btn.onClick.AddListener(Q1question);
        Close_btn.onClick.AddListener(Close_controler);
        Renderer renderer = next_level202.GetComponent<Renderer>(); //為了隱藏當前關卡並繼續執行
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(iceTag)) //如果小蘇打加入水中 觸發
        {
            UI01.SetActive(false); //關閉提示
            isIceEntered = true;  //標記小蘇打進入
            UI02.SetActive(true); //開啟UI01物件 泡泡
            //Debug.Log("小蘇打碰到水");

            StartCountdown();

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
            yield return null;  // 等待下一幀
        }

        // 當倒數結束時
        OnCountdownFinished();
    }



    // 倒數結束後的處理
    private void OnCountdownFinished()
    {
        UI01.SetActive(false); //關閉提示
        // 倒數完
        isCounting = false;  // 停止倒數
        cone.SetActive(false);
        Q1MarkShow(); //觸發問題Q1
    }

    //第Q1Mark開始
    public void Q1MarkShow()
    {
        q1questionMark.SetActive(true);        //顯示 q1questionMark 物件
    }

    void Q1question()
    {
        q1questionMark.SetActive(false);
        q1question_btn.gameObject.SetActive(false);
        Canvas_Test_UI.SetActive(false);
        Canvas_Test.SetActive(true);
        Q1_UI.SetActive(true);

    }

    //控制結算
    void Close_controler()
    {
        StartCoroutine(WaitAndStartNextLevel());
    }

    private IEnumerator WaitAndStartNextLevel()
    {
        //next_level202.SetActive(false);  //關閉顯示 目前關卡
        HideAllRenderers(next_level202);//關閉顯示 目前關卡
        loading_sign.SetActive(true);  // 顯示加載標誌
        TestDataEnd(() =>
        {
            TestDataStart(1);
            loading_sign.SetActive(false); // 隱藏加載標誌

        });

        yield return null;

        Debug.Log("下一個場景");
        /*
        levelEndSequence.EndLevel(false, false, 1f, 3f, 1f, "1", () =>
        {
            Canvas_Test.SetActive(true);//開Canvas_Test
            Canvas_Test_UI.SetActive(true); //開下一關首要登場
            next_level203.SetActive(true);  //開 下一關
            next_level202.SetActive(false);  //關閉 目前關卡
        });
        */
        levelObjManager.LevelClear("1", "");
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

    public void HideAllRenderers(GameObject parentObject)
    {
        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
    }

    public void ShowAllRenderers(GameObject parentObject)
    {
        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = true;
        }
    }

}
