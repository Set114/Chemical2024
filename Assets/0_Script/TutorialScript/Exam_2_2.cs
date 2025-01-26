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
    float bakingSodaPowderInWaterAlready;   //已經放入水裡的小蘇打、檸檬粉
    float bakingSodaPowderInWaterCurrent;   //目前水裡的小蘇打、檸檬粉
    [Tooltip("水中氣泡效果")]
    [SerializeField] private GameObject bubble;
    [Tooltip("水面氣泡效果")]
    [SerializeField] private GameObject sodaBubble;

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

        bakingSodaPowderInWater.transform.localScale = new Vector3(0f, 0f, 0f);
        tv.SetActive(true);
        audioManager.PlayVoice("E2_2_1");
        objects.SetActive(false);

        bakingSodaPowderInWaterAlready = bakingSodaPowderInWater.transform.localScale.x;
        bakingSodaPowderInWaterCurrent = bakingSodaPowderInWaterAlready;
    }

    private void Update()
    {
        ParticleSystem.EmissionModule emission;
        switch (Status)
        {
            case 2: //待小蘇打粉倒入
                bakingSodaPowderInWaterCurrent -= Time.deltaTime * bakingSodaDissolveSpeed;
                bakingSodaPowderInWater.transform.localScale = Vector3.one * bakingSodaPowderInWaterCurrent;
                //控制氣泡大小
                emission = bubble.GetComponent<ParticleSystem>().emission;
                emission.rateOverTime = bakingSodaPowderInWaterCurrent / 0.15f * 500.0f;
                //控制水面上氣泡大小
                emission = sodaBubble.GetComponent<ParticleSystem>().emission;
                if (bakingSodaPowderInWaterCurrent > 0.05)
                    emission.rateOverTime = 500.0f;
                else
                    emission.rateOverTime = 10.0f;
                break;
            case 3: //等待反應
                bakingSodaPowderInWaterCurrent -= Time.deltaTime * bakingSodaDissolveSpeed;
                bakingSodaPowderInWater.transform.localScale = Vector3.one * bakingSodaPowderInWaterCurrent;
                //控制氣泡大小
                emission = bubble.GetComponent<ParticleSystem>().emission;
                emission.rateOverTime = bakingSodaPowderInWaterCurrent / 0.15f * 500.0f;
                //控制水面上氣泡大小
                emission = sodaBubble.GetComponent<ParticleSystem>().emission;
                if (bakingSodaPowderInWaterCurrent > 0.05)
                    emission.rateOverTime = 500.0f;
                else
                    emission.rateOverTime = 10.0f;

                if (bakingSodaPowderInWaterCurrent <= 0)
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
            bubble.SetActive(true);
            sodaBubble.SetActive(true);
            bakingSodaPowderInWaterAlready += Time.deltaTime * bakingSodaPourSpeed;
            bakingSodaPowderInWaterCurrent += Time.deltaTime * bakingSodaPourSpeed;
            bakingSodaPowderInWaterAlready = Mathf.Clamp(bakingSodaPowderInWaterAlready, 0f, 1f);
            bakingSodaPowderInWaterCurrent = Mathf.Clamp(bakingSodaPowderInWaterCurrent, 0f, 1f);

            if (bakingSodaPowderInWaterAlready >= 1f)
            {
                bakingSodaPowder.SetActive(false);
                bakingSodaPowderInWater.SetActive(true);
                particleSystem_SodaPowder.SetActive(false);
                
                Status++;
            }
        }
    }

    public void CloseHint()    //關閉提示視窗
    {
        switch (Status)
        {
            case 0: //關閉電視廣告
                hintManager.SwitchStep("E2_2_2");
                hintManager.ShowNextButton(gameObject);
                tv.SetActive(false);
                Status++;
                break;
            case 1: //關閉語音及文字提示                
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
