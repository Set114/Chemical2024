using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exam_2_1 : MonoBehaviour
{
    [Tooltip("盤子內的小蘇打粉")]
    [SerializeField] private GameObject bakingSodaPowder;
    [Tooltip("小蘇打粉效果")]
    [SerializeField] private GameObject particleSystem_SodaPowder;
    [Tooltip("燒杯內的小蘇打粉")]
    [SerializeField] private GameObject bakingSodaPowderInWater;

    [Tooltip("盤子")]
    [SerializeField] private GameObject bakingSoda;
    [Tooltip("玻璃棒")]
    [SerializeField] private GameObject glass;
    [Tooltip("抹布")]
    [SerializeField] private GameObject rag;
    private MeshRenderer ragMesh;
    [Tooltip("濕抹布材質")]
    [SerializeField] private Material wetRagMaterial;
    [Tooltip("髒抹布材質")]
    [SerializeField] private Material dirtyRagMaterial;

    [Tooltip("油污")]
    [SerializeField] private MeshRenderer oilStain;

    [Tooltip("倒入小蘇打速度")]
    [SerializeField] private float bakingSodaPourSpeed = 1f;
    [Tooltip("小蘇打溶解速度")]
    [SerializeField] private float bakingSodaDissolveSpeed = 1f;
    [Tooltip("油污消失速度")]
    [SerializeField] private float cleanSpeed = 1f;
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
        hintManager.SwitchStep("E2_1_1");
        ragMesh = rag.GetComponent<MeshRenderer>();
        rag.SetActive(false);
        bakingSodaPowderInWater.transform.localScale = new Vector3(0f, 0f, 0f);        
    }

    public void Reaction(GameObject sender)
    {
        switch (Status)
        {
            case 3: //沾濕抹布
                if (sender.name == "Rag_2-4")
                {
                    ragMesh.material = wetRagMaterial;
                    rag.name = "Rag_Wet_2-4";
                    rag.GetComponent<CollisionDetection>().targetName = "OilStain";
                }
                break;
        }
    }
    public void ReactionStay(GameObject sender)
    {
        switch (Status)
        {
            case 1: //待攪拌完成
                if (sender.name == "Glass_2-4")
                {
                    float size = bakingSodaPowderInWater.transform.localScale.x;
                    size -= Time.deltaTime * bakingSodaDissolveSpeed;
                    size = Mathf.Clamp(size, 0f, 1f);
                    bakingSodaPowderInWater.transform.localScale = new Vector3(size, size, size);
                    if(size <= 0)
                    {
                        //跳出題目一
                        questionManager.ShowExamWithDelay(0, answerDelay, gameObject);
                        hintManager.OnCloseBtnClicked();
                        sender.SendMessage("Return", SendMessageOptions.DontRequireReceiver);
                        Status++;
                    }
                }
                break;
            case 3: //待擦拭桌子
                if (sender.name == "Rag_Wet_2-4")
                {
                    Color oilColor = oilStain.material.color;
                    oilColor.a -= Time.deltaTime * cleanSpeed;
                    oilStain.material.color = oilColor;
                    if (oilColor.a <= 0f)
                    {
                        oilColor.a = 0f;
                        oilStain.material.color = oilColor;
                        hintManager.SwitchStep("E2_1_3");
                        hintManager.ShowNextButton(this.gameObject);
                        Status++;
                    }
                }
                break;
        }
    }

    public void PourSodaPowder()
    {
        if (Status == 0)
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
                Status++;
            }
        }
    }
    //答題完畢
    public void FinishExam()
    {
        switch (Status)
        {
            case 2: //待攪拌完成
                hintManager.SwitchStep("E2_1_2");
                bakingSoda.SetActive(false);
                glass.SetActive(false);
                rag.SetActive(true);
                Status++;
                break;
        }  
    }

    public void CloseHint()
    {
        levelObjManager.LevelClear(0);
    }
}
