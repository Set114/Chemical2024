using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIManager : MonoBehaviour
{
    public static int SharedChapterModeData { get; set; }
    // 宣告 UI 元素
    [Header("Button")]
    [SerializeField] Button login_btn;
    [SerializeField] Button back_btn;
    [SerializeField] Button setting_btn;
    [SerializeField] Button settingBack_btn;
    [SerializeField] Button menuLogout_btn;
    [SerializeField] Button learnMode_btn;
    [SerializeField] Button testMode_btn;
    [SerializeField] Button modeback_btn;

    [SerializeField] Button bgmMax_btn;
    [SerializeField] Button bgmMin_btn;    
    [SerializeField] Button UIeffectMax_btn;
    [SerializeField] Button UIeffectMin_btn;

    [Header("Chapter Buttons")]
    [SerializeField] Button[] chapterButtons;
    [Header("UI")]
    [SerializeField] GameObject startMenuUI;
    [SerializeField] GameObject levelPanelUI;
    [SerializeField] GameObject SelectionModeUI;
    [SerializeField] GameObject SettingUI;
    [Header("Slider")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider UIeffectSlider;
    [Header("LoadingUI")]
    [SerializeField] GameObject LoadingUI;
    [Header("TempU3TestButton")]
    [SerializeField] GameObject TestButton;

    public AudioManager audioManager;
    public UserDataManager userDataManager;

    public static bool shouldOpenMenu = false;
    private int chapterData;
    public int chapterModeData;

    private string cityData;
    private string regionData;
    private string schoolData;
    private string yearData;   
    private string classData;  
    private string studentData; 

    void Start()
    {
        ReadDatas readDatasInstance = ReadDatas.Instance;
        userDataManager = UserDataManager.Instance; // 確保gameManager被正確分配
        audioManager = AudioManager.instance; // 確保audioManager被正確分配

        InitializeUI();
    }

    void InitializeUI()
    {
        // 設定 UI 元素的初始化
        if (ReadDatas.Instance != null)
        {
            cityData = ReadDatas.Instance.cityData;
            regionData = ReadDatas.Instance.regionData;
            schoolData = ReadDatas.Instance.schoolData;
            yearData = ReadDatas.Instance.yearData;
            classData = ReadDatas.Instance.classData;
            studentData = ReadDatas.Instance.studentData;
        }

        // 返回章節控制是否為登入狀態
        if (shouldOpenMenu)
        {
            Login();
        }else{
            startMenuUI.SetActive(true);
        }

        // 事件綁定
        back_btn.onClick.AddListener(Back);
        setting_btn.onClick.AddListener(Setting_open);
        settingBack_btn.onClick.AddListener(Setting_close);
        menuLogout_btn.onClick.AddListener(MenuLogout);
        learnMode_btn.onClick.AddListener(LearnMode);
        testMode_btn.onClick.AddListener(TestMode);
        modeback_btn.onClick.AddListener(ModeBack);

        if (chapterButtons != null && chapterButtons.Length == 6)
        {
            for (int i = 0; i < chapterButtons.Length; i++)
            {
                int chapterIndex = i + 1;
                chapterButtons[i].onClick.AddListener(() => EnterChapter(chapterIndex));
            }
        }
        else
        {
            Debug.Log("Chapter buttons array is not properly initialized");
        }

        chapterModeData = 0;
        SharedChapterModeData = chapterModeData;

        // Debug.Log("Start succeed");
    }

    //登入
    public void Login()
    {
        startMenuUI.SetActive(false);
        levelPanelUI.SetActive(true);
        LoadingUI.SetActive(false);
        PlayUIeffect();
    }

    void EnterChapter(int chapterIndex)
    {
        // 第三章 教學題目與測驗題目應該是分開的步驟
        /*
        if(chapterIndex == 3)
        {
            TestButton.SetActive(false);
        }else{
            TestButton.SetActive(true);
        }
        */
        if (userDataManager == null)
        {
            Debug.Log("GameManager is null. Please ensure it is properly initialized.");
            return;
        }

        userDataManager.UpdateUid(chapterIndex);
        //PlayUIeffect();   //2025.2.12 戴偉勝 關閉音效
        chapterData = chapterIndex;
        SelectionModeUI.SetActive(true);
        levelPanelUI.SetActive(false);
    }

    void LearnMode()
    {
        //目前AudioManager改為DontDestroyOnLoad，不適用單元四、單元六
        if (chapterData == 4 || chapterData == 6)
        {
            Destroy(audioManager.gameObject);
        }
        // 設置章節模式和場景名稱
        chapterModeData = 0;

        userDataManager.UpdateChapterMode(chapterModeData);
        SharedChapterModeData = chapterModeData;
        string sceneName = $"Stage{chapterData}";
        // 在進入場景之前執行清理工作
        CleanupBeforeSceneLoad();
        // 加載場景
        SceneManager.LoadScene(sceneName);
    }

    void TestMode()
    {
        //目前AudioManager改為DontDestroyOnLoad，不適用單元四、單元六
        if (chapterData == 4 || chapterData == 6)
        {
            Destroy(audioManager.gameObject);
        }
        // 設置章節模式和場景名稱
        chapterModeData = 1;

        userDataManager.UpdateChapterMode(chapterModeData);
        SharedChapterModeData = chapterModeData;
        string sceneName = $"Stage{chapterData}";
        // 在進入場景之前執行清理工作
        CleanupBeforeSceneLoad();
        // 加載場景
        SceneManager.LoadScene(sceneName);
    }

    void CleanupBeforeSceneLoad()
    {
        Destroy(gameObject); // 銷毀MenuUIManager物件
    }
    void ModeBack()
    {
        SelectionModeUI.SetActive(false);
        levelPanelUI.SetActive(true);
        PlayUIeffect();
    }

    //返回登入介面
    void Back()
    {
        startMenuUI.SetActive(true);
        levelPanelUI.SetActive(false);
        PlayUIeffect();
    }   

    void Setting_open()
    {
        SettingUI.SetActive(true);
        PlayUIeffect();
    }   

    void Setting_close()
    {
        SettingUI.SetActive(false);
        PlayUIeffect();
    }  

    void MenuLogout()
    {
        startMenuUI.SetActive(true);
        levelPanelUI.SetActive(false);
        PlayUIeffect();
    }  

    public void PlayUIeffect()
    {
        audioManager.PlayVoice("UIeffect");
    }  
}
