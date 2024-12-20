using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    [Tooltip("假設關卡一開始是教學模式，0 = 教學 ，1 = 測驗")]
    private int gameMode = 0;
    [Tooltip("目前單元，需手動輸入")]
    [SerializeField] private int currStage = 0;
    [Tooltip("目前關卡")]
    [SerializeField] private int currLevel = 0;
    [Tooltip("輸入測驗的第一個UI在第幾個levelUIs")]
    [SerializeField] private int testIndex;

    [Tooltip("管理題目介面")]
    [SerializeField] private QuestionManager questionManager;
    [Tooltip("管理關卡物件")]
    [SerializeField] private LevelObjManager levelObjManager;
    [Tooltip("管理提示板、角色")]
    [SerializeField] private HintManager hintManager;
    [Tooltip("管理分子視窗顯示")]
    [SerializeField] private MoleculaDisplay moleculaDisplay;
    [Tooltip("管理播放音效")]
    [SerializeField] private AudioManager audioManager;

    [Tooltip("讀取玩家資料")]
    private UserDataManager userDataManager;
    [Tooltip("紀錄學生的教學資料")]
    [SerializeField] private LearnDataManager learnDataManager;
    [Tooltip("紀錄學生的測驗資料")]
    [SerializeField] private TestDataManager testDataManager;

    [Tooltip("緩衝時間")]
    [SerializeField] private float defaultDelay = 3f;

    // Start is called before the first frame update
    void Start()
    {
        userDataManager = UserDataManager.Instance;
        gameMode = userDataManager.GetChapterMode();

        questionManager.gameObject.SetActive(false);
        levelObjManager.gameObject.SetActive(false);
        hintManager.gameObject.SetActive(false);

        //  根據模式切換介面
        if (gameMode == 0)
        {
            questionManager.SwitchUI(0);
        }
        else if (gameMode == 1)
        {
            questionManager.SwitchUI(testIndex);
        }
        questionManager.gameObject.SetActive(true);
    }

    //  關卡開始
    public void LevelStart()
    {
        if (currLevel < testIndex)
        {
            // 紀錄學生關卡的學習歷程資料
            learnDataManager.GetsId(currLevel);
            learnDataManager.StartLevel();
        }

        questionManager.gameObject.SetActive(false);
        levelObjManager.gameObject.SetActive(true);
        levelObjManager.SwitchObject(currLevel);

        hintManager.gameObject.SetActive(true);
        hintManager.SwitchStep(0);
        moleculaDisplay.SwitchDisplay(hintManager.GetCurrStep());
    }

    //  切換到測驗關卡
    public void SwitchToTestLevel()
    {
        userDataManager.UpdateChapterMode(1);
        gameMode = userDataManager.GetChapterMode();
        SwitchLevel(testIndex);
    }

    public void SwitchLevel(int level)
    {
        currLevel = level;
        questionManager.gameObject.SetActive(true);
        //  如果教學模式結束
        if (gameMode == 0 && currLevel >= testIndex)
        {
            questionManager.SwitchLevelClearUI(true);
            return;
        }

        if (currLevel < questionManager.GetQuestionsLength())
        {
            //切換下一關
            questionManager.SwitchUI(currLevel);

            levelObjManager.gameObject.SetActive(false);
            hintManager.gameObject.SetActive(false);
            //switchItem.UpdateLevelName(levelCount);
        }
        else
        {
            questionManager.SwitchLevelClearUI(false);
        }

    }
    //  關卡結束
    public void LevelClear(string answer)
    {
        currLevel++;
        //結束
        if (gameMode == 0)
        {
            learnDataManager.EndLevel(answer);
        }
        else if (gameMode == 1)
        {
            testDataManager.EndLevel();
        }
        SwitchLevel(currLevel);
    }

    public float NextStep()
    {
        hintManager.gameObject.SetActive(true);
        //  切換步驟的同時呼叫分子視窗顯示
        float delay = hintManager.SwitchStep(hintManager.GetCurrStep() + 1)
            + moleculaDisplay.SwitchDisplay(hintManager.GetCurrStep()) ;
        return delay;
    }

    //  取得指定音效長度
    public float GetClipLength(string name)
    {
        return audioManager.GetClipLength(name);
    }

    //  播放指定音效
    public void PlaySound(string name,float delay)
    {
        print("Play: " + name);
        StartCoroutine(CallAudioManagerPlaySound(name, delay));
    }

    IEnumerator CallAudioManagerPlaySound(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioManager.Play(name);
    }

    //  取得目前關卡
    public int GetCurrLevel()
    {
        return currLevel;
    }
    public float GetDefaultDelay()
    {
        return defaultDelay;
    }
    public void BackToMainMenu()
    {
        MenuUIManager.shouldOpenMenu = true;
        SceneManager.LoadScene("MainMenu");
    }
}
