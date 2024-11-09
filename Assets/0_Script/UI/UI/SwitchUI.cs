using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchUI : MonoBehaviour
{
    [Header("ModeUI")]
    [SerializeField] GameObject LearnUI;
    [SerializeField] GameObject TestUI;

    [Header("下一關的案紐")]//但只負責關掉UI
    [SerializeField] Button learnUI_Close;
    [SerializeField] Button testUI_Close;

    [Header("UI Panels")]
    [SerializeField] GameObject[] levelUIs; 

    [Header("輸入測驗的第一個UI在第幾個Element")]
    [SerializeField] int TestIndex;
    
    [SerializeField] GameObject items; 

    private int levelCount = -1;
    private int chapterMode = 0; 
    // 假設關卡一開始是教學模式，0 = 教學 ，1 = 測驗

    private LevelManager levelManager; // 查看與紀錄關卡是否已做過
    private SwitchItem switchItem; // 切換關卡物品
    public LearnDataManager learnDataManager; // 紀錄學生的 教學 資料
    private GameManager gameManager; // 讀取玩家資料

    void Start()
    {
        levelManager = new LevelManager(levelUIs.Length);

        gameManager = GameManager.Instance;
        chapterMode = gameManager.GetChapterMode();
        
        switchItem = GetComponent<SwitchItem>();

        if (switchItem == null)
        {
            Debug.LogError("SwitchItem component not found!");
            return;
        }
        
        // 確保關閉按鈕已初始化
        if (learnUI_Close != null && testUI_Close != null) 
        {
            learnUI_Close.onClick.AddListener(CloseCurrentUI);
            testUI_Close.onClick.AddListener(CloseCurrentUI);
        }
        else
        {
            Debug.LogError("UI_Close button is not assigned!"); 
        }

        // 顯示適當的 UI 模式
        if (chapterMode == 0)
        {
            ListChangeLevel(0);
        }
        else if (chapterMode == 1)
        {
            ListChangeLevel(TestIndex);
        }
        
    }

    // 關閉當前活動的 UI ( UI 全關 )
    public void CloseCurrentUI()
    {
        foreach (GameObject ui in levelUIs)
        {
            if (ui != null && ui.activeSelf) 
            {
                ui.SetActive(false);
                LearnUI.SetActive(false);
                TestUI.SetActive(false);
                //items.SetActive(true);
                break;
            }
        }
    }

    // 顯示下一個 UI
    public void ShowNextUI()
    {
        if (levelCount < levelUIs.Length)
        {
            CloseCurrentUI();
            items.SetActive(false);

            // 使用 LevelManager 來檢查通關狀態(此關卡是否已做過)
            do
            {
                levelCount++;
            } while (levelCount < levelUIs.Length && levelManager.IsLevelCompleted(levelCount));

            if (levelCount < levelUIs.Length && levelUIs[levelCount] != null)
            {
                levelUIs[levelCount].SetActive(true);
                if (levelCount < TestIndex)
                {
                    LearnUI.SetActive(true);
                    // 紀錄學生關卡的學習歷程資料
                    learnDataManager.GetsId(levelCount);
                    learnDataManager.StartLevel();
                }
                else
                {
                    TestUI.SetActive(true);
                    gameManager.UpdateChapterMode(1);
                }
                switchItem.UpdateLevelName(levelCount);
                //換關
                switchItem.SetCurrentLevel(levelCount);
                //Debug.Log("ShowNextUI is Finish");
            }
            else
            {
                Debug.LogError("Level UI is null or index out of range.");
            }
        }
        else
        {
            Debug.Log("No more UI panels to show."); 
        }
    }

    // 顯示測驗關卡
    public void ShowTestLevel()
    {
        levelCount = TestIndex - 1;
        ShowNextUI();
    }

    // 顯示跳關
    public void ListChangeLevel(int levelindex)
    {
        levelCount = levelindex - 1;
        ShowNextUI();
    }

    // 取得目前在第幾關的值
    public int GetLevelCount()
    {
        return levelCount;
    }

    // 標記某個關卡為通關
    public void CompletedState(int levelIndex)
    {
        levelManager.CompleteLevel(levelIndex);
    }
}
