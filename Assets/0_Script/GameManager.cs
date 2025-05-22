using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("預設關卡一開始是教學模式，0 = 教學 ，1 = 測驗")]
    public int gameMode = 0;
    [Tooltip("目前單元，需手動輸入")]
    public int currStage = 0;
    [Tooltip("目前關卡")]
    public int currLevel = 0;   //這個直接開放給其他物件使用，要正式一點就設定只能讀取
    [Tooltip("輸入測驗在第幾個levelObj")]
    public int examIndex;
    [Tooltip("總共有多少關")]
    public int totalIndex = 0;
    [Tooltip("失誤次數")]
    private int mistake = 0;

     [Tooltip("管理播放音效")]
    private AudioManager audioManager;
    [Tooltip("管理關卡物件")]
    private LevelObjManager levelObjManager;
    [Tooltip("管理題目介面")]
    private QuestionManager questionManager;
    [Tooltip("管理左上角控制板")]
    private SettingUIManager controlPanel;

    [Tooltip("讀取玩家資料")]
    private UserDataManager userDataManager;
    [Tooltip("紀錄學生的教學資料")]
    private LearnDataManager learnDataManager;
    [Tooltip("紀錄學生的測驗資料")]
    private TestDataManager testDataManager;
    [Tooltip("紀錄各單元的教學與測驗資料")]
    private DataInDevice DataInDeviceScript;
    public bool isSaved = false;

    [Tooltip("緩衝時間")]
    [SerializeField] private float defaultDelay = 3f;

    private void OnEnable()
    {
        controlPanel = FindObjectOfType<SettingUIManager>();
        levelObjManager = FindObjectOfType<LevelObjManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        audioManager = FindObjectOfType<AudioManager>();

        userDataManager = UserDataManager.Instance;
        learnDataManager = FindObjectOfType<LearnDataManager>();
        testDataManager = FindObjectOfType<TestDataManager>();
        DataInDeviceScript = FindObjectOfType<DataInDevice>();

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
        controlPanel.SetLessonListButton();
    }

    //  關卡開始------------可能要搬到上面
    public void LevelStart()
    {
        if (currLevel == 0 || currLevel == examIndex)
        {
            string[] userData = { controlPanel.studentDataID, controlPanel.studentData };

            int stage = ((currStage - 1) * 2) + gameMode;
            DataInDeviceScript.SaveData[stage].AddRange(userData);

            int level;
            if (gameMode == 0|| currStage==3)
            {
                if (currStage == 3)
                {
                    level = totalIndex - examIndex;
                }
                else
                    level = examIndex;
                for (int i = 0; i < level; i++)
                {
                    DataInDeviceScript.SaveData[stage].Add("無紀錄＿開始時間");
                    DataInDeviceScript.SaveData[stage].Add("無紀錄＿結束時間");
                    DataInDeviceScript.SaveData[stage].Add("無紀錄＿次數");
                }
            }
            else
            {
                if (currStage == 5)
                {
                    DataInDeviceScript.SaveData[stage].Add("無紀錄＿時間");
                    DataInDeviceScript.SaveData[stage].Add("無紀錄＿次數");
                }
                else
                {
                    level = questionManager.GetQuestionsLength();
                    for (int i = 0; i < level; i++)
                    {
                        DataInDeviceScript.SaveData[stage].Add("無紀錄＿答案");
                        DataInDeviceScript.SaveData[stage].Add("無紀錄＿得分");

                    }
                }
            }
            isSaved = false;
        }
        if (currLevel < examIndex)
        {
            // 紀錄學生關卡的學習歷程資料
            learnDataManager.GetsId(currLevel);
            userDataManager.UpdateStartTime(); // 2025.2.12 戴偉勝
            learnDataManager.StartLevel();

            controlPanel.SetStageText(currStage + "-" + (currLevel + 1).ToString("0"));
            RecordTeachTime(true);
        }
        else
        {
            controlPanel.SetStageText(currStage + "-" + (currLevel + 1 - examIndex).ToString("0"));
            if(currStage == 3)
            {
                RecordTeachTime(true);
            }
        }
        controlPanel.LessonList_Close();
        mistake = 0;
    }

    //  切換到測驗關卡
    public void SwitchToLearnLevel()
    {
        userDataManager.UpdateChapterMode(0);
        PushToExcel();
        gameMode = userDataManager.GetChapterMode();
        currLevel = 0;
        levelObjManager.SetLevel();
    }

    //  切換到測驗關卡
    public void SwitchToExamLevel()
    {
        userDataManager.UpdateChapterMode(1);
        PushToExcel();
        gameMode = userDataManager.GetChapterMode();
        currLevel = examIndex;
        levelObjManager.SetLevel();
        controlPanel.UpdatePanelButtons();
    }

    //  切換到指定關卡
    public void SwitchLevel(int level)
    {
        //在這邊處理跳關後的操作記錄
        currLevel = level;
        levelObjManager.SetLevel();
    }

    //  關卡結束 儲存資料以及切換關卡編號
    public void LevelClear()
    {
        if (gameMode == 0 || currStage == 3)
        {
            RecordTeachTime(false);
        }
        controlPanel.LevelClear(currLevel);
        currLevel++; 
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
    //累計失誤次數
    public void GetMistake()
    {
        mistake++;
    }

    //  紀錄各單元的教學資料
    public void RecordTeachTime(bool isStart)
    {
        int stage = ((currStage - 1) * 2) + gameMode;

        int level;
        if (gameMode == 0)
        {
            level = currLevel + 1;
        }
        else
        {
            level = currLevel + 1 - examIndex;
        }

        int baseIndex = 2 + (level - 1) * 3;
        int startIndex = baseIndex;      // 開始時間
        int endIndex = baseIndex + 1;    // 結束時間
        int countIndex = baseIndex + 2;  // 次數

        if (isStart)
        {
            DataInDeviceScript.SaveData[stage][startIndex] = DateTime.Now.ToString();
        }
        else
        {
            DataInDeviceScript.SaveData[stage][endIndex] = DateTime.Now.ToString();
            DataInDeviceScript.SaveData[stage][countIndex] = mistake.ToString("0");
        }
        isSaved = false;
    }
    //  寫出資料
    public void PushToExcel()
    {
        int stage = ((currStage - 1) * 2) + gameMode;
        DataInDeviceScript.AddDataExcel(stage);
        isSaved = true;
    }

    public void BackToMainMenu()
    {
        if (!isSaved)
            PushToExcel();
        MenuUIManager.shouldOpenMenu = true;
        SceneManager.LoadScene("MainMenu");
    }
}
