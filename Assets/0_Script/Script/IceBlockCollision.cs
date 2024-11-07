using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System; 


public class IceBlockCollision : MonoBehaviour
{
    public GameObject Show1;
    public GameObject Show2;
    public string targetTag = "Object";

    [Header("Mark")]
    public GameObject q1questionMark;
    public GameObject q2questionMark;
    [Header("Button")]
    public Button q1question_btn;
    public Button q2question_btn;
    public Button Close_btn;
    [Header("LoadingSign")]
    [SerializeField] GameObject loading_sign;

    public LevelEndSequence levelEndSequence;
    public TestDataManager testDataManager;
    public IceBlockCollisionStage1UI iceBlockCollisionStage1UI;
    int count = 0;

    void Start()
    {
        q1question_btn.onClick.AddListener(Q1question);
        q2question_btn.onClick.AddListener(Q2question);
        Close_btn.onClick.AddListener(Close_controler);
    }       

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Show1.SetActive(true);
            Show2.SetActive(true);
            if(count == 0)
            {
                Q1MarkShow();       
                TestDataStart(0);     
            }
        }
    }
    //第Q1Mark開始
    public void Q1MarkShow()
    {
        q1questionMark.SetActive(true);        
    }

    void Q1question()
    {
        q1questionMark.SetActive(false);
        iceBlockCollisionStage1UI.ShowQuestionUI(count); 
    }

    //第Q2Mark開始
    public void Q2MarkShow()
    {
        q2questionMark.SetActive(true);
    }

    // 當Q2問題按鈕被點擊時
    void Q2question()
    {
        q2questionMark.SetActive(false);
        iceBlockCollisionStage1UI.ShowQuestionUI(count); 
    }

    // // 當Q3問題按鈕被點擊時
    void Q3question()
    {
        iceBlockCollisionStage1UI.ShowQuestionUI(count);   
    }

    //控制結算
    void Close_controler()
    {
        StartCoroutine(WaitAndStartNextLevel());
    }

    private IEnumerator WaitAndStartNextLevel()
    {
        if (count == 0)
        {
            loading_sign.SetActive(true);
            TestDataEnd(() => {
                Q2MarkShow();
                TestDataStart(1);
                loading_sign.SetActive(false);
            });
        }
        else if (count == 1)
        {
            loading_sign.SetActive(true);
            TestDataEnd(() => {
                TestDataStart(2);
                iceBlockCollisionStage1UI.T213ShowUI();
                loading_sign.SetActive(false);
            });
        }
        else if (count == 2)
        {
            levelEndSequence.EndLevel(false,false, 1f, 2f, 1f, "1", () => {iceBlockCollisionStage1UI.TestUIShow();});
        }
        count++;
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
