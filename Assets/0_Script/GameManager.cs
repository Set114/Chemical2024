using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("預設關卡一開始是教學模式，0 = 教學 ，1 = 測驗")]
    private int gameMode = 0;
    [Tooltip("目前單元，需手動輸入")]
    [SerializeField] private int currStage = 0;
    [Tooltip("目前關卡")]
    public int currLevel = 0;   //這個直接開放給其他物件使用，要正式一點就設定只能讀取
    [Tooltip("輸入測驗的第一個UI在第幾個levelUIs")]
    public int examIndex;

    [Tooltip("管理播放音效")]
    private AudioManager audioManager;
    [Tooltip("管理關卡物件")]
    private LevelObjManager levelObjManager;
    [Tooltip("管理左上角控制板")]
    private SettingUIManager controlPanel;

    [Tooltip("讀取玩家資料")]
    private UserDataManager userDataManager;
    [Tooltip("紀錄學生的教學資料")]
    private LearnDataManager learnDataManager;
    [Tooltip("紀錄學生的測驗資料")]
    private TestDataManager testDataManager;

    [Tooltip("緩衝時間")]
    [SerializeField] private float defaultDelay = 3f;

    private void OnEnable()
    {
        controlPanel = FindObjectOfType<SettingUIManager>();
        levelObjManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();

        userDataManager = UserDataManager.Instance;
        learnDataManager = FindObjectOfType<LearnDataManager>();
        testDataManager = FindObjectOfType<TestDataManager>();
        gameMode = userDataManager.GetChapterMode();
        //整合gameMode 與 currentLevel 的運用
        //  根據模式切換介面
        if (gameMode == 0)
        {
            levelObjManager.SetLevel();
        }
        else if (gameMode == 1)
        {
            SwitchToExamLevel();
        }
    }

    //  關卡開始------------可能要搬到上面
    public void LevelStart()
    {
        if (currLevel < examIndex)
        {
            // 紀錄學生關卡的學習歷程資料
            learnDataManager.GetsId(currLevel);
            userDataManager.UpdateStartTime(); // 2025.2.12 戴偉勝
            learnDataManager.StartLevel();

            controlPanel.SetStageText(currStage + "-" + (currLevel + 1).ToString("0"));
        }
        else
        {
            controlPanel.SetStageText(currStage + "-" + (currLevel + 1 - examIndex).ToString("0"));
        }
        controlPanel.LessonList_Close();
    }

    //  切換到測驗關卡
    public void SwitchToExamLevel()
    {
        userDataManager.UpdateChapterMode(1);
        gameMode = userDataManager.GetChapterMode();
        currLevel = examIndex;
        levelObjManager.SetLevel();
    }

    //  切換到指定關卡
    public void SwitchLevel(int level)
    {
        currLevel = level;
        levelObjManager.SetLevel();
    }

    //  關卡結束 儲存資料以及切換關卡編號
    public void LevelClear(string answer)
    {
        controlPanel.LevelClear(currLevel);
        // answer用意待確認
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

    //  處理測驗資料儲存
    public void TestDataStart(int countindex)
    {
        testDataManager.StartLevel();
        testDataManager.GetsId(countindex);
    }
    //  處理測驗資料儲存
    public void TestDataEnd(Action callback)
    {
        testDataManager.CompleteLevel();
        testDataManager.EndLevelWithCallback(callback);
        // testDataManager.EndLevel();
    }

    public void BackToMainMenu()
    {
        MenuUIManager.shouldOpenMenu = true;
        SceneManager.LoadScene("MainMenu");
    }
}
