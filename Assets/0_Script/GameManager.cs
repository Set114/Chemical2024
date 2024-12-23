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
    [Tooltip("預設關卡一開始是教學模式，0 = 教學 ，1 = 測驗")]
    private int gameMode = 0;
    [Tooltip("目前單元，需手動輸入")]
    [SerializeField] private int currStage = 0;
    [Tooltip("目前關卡")]
    public int currLevel = 0;   //這個直接開放給其他物件使用，要正式一點就設定只能讀取
    [Tooltip("輸入測驗的第一個UI在第幾個levelUIs")]
    [SerializeField] private int examIndex;

    [Tooltip("管理關卡物件")]
    [SerializeField] private LevelObjManager levelObjManager;
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
        //整合gameMode 與 currentLevel 的運用
        //  根據模式切換介面
        if (gameMode == 0)
        {
            levelObjManager.SetLevel();
        }
        else if (gameMode == 1)
        {
            //questionManager.SwitchUI(testIndex);
        }        
    }

    //  關卡開始------------可能要搬到上面
    public void LevelStart()
    {
        if (currLevel < examIndex)
        {
            // 紀錄學生關卡的學習歷程資料
            learnDataManager.GetsId(currLevel);
            learnDataManager.StartLevel();
        }
    }

    //  切換到測驗關卡
    public void SwitchToExamLevel()
    {
        userDataManager.UpdateChapterMode(1);
        gameMode = userDataManager.GetChapterMode();
        currLevel = examIndex;
    }

    //  關卡結束 儲存資料以及切換關卡編號
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
    }
    //-------感覺用不到
    public float NextStep()
    {
        return 1;
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
        audioManager.PlayVoice(name);
    }

    //  取得目前關卡-------都開放了，這個其實就多餘了
    public int GetCurrLevel()
    {
        return currLevel;
    }
    public float GetDefaultDelay()  //是在delay什麼...=..=||||
    {
        return defaultDelay;
    }
    public void BackToMainMenu()
    {
        MenuUIManager.shouldOpenMenu = true;
        SceneManager.LoadScene("MainMenu");
    }
}
