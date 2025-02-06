using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam_1_2 : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer candleMeshRenderer; //蠟燭物件的SkinnedMeshRenderer
    [SerializeField] float totalBurnTime;           //燃燒時間
    [SerializeField] Transform ropeObject;          //燭芯物件
    [SerializeField] Transform ropeStartPosition;   //燭芯最高位置
    [SerializeField] Transform ropeFinalPosition;   //燭芯最低位置
    [SerializeField] GameObject QuestionMark_04;    //問題物件
    [SerializeField] GameObject QuestionMark_05;    //問題物件

    float timer;                    //計時器
    float progressRatio = 0.0f;     //進度比例
    [Tooltip("答題延遲")]
    [SerializeField] private float answerDelay = 3f;
    int Status = 0;

    LevelObjManager levelObjManager;
    QuestionManager questionManager; //管理題目介面
    private ZoomDisplay zoomDisplay;            //管理近看視窗
    AudioManager audioManager;          //音樂管理
    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        zoomDisplay = FindObjectOfType<ZoomDisplay>();
        audioManager = FindObjectOfType<AudioManager>();

        timer = 0.0f;
        //蠟燭回歸初始
        candleMeshRenderer.SetBlendShapeWeight(0, 0.0f);
        candleMeshRenderer.SetBlendShapeWeight(1, 0.0f);
        ropeObject.position = ropeStartPosition.position;
        Status = 0;
        QuestionMark_04.GetComponent<Canvas>().worldCamera = Camera.main;
        QuestionMark_05.GetComponent<Canvas>().worldCamera = Camera.main;
        QuestionMark_04.SetActive(false);
        QuestionMark_05.SetActive(false);
        //moleculaManager.ShowMoleculas(0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (Status)
        {
            case 0: //燃燒
                timer = timer + Time.deltaTime;
                if (timer > 5.0f)
                {
                    QuestionMark_04.SetActive(true);
                    Status++;
                }
                break;
            case 1: //等待回答問題

                break;
            case 2: //繼續燃燒
                timer = timer + Time.deltaTime;
                if (timer > 10.0f)
                {
                    QuestionMark_05.SetActive(true);
                    Status++;
                }
                break;
        }

        //蠟燭動畫控制
        if (timer < totalBurnTime)
        {
            progressRatio = timer / totalBurnTime;
            candleMeshRenderer.SetBlendShapeWeight(0, progressRatio * 100.0f);
            candleMeshRenderer.SetBlendShapeWeight(1, progressRatio * 100.0f);
            ropeObject.position = Vector3.Lerp(ropeStartPosition.position, ropeFinalPosition.position, progressRatio);
        }
        else
        {
            progressRatio = 0.0f;
        }
    }

    public void ShowExam()
    {
        switch (Status)
        {
            case 1:
                questionManager.ShowExamWithDelay(3, answerDelay, gameObject);
                QuestionMark_04.SetActive(false);
                break;
            case 3:
                questionManager.ShowExamWithDelay(4, answerDelay, gameObject);
                QuestionMark_05.SetActive(false);
                break;
        }
    }

    public void FinishExam()
    {
        switch (Status)
        {
            case 1:
                zoomDisplay.ShowZoomObj(2);
                zoomDisplay.PlayAnimation();
                break;
            case 3:
                levelObjManager.LevelClear(2);
                break;
        }
        Status++;
    }
}
