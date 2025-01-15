using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam_2_3 : MonoBehaviour
{

    private int Status = 0;

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板
    private QuestionManager questionManager; //管理題目介面
    private AudioManager audioManager;          //音樂管理

    // Start is called before the first frame update
    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        audioManager = FindObjectOfType<AudioManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("E2_2_1");
    }

    private void Update()
    {
        switch (Status)
        {
            case 0:
                break;
        }
    }


    void CloseHint()    //關閉提示視窗
    {

    }

    //答題完畢
    public void FinishExam()
    {
        levelObjManager.LevelClear(0);
    }
}
