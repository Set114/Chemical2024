using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam_1_1 : MonoBehaviour
{
    [Tooltip("乾冰")]
    [SerializeField] private GameObject dryIce;
    [Tooltip("乾冰特效")]
    [SerializeField] private GameObject smoke_dryIce;
    [Tooltip("冒泡特效")]
    [SerializeField] private GameObject bubble_dryIce;
    [Tooltip("水面特效")]
    [SerializeField] private GameObject smoke_water;
    [Tooltip("試紙")]
    [SerializeField] private Animator paper;
    [Tooltip("問號按鈕")]
    [SerializeField] private GameObject[] questionMarks;

    [Tooltip("縮小比例")]
    [SerializeField] private float smallerRatio = 0.5f;
    private float dryIceMinScale;
    private float dryIceScale;

    [Tooltip("乾冰反應時間")]
    [SerializeField] private float dryIceReactionTime = 5.0f;
    [Tooltip("試紙反應時間")]
    [SerializeField] private float paperReactionTime = 6f;

    [Tooltip("答題延遲")]
    [SerializeField] private float answerDelay = 3f;
    private float timer = 0.0f;
    private bool glassWet = false;
    private int examCount = 0;                  //作題進度

    private LevelObjManager levelObjManager;
    private QuestionManager questionManager;    //管理題目介面
    private HintManager hintManager;            //管理提示板

    int Status = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("E1_1_1");

        smoke_dryIce.SetActive(true);
        smoke_water.SetActive(false);
        dryIceScale = dryIce.transform.localScale.x;
        dryIceMinScale = dryIceScale * smallerRatio;
        foreach (GameObject questionMark in questionMarks)
        {
            questionMark.SetActive(false);
        }

        Status = 0;
        examCount = 0;
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //等待乾冰放入

                break;
            case 1: //乾冰已放入
                timer += Time.deltaTime;
                if (timer > dryIceReactionTime)
                {
                    timer = 0.0f;
                    questionMarks[0].SetActive(true);
                    Status++;
                }
                break;
            case 2: //乾冰在水中反應完畢，Q1出現

                break;
            case 3: //回答Q1完畢，等待Q2出現
                timer += Time.deltaTime;
                if (timer > 2.0f)
                {
                    questionMarks[1].SetActive(true);
                    Status++;
                }                 
                break;
            case 4: //乾冰在水中反應完畢，Q2出現

                break;
            case 5: //回答Q2完畢，等待玻璃棒觸碰水

                break;
            case 6: //等待玻璃棒觸碰試紙

                break;
            case 7: //試紙開始變化
                timer += Time.deltaTime;
                if (timer > paperReactionTime)
                {
                    ShowExam(2);
                    Status++;
                }                    
                break;
            case 8: //結束狀態

                break;
        }
    }

    //  開始反應
    public void Reaction(GameObject sender)
    {
        switch (Status)
        {
            case 0:
                if (sender.name == "DryIce")
                {
                    smoke_water.SetActive(true);
                    smoke_dryIce.SetActive(false);
                    bubble_dryIce.SetActive(true);
                    dryIce.AddComponent<MotionTimeScale>();
                    hintManager.OnCloseBtnClicked();
                    Status++;
                }
                break;
            case 5:
                if (sender.name == "Glass")
                {
                    glassWet = true;
                    Status ++;
                }
                break;
            case 6:
                if (sender.name == "Paper" && glassWet)
                {
                    paper.SetTrigger("isClick");
                    timer = 0.0f;
                    hintManager.OnCloseBtnClicked();
                    Status ++;
                }
                break;
        }
    }

    public void ShowExam(int index)
    {
        questionManager.ShowExamWithDelay(index, answerDelay, gameObject);
        foreach(GameObject questionMark in questionMarks)
        {
            questionMark.SetActive(false);
        }
    }

    public void EndTheTutorial()
    {
        hintManager.ShowNextButton(this.gameObject);
    }

    //  關閉提示視窗   感覺沒用到
    /*
    void CloseHint()
    {
        if (paperReactionDone)
        {
            levelObjManager.LevelClear(0);
        }
        else if (dryIceReactionDone)
        {

        }
    }
    */

    //  回答完畢
    void FinishExam()
    {
        examCount++;
        switch (examCount)
        {
            case 1:
                Status = 3;
                break;
            case 2:
                Status = 5;
                hintManager.SwitchStep("E1_1_2");
                break;
            case 3:
                levelObjManager.LevelClear(0);
                break;
        }
    }

    //  以下是原先紀錄測驗資料的流程，待修改
    /*
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
            //levelEndSequence.EndLevel(false,false, 1f, 2f, 1f, "1", () => {iceBlockCollisionStage1UI.TestUIShow();});
            levelObjManager.LevelClear("1", "");
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
    */
}
