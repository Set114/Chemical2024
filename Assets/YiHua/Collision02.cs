using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System;


public class Collision02 : MonoBehaviour
{
    [Header("Tag")]
    public string tag_test_tube02 = "tag_test_tube02"; //test_tube02 燒燈上那隻

    [Header("GameObject")]
    public GameObject test_tube02; //test_tube02 燒燈上那隻
    public GameObject test_tube03; //test_tube03 磅秤上那隻

    
    public GameObject hints02;//提示放回磅秤上
    public GameObject eighty_grams;//80克UI
    public GameObject stepUI;//步驟UI
    public GameObject Transform02; //空的碰撞框偵測 在磅秤上

    [Header("Canvas")]
    public GameObject Q1_UI; //選擇題
    public GameObject Q2_UI;
    public GameObject Q3_UI;
    public GameObject Canvas_Test; //Canvas_Test
    public GameObject Canvas_Test_UI; //Canvas_Test 的UI
    public GameObject Canvas_Test_UI_END; //END

    [Header("Mark")]
    public GameObject q1questionMark;//獲得問題球 物件
    public Button q1question_btn;
    public Button Close_btn;
    [Header("LoadingSign")]
    [SerializeField] GameObject loading_sign;
    public GameObject next_level203;//當前關卡2-3
    public LevelEndSequence levelEndSequence;
    public TestDataManager testDataManager;

    private int currentQuestionIndex = 1; // 當前題目索引（從1開始）

    void Start()
    {

        q1question_btn.onClick.AddListener(OnQ1MarkClicked);
        Close_btn.onClick.AddListener(Close_controler);
        loading_sign.SetActive(true);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag_test_tube02))
        {
            test_tube02.SetActive(false);//關閉 拿在手上 燒燈那隻
            test_tube03.SetActive(true);//開啟磅秤那隻
            hints02.SetActive(false);//關閉放回磅秤提示
            eighty_grams.SetActive(true);//開啟80克UI
            stepUI.SetActive(false); //步驟UI關閉
            next_level203.SetActive(false);//關卡物件關閉
            StartQuestion1Mark();
        }
    }

    //第Q1Mark開始
    void StartQuestion1Mark()
    {
        // 啟用問題 1 的標誌，等待玩家點擊
        q1questionMark.SetActive(true);
        Canvas_Test_UI.SetActive(false);
        Canvas_Test.SetActive(false);
    }

    void OnQ1MarkClicked()
    {
        // 關閉問題 1 的標誌並顯示問題 UI
        q1questionMark.SetActive(false);
        q1question_btn.gameObject.SetActive(false);
        StartQuestion1();
    }
    void StartQuestion1()
    {
        Q1_UI.SetActive(true);
        Canvas_Test.SetActive(true);
        Canvas_Test_UI.SetActive(false);
        TestDataStart(0); // 啟動數據管理的第一題
    }
    void StartQuestion2()
    {
        Q1_UI.SetActive(false);
        Q2_UI.SetActive(true);
        TestDataStart(1); // 啟動數據管理的第二題
    }
    void StartQuestion3()
    {
        Q2_UI.SetActive(false);
        Q3_UI.SetActive(true);
        TestDataStart(2); // 啟動數據管理的第三題
    }


    //控制結算
    void Close_controler()
    {
        StartCoroutine(WaitAndStartNextLevel());
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
    }
    private IEnumerator WaitAndStartNextLevel()
    {

        // loading_sign.SetActive(true); // 顯示加載標誌
        TestDataEnd(() =>
        {
            // loading_sign.SetActive(false); // 隱藏加載標誌
            currentQuestionIndex++; // 更新當前題目索引

            if (currentQuestionIndex == 2)
            {
                StartQuestion2();
            }
            else if (currentQuestionIndex == 3)
            {
                StartQuestion3();
            }
            else
            {
                EndAllQuestions();
            }
        });

        yield return null;

    }
    void EndAllQuestions()
    {
        Debug.Log("所有題目完成，進入結算流程");
        levelEndSequence.EndLevel(false, false, 1f, 3f, 1f, "1", () =>
        {
            Canvas_Test.SetActive(true);
            Canvas_Test_UI.SetActive(true);
            Canvas_Test_UI_END.SetActive(true);
        });
    }

}