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

    [Header("Buttons")]
    [SerializeField] Button learnUI_Close;
    [SerializeField] Button testUI_Close;

    [Header("UI Panels")]
    [SerializeField] GameObject[] levelUIs; 
    [SerializeField] GameObject items; 

    [Header("TestIndex")]
    [SerializeField] int TestIndex;
    private int levelCount = -1;
    private int chapterMode = 0; // 假設的初始值

    private LevelManager levelManager;
    private SwitchItem switchItem;
    public LearnDataManager learnDataManager;
    public TestDataManager testDataManager;
    private GameManager gameManager;

    void Start()
    {
        // 初始化 LevelManager
        levelManager = new LevelManager(levelUIs.Length);
        gameManager = GameManager.Instance;
        chapterMode = gameManager.GetChapterMode();

        // 確保 SwitchItem 已初始化
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

    // 關閉當前活動的 UI
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

            // 使用 LevelManager 來檢查通關狀態
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
                    learnDataManager.GetsId(levelCount);
                    learnDataManager.StartLevel();
                }
                else
                {
                    
                    gameManager.UpdateChapterMode(1);
                    TestUI.SetActive(true);
                    // int testindex = levelCount % TestIndex;
                    // testDataManager.GetsId(testindex);
                    // testDataManager.StartLevel();
                }
                switchItem.SetCurrentLevel(levelCount);
                //Debug.Log("ShowUIFinish");
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

    public void ShowTestLevel()
    {
        levelCount = TestIndex - 1;
        ShowNextUI();
    }

    public void ListChangeLevel(int levelindex)
    {
        levelCount = levelindex - 1;
        ShowNextUI();
    }

    public int GetLevelCount()
    {
        return levelCount;
    }

    public void CompletedState(int levelIndex)
    {
        levelManager.CompleteLevel(levelIndex);
    }
}
