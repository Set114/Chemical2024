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

    float timer;                    //計時器
    float progressRatio = 0.0f;     //進度比例
    


    LevelObjManager levelObjManager;
    HintManager hintManager;            //管理提示板
    MoleculaDisplay moleculaManager;    //管理分子螢幕
    AudioManager audioManager;          //音樂管理
    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        audioManager = FindObjectOfType<AudioManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("E1_2");

        timer = 0.0f;
        //蠟燭回歸初始
        candleMeshRenderer.SetBlendShapeWeight(0, 0.0f);
        candleMeshRenderer.SetBlendShapeWeight(1, 0.0f);
        ropeObject.position = ropeStartPosition.position;
        //moleculaManager.ShowMoleculas(0);
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;
        //蠟燭動畫控制
        if (timer < totalBurnTime)
        {
            progressRatio = 1.0f - timer / totalBurnTime;
            candleMeshRenderer.SetBlendShapeWeight(0, progressRatio * 100.0f);
            candleMeshRenderer.SetBlendShapeWeight(1, progressRatio * 100.0f);
            ropeObject.position = Vector3.Lerp(ropeStartPosition.position, ropeFinalPosition.position, progressRatio);
        }
        else
        {
            progressRatio = 0.0f;
        }        
    }
}
