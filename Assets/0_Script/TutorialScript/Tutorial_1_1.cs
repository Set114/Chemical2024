using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_1_1 : MonoBehaviour
{
    [SerializeField] GameObject iron;           //軟鐵片
    [SerializeField] GameObject hammer;         //槌子
    [SerializeField] GameObject[] ironObjects;  //軟鐵片的四個狀況
    [SerializeField] AudioClip hitSound;        // 敲击音效

    int currentIronIndex = 0; //当前铁块索引
    float timer = 0.0f; //計時器
    bool isEnd = false;

    LevelObjManager levelObjManager;
    HintManager hintManager;            //管理提示板
    MoleculaDisplay moleculaManager;    //管理分子螢幕
    AudioManager audioManager;          //音樂管理

    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        audioManager = FindObjectOfType<AudioManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_1_1");

        moleculaManager.ShowMoleculas(0);

        // 隐藏所有的铁块对象
        foreach (GameObject ironObject in ironObjects)
        {
            ironObject.SetActive(false);
        }
        ironObjects[0].SetActive(true);

        currentIronIndex = 0;
        isEnd = false;
    }

    void Update()
    {
        //打了第一下之後數10秒用
        if (currentIronIndex > 0 && Time.time > timer )
        {
            endTheTutorial();
        }
    }

    void HammerHit()
    {
        if (currentIronIndex < ironObjects.Length - 1 && !isEnd )
        {
            audioManager.PlaySound(1);

            //軟鐵片彈起效果
            Rigidbody ironRig = iron.GetComponent<Rigidbody>();
            ironRig.AddForce(Random.Range(-10.0f, 10.0f), 40.0f, Random.Range(-10.0f, 10.0f));
            ironRig.AddTorque(Random.Range(-1.0f, 1.0f), Random.Range(-2.0f, 2.0f), 0.0f);
            //切換模型
            ironObjects[currentIronIndex].SetActive(false);
            currentIronIndex++;
            ironObjects[currentIronIndex].SetActive(true);
            //粒子視窗動畫
            moleculaManager.PlayMoleculasAnimation();

            //打了第一下之後數10秒用
            if (currentIronIndex == 1)
            {
                timer = Time.time + 10.0f;
            }
            //敲三下之後，強制完成(文件寫5下)
            if (currentIronIndex == 3)
            {
                endTheTutorial();
            }
        }
    }
    
    void endTheTutorial()   //完成教學
    {
        hintManager.SwitchStep("T1_1_2");
        hintManager.showNextButton(this.gameObject);
        isEnd = true;
        hammer.SendMessage("backToInitial");
    }

    void closeHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
