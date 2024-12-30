using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tutorial_1_5 : MonoBehaviour
{
    [SerializeField] private Animator cap;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject bubble;

    [Tooltip("加熱時間")]
    [SerializeField] private float heatingTime = 10f;
    [Tooltip("反應時間")]
    [SerializeField] private float reactionTime = 6f;
    private float timer = 0.0f;
    private bool heating = false;
    private bool heatingDone = false;
    private bool reactionDone = false;
    private bool firstTimeWarning = true;              // 第一次抓取危險物品的通知

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板
    private MoleculaDisplay moleculaManager;    //管理分子螢幕
    private ZoomDisplay zoomDisplay;            //管理近看視窗

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        zoomDisplay = FindObjectOfType<ZoomDisplay>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_5_1");

        moleculaManager.ShowMoleculas(3);
        zoomDisplay.ShowZoomObj(0);
        fire.SetActive(false);
        bubble.SetActive(false);
    }

    private void Update()
    {
        if (heating && !reactionDone)
        {
            timer += Time.deltaTime;

            if (timer > heatingTime + reactionTime)
            {
                reactionDone = true;
                EndTheTutorial();
            }
            else if (timer > heatingTime && !heatingDone)
            {
                heatingDone = true;
                bubble.SetActive(true);
                moleculaManager.PlayMoleculasAnimation();
                zoomDisplay.PlayAnimation();
            }
        }
    }

    public void OnAlcoholLampTouched()
    {
        if (firstTimeWarning)
        {
            hintManager.PlayWarningHint("W_AlcoholLamp");
            cap.SetBool("cover", true);
            fire.SetActive(true);
            heating = true;
            firstTimeWarning = false;
        }
    }

    public void OnTestTubeTouched()
    {
        hintManager.PlayWarningHint("W_Hot");
    }

    public void EndTheTutorial()
    {
        hintManager.SwitchStep("T1_5_2");
        hintManager.showNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
