using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam_2_2 : MonoBehaviour
{
    [Tooltip("電視")]
    [SerializeField] private GameObject tv;
    [Tooltip("實驗道具")]
    [SerializeField] private GameObject objects;

    [Tooltip("盤子內的小蘇打粉")]
    [SerializeField] private GameObject bakingSodaPowder;
    [Tooltip("小蘇打粉效果")]
    [SerializeField] private GameObject particleSystem_SodaPowder;
    [Tooltip("燒杯內的小蘇打粉")]
    [SerializeField] private GameObject bakingSodaPowderInWater;
    [Tooltip("泡沫效果")]
    [SerializeField] private GameObject bubble;

    [Tooltip("倒入小蘇打速度")]
    [SerializeField] private float bakingSodaPourSpeed = 1f;
    [Tooltip("小蘇打溶解速度")]
    [SerializeField] private float bakingSodaDissolveSpeed = 1f;
    [Tooltip("答題延遲")]
    [SerializeField] private float answerDelay = 3f;
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
        hintManager.ShowNextButton(gameObject);
        bakingSodaPowderInWater.transform.localScale = new Vector3(0f, 0f, 0f);
        tv.SetActive(true);
        objects.SetActive(false);
    }

    private void Update()
    {
        switch (Status)
        {
            case 2: //待小蘇打粉倒入
                break;
            case 3: //等待反應
                float size = bakingSodaPowderInWater.transform.localScale.x;
                size -= Time.deltaTime * bakingSodaDissolveSpeed;
                size = Mathf.Clamp(size, 0f, 1f);
                bakingSodaPowderInWater.transform.localScale = new Vector3(size, size, size);
                if (size <= 0)
                {
                    //跳出題目
                    questionManager.ShowExamWithDelay(1, answerDelay, gameObject);
                    Status++;
                }
                break;
        }
    }

    public void PourSodaPowder()
    {
        if (Status == 2)
        {
            float size = bakingSodaPowderInWater.transform.localScale.x;
            size += Time.deltaTime * bakingSodaPourSpeed;
            size = Mathf.Clamp(size, 0f, 1f);
            bakingSodaPowderInWater.transform.localScale = new Vector3(size, size, size);

            if (size >= 1f)
            {
                bakingSodaPowder.SetActive(false);
                bakingSodaPowderInWater.SetActive(true);
                particleSystem_SodaPowder.SetActive(false);
                bubble.SetActive(true);
                Status++;
            }
        }
    }

    void CloseHint()    //關閉提示視窗
    {
        switch (Status)
        {
            case 0: //關閉電視廣告
                hintManager.SwitchStep("E2_2_2");
                hintManager.ShowNextButton(gameObject);
                Status++;
                break;
            case 1: //關閉語音及文字提示
                tv.SetActive(false);
                objects.SetActive(true);
                hintManager.SwitchStep("E2_2_3");
                Status++;
                break;
        }

    }

    //答題完畢
    public void FinishExam()
    {
        levelObjManager.LevelClear(0);
    }
}
