using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  負責管理關卡物件
public class LevelObjManager : MonoBehaviour
{
    [Tooltip("關卡物品")]
    [SerializeField] GameObject[] levelObjects; // 關卡物品
    private GameObject currentScene;

    [Tooltip("LoadingSign")]
    public GameObject loading_sign;

    //---system
    private GameManager gm;
    private QuestionManager questionManager;    //管理題目介面
    private HintManager hintManager;            //管理提示板
    private MoleculaDisplay moleculaManager;    //管理分子螢幕
    private ZoomDisplay zoomDisplay;            //管理近看視窗

    void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        hintManager = FindObjectOfType<HintManager>();
        moleculaManager = FindObjectOfType<MoleculaDisplay>();
        zoomDisplay = FindObjectOfType<ZoomDisplay>();
        loading_sign.SetActive(false);
    }

    public void SetLevel()    //給GameManager 使用的
    {
        if (currentScene)
            Destroy(currentScene);
        hintManager.OnCloseBtnClicked();
        moleculaManager.CloseDisplay();
        zoomDisplay.CloseDisplay();
        //應該要依照currentLevel 去切換對應的
        questionManager.ShowQuestion(gm.currLevel);
    }

    public void LevelStart()
    {
        if (gm.currLevel < levelObjects.Length)
        {
            if (levelObjects[gm.currLevel] != null)
            {
                currentScene = Instantiate(levelObjects[gm.currLevel], this.transform);
                currentScene.SetActive(true);
                gm.LevelStart();
            }
        }
    }

    //關卡完成 NextStageType 0:下一關 1:教學結束 2:測驗結束
    public void LevelClear(int NextStageType)
    {
        //  播放最後的提示語音
        //levelObjects[gm.currLevel].SetActive(false);
        if (currentScene)
            Destroy(currentScene);

        //刪除被抓取中或沒回到currentScene底下的關卡物件
        GameObject[] pickables = GameObject.FindGameObjectsWithTag("Pickable");
        foreach(GameObject pickable in pickables)
        {
            Destroy(pickable);
        }

        moleculaManager.CloseDisplay();
        zoomDisplay.CloseDisplay();

        switch (NextStageType)
        {
            case 0: //下一關
                loading_sign.SetActive(true);
                StartCoroutine(ShowFinishDialog());
                break;
            case 1: //教學結束
                loading_sign.SetActive(false);
                questionManager.ShowFinishLearnUI();
                break;
            case 2: //測驗結束
                loading_sign.SetActive(false);
                questionManager.ShowFinishExamUI();
                break;
        }
        gm.LevelClear();
    }

    //待移除
    public void LevelClear(string answer, string hintSoundName)
    {
        //  播放最後的提示語音
        loading_sign.SetActive(true);
        //StartCoroutine(ShowHintDelay(answer));
    }

    IEnumerator ShowFinishDialog()
    {
        yield return new WaitForSeconds(1.0f);
        loading_sign.SetActive(false);
        //??????  gm.LevelClear(answer); 
        this.SetLevel();
    }
}
