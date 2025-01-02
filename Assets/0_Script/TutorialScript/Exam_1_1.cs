using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam_1_1 : MonoBehaviour
{
    [Tooltip("乾冰")]
    [SerializeField] private GameObject dryIce;
    [Tooltip("乾冰特效")]
    [SerializeField] private GameObject smoke_dryIce;
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
    [SerializeField] private float dryIceReactionTime = 6f;
    [Tooltip("試紙反應時間")]
    [SerializeField] private float paperReactionTime = 6f;

    private float timer = 0.0f;
    private bool dryIceReaction = false;
    private bool dryIceReactionDone = false;
    private bool glassWet = false;
    private bool paperReaction = false;
    private bool paperReactionDone = false;
    private int examCount = 0;

    private LevelObjManager levelObjManager;
    private QuestionManager questionManager;    //管理題目介面
    private HintManager hintManager;            //管理提示板

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

    }

    private void Update()
    {
        if (paperReactionDone)
            return;


        if (dryIceReaction && !dryIceReactionDone)
        {
            timer += Time.deltaTime;

            if(dryIceScale > dryIceMinScale)
            {
                dryIceScale -= Time.deltaTime*(dryIceReactionTime * smallerRatio);
                dryIce.transform.localScale = new Vector3(dryIceScale, dryIceScale, dryIceScale);
            }

            if (timer > dryIceReactionTime)
            {
                //  乾冰在水中反應完畢
                dryIceReactionDone = true;
                timer = 0f;
                QuestionMarkShow(0);
            }
        }

        if (paperReaction && dryIceReactionDone)
        {
            timer += Time.deltaTime;

            if (timer > paperReactionTime)
            {
                paperReactionDone = true;
                ShowExam(2);
            }
        }
    }

    //  開始反應
    public void Reaction(GameObject sender)
    {
        if(sender.name == "DryIce")
        {
            if (!dryIceReaction)
            {
                dryIceReaction = true;
                smoke_water.SetActive(true);
            }
        }
        else if(dryIceReactionDone)
        {
            if (sender.name == "Glass")
            {
                glassWet = true;
            }
            else if (sender.name == "Paper" && glassWet && !paperReaction)
            {
                paper.SetBool("move", true);
                paperReaction = true;
            }
        }
    }

    public void QuestionMarkShow(int index)
    {
        questionMarks[index].SetActive(true);
    }

    public void ShowExam(int index)
    {
        questionManager.ShowExam(index, gameObject);
        foreach(GameObject questionMark in questionMarks)
        {
            questionMark.SetActive(false);
        }
    }

    public void EndTheTutorial()
    {
        hintManager.ShowNextButton(this.gameObject);
    }

    //  關閉提示視窗
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
    //  回答完畢
    void FinishExam()
    {
        examCount++;
        switch (examCount)
        {
            case 0:
                break;
            case 1:
                QuestionMarkShow(1);
                break;
            case 2:
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
