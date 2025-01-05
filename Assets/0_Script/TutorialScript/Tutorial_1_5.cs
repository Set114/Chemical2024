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
    private bool heatingDone = false;
    private bool warningAlcoholLamp = true;           // 第一次抓取危險物品的通知
    private bool warningTestTube = true;              // 第一次抓取危險物品的通知

    private LevelObjManager levelObjManager;
    private HintManager hintManager;            //管理提示板
    private MoleculaDisplay moleculaManager;    //管理分子螢幕
    private ZoomDisplay zoomDisplay;            //管理近看視窗
    private AudioManager audioManager;          //音樂管理

    int Status = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        zoomDisplay = FindObjectOfType<ZoomDisplay>();
        audioManager = FindObjectOfType<AudioManager>();

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep("T1_5_1");

        moleculaManager.ShowMoleculas(3);
        zoomDisplay.ShowZoomObj(0);
        fire.SetActive(false);
        bubble.SetActive(false);

        Status = 0;        
        warningAlcoholLamp = true;
        warningTestTube = true;
    }

    private void Update()
    {
        switch (Status)
        {
            case 0: //等待點選酒精燈

                break;
            case 1: //等待試管加熱
                timer += Time.deltaTime;
                if (timer > heatingTime )
                {
                    bubble.SetActive(true);
                    moleculaManager.PlayMoleculasAnimation();
                    zoomDisplay.PlayAnimation();
                    hintManager.SwitchStep("T1_5_2");
                    Status++;
                }                
                break;
            case 2: //當試管加熱完畢時，等待觀察時間過後
                timer += Time.deltaTime;
                if (timer > heatingTime + reactionTime)
                {
                    EndTheTutorial();
                    Status++;
                }
                break;
            case 3: //等待到下一關

                break;
        }        
    }

    public void OnAlcoholLampTouched()
    {
        if (warningAlcoholLamp)
        {
            audioManager.PlayVoice("W_AlcoholLamp");
            warningAlcoholLamp = false;
            cap.SetBool("cover", true);
            fire.SetActive(true);
            timer = 0.0f;
            Status = 1;
        }
    }

    public void OnTestTubeTouched()
    {
        if (Status > 0 && warningTestTube)
        {
            audioManager.PlayVoice("W_Hot");
            warningTestTube = false;
        }
    }

    public void EndTheTutorial()
    {
        hintManager.SwitchStep("T1_5_3");
        hintManager.ShowNextButton(this.gameObject);
    }

    void CloseHint()    //關閉提示視窗
    {
        levelObjManager.LevelClear(0);
    }
}
