using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_5_1 : MonoBehaviour
{
    [Header("長條圖")]
    [Tooltip("長條")]
    [SerializeField] private Transform bar1, bar2;
    private float scaleX;
    [Tooltip("比例")]
    [SerializeField] private float ratio1, ratio2;

    private float timer = 0;
    private int Status = 0;

    private LevelObjManager levelObjManager;
    private AudioManager audioManager;          //音樂管理
    private HintManager hintManager;            //管理提示板


    // Start is called before the first frame update
    void Start()
    {

        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        hintManager = FindObjectOfType<HintManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T5_1_1");
        scaleX = bar1.localScale.x;
    }

    private void Update()
    {
        bar1.localScale = new Vector3(scaleX, ratio1, scaleX);
        bar2.localScale = new Vector3(scaleX, ratio2, scaleX);
    }

    private void EndTheTutorial()   //完成教學
    {
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
