using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingUIManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button setting_btn;
    [SerializeField] Button settingBack_btn;
    [SerializeField] Button lessonList_btn;
    [SerializeField] Button lessonBack_btn;
    [SerializeField] Button logout_btn;
    [SerializeField] Button levlMenu_btn;
    [SerializeField] Button refresh_btn;



    [SerializeField] Button bgmMax_btn;
    [SerializeField] Button bgmMin_btn;    
    [SerializeField] Button UIeffectMax_btn;
    [SerializeField] Button UIeffectMin_btn;

    [Header("Image")]
    [SerializeField] Image modeButton_img;
    [SerializeField] Sprite learnMode_img;
    [SerializeField] Sprite testMode_img;
    
    [Header("Text")]
    [SerializeField] Text stage_txt;
    [SerializeField] Text studentname_txt;
    [SerializeField] Text studentid_txt;

    [Header("UI")]
    [SerializeField] GameObject Account;
    [SerializeField] GameObject Lesson_List;
    [SerializeField] GameObject Setting;
    [SerializeField] GameObject LearnBtns;
    [SerializeField] GameObject TestBtns;
    [SerializeField] private List<LessonListButton> lessonListButtons;
    [SerializeField] private GameObject lessonListBtnPrefab;

    [Header("Slider")]
    public Slider bgmSlider;
    public Slider UIeffectSlider;

    private string studentData;
    private string studentDataID;
    public int chapterModeData = 0;

    private GameManager gm;
    private UserDataManager userDataManager;
    private QuestionManager questionManager;    //取得題目
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        userDataManager = UserDataManager.Instance;
        questionManager = FindObjectOfType<QuestionManager>();
        // 確保 gameManager 實例已初始化
        if (userDataManager != null)
        {
            studentData = userDataManager.GetCurrentPlayerName();
            studentDataID = userDataManager.GetPlayerID();

            if (!string.IsNullOrEmpty(studentData))
            {
                studentname_txt.text = studentData;
                studentid_txt.text = studentDataID;
                chapterModeData = userDataManager.GetChapterMode();
            }
            else
            {
                //Debug.Log("GameManager 實例的 studentData 為空。");
                studentname_txt.text = "黃小美";
            }
        }
        else
        {
            //Debug.Log("GameManager 實例為空。");
            studentname_txt.text = "黃小美";
        }

        // 學習模式切換
        if (modeButton_img != null)
        {
            UpdateChapterModeImage(modeButton_img);
        }

        BindButtonEvents();

        lessonListButtons = new List<LessonListButton>();



        foreach (DialogMapping data in questionManager.dialogContent.dialogContent)
        {
            LessonListButton btn = Instantiate(lessonListBtnPrefab, LearnBtns.transform).GetComponent<LessonListButton>();
            lessonListButtons.Add(btn);
            btn.SetLessonName(data.title);
            int index = lessonListButtons.Count - 1;
            btn.GetComponent<Button>().onClick.AddListener(() => OnLessonButtonClicked(index));

            if (index >= gm.examIndex)
            {
                btn.transform.SetParent(TestBtns.transform);
            }
        }
    }

    private void BindButtonEvents()
    {
        lessonList_btn.onClick.AddListener(LessonList_Open);
        lessonBack_btn.onClick.AddListener(LessonList_Close);
        setting_btn.onClick.AddListener(Setting_Open);
        settingBack_btn.onClick.AddListener(Setting_Close);

        // settingUI內按鈕
        logout_btn.onClick.AddListener(Logout);
        levlMenu_btn.onClick.AddListener(LevlMenu);
        refresh_btn.onClick.AddListener(RefreshScene);
    }

    private void UpdateChapterModeImage(Image modeButton_img)
    {
        if (chapterModeData == 0)
        {
            modeButton_img.sprite = learnMode_img;
            LearnBtns.SetActive(true);
            TestBtns.SetActive(false);
        }
        else if (chapterModeData == 1)
        {
            modeButton_img.sprite = testMode_img;
            LearnBtns.SetActive(false);
            TestBtns.SetActive(true);
        }
    }

    public void ChapterMode()
    {
        int uidlevel = userDataManager.GetUid();
        if (uidlevel != 3)
        {
            if (modeButton_img.sprite == learnMode_img)
            {
                modeButton_img.sprite = testMode_img;
                chapterModeData = 1;
                userDataManager.UpdateChapterMode(chapterModeData);
                LearnBtns.SetActive(false);
                TestBtns.SetActive(true);
                gm.SwitchToExamLevel();

            }
            else if (modeButton_img.sprite == testMode_img)
            {
                modeButton_img.sprite = learnMode_img;
                chapterModeData = 0;
                userDataManager.UpdateChapterMode(chapterModeData);
                LearnBtns.SetActive(true);
                TestBtns.SetActive(false);
                RefreshScene();
            }
        }
    }

    public void OnLessonButtonClicked(int level)
    {
        LessonList_Close();
        gm.SwitchLevel(level);
    }

    public void LessonList_Open()
    {
        Account.SetActive(false);
        Lesson_List.SetActive(true);
    }

    public void LessonList_Close()
    {
        Account.SetActive(true);
        Lesson_List.SetActive(false);
    }

    void Setting_Open()
    {
        Account.SetActive(false);
        Setting.SetActive(true);
    }

    void Setting_Close()
    {
        Account.SetActive(true);
        Setting.SetActive(false);
    }

    void Logout()
    {
        MenuUIManager.shouldOpenMenu = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void LevlMenu()
    {
        MenuUIManager.shouldOpenMenu = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void RefreshScene()
    {
        int checkUid = userDataManager.GetUid();
        if (checkUid == 3)
        {
            userDataManager.UpdateChapterMode(0);
        }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void TempChapterMode()
    {
        if (modeButton_img.sprite == learnMode_img)
        {
            modeButton_img.sprite = testMode_img;
            chapterModeData = 1;
            userDataManager.UpdateChapterMode(chapterModeData);

        }
        else if (modeButton_img.sprite == testMode_img)
        {
            modeButton_img.sprite = learnMode_img;
            chapterModeData = 0;
            userDataManager.UpdateChapterMode(chapterModeData);
        }
    }
    public void SetStageText(string stage)
    {
        stage_txt.text = stage;
    }
    public void LevelClear(int level)
    {
        lessonListButtons[level].CheckLessonClear(true);
    }
}
