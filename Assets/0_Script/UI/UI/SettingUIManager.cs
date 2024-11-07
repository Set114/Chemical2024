using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingUIManager : MonoBehaviour
{
    //public static AudioManager instance { get; private set; }

    [Header("Button")]
    [SerializeField] Button setting_btn;
    [SerializeField] Button settingBack_btn;
    [SerializeField] Button lessonList_btn;
    [SerializeField] Button lessonBack_btn;
    [SerializeField] Button logout_btn;
    [SerializeField] Button levlMenu_btn;
    [SerializeField] Button refresh_btn;
    [SerializeField] Button chapterMode_btn;

    [SerializeField] Button bgmMax_btn;
    [SerializeField] Button bgmMin_btn;    
    [SerializeField] Button UIeffectMax_btn;
    [SerializeField] Button UIeffectMin_btn;

    [Header("Image")]
    [SerializeField] Sprite learnMode_img;
    [SerializeField] Sprite testMode_img;
    
    [Header("Text")]
    [SerializeField] Text studentname_txt;
    [SerializeField] Text studentid_txt;

    [Header("UI")]
    [SerializeField] GameObject Account;
    [SerializeField] GameObject Lesson_List;
    [SerializeField] GameObject Setting;
    [SerializeField] GameObject Learnimg;
    [SerializeField] GameObject Testing;

    [Header("Slider")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider UIeffectSlider;

    private string studentData;
    private string studentDataID;
    public int chapterModeData = 0;
    public SwitchUI switchUI;
    public AudioManager audioManager;
    public GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        // 確保 gameManager 實例已初始化
        if (gameManager != null)
        {
            studentData = gameManager.GetCurrentPlayerName();
            studentDataID = gameManager.GetPlayerID();

            if (!string.IsNullOrEmpty(studentData))
            {
                studentname_txt.text = studentData;
                studentid_txt.text = studentDataID;
                chapterModeData = gameManager.GetChapterMode();
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
        if (chapterMode_btn != null)
        {
            Image modeButton_img = chapterMode_btn.GetComponent<Image>();
            if (modeButton_img != null)
            {
                UpdateChapterModeImage(modeButton_img);
            }
            else
            {
                Debug.LogError("chapterMode_btn 沒有 Image 組件。");
            }
        }
        else
        {
            Debug.LogError("chapterMode_btn 在檢視面板中未指定。");
        }

        BindButtonEvents();

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
        chapterMode_btn.onClick.AddListener(ChapterMode);
    }

    private void UpdateChapterModeImage(Image modeButton_img)
    {
        if (chapterModeData == 0)
        {
            modeButton_img.sprite = learnMode_img;
            Learnimg.SetActive(true);
            Testing.SetActive(false);
        }
        else if (chapterModeData == 1)
        {
            modeButton_img.sprite = testMode_img;
            Learnimg.SetActive(false);
            Testing.SetActive(true);
        }
    }

    public void ChapterMode()
    {
        int uidlevel = gameManager.GetUid();
        if (uidlevel != 3)
        {
            Image modeButton_img = chapterMode_btn.GetComponent<Image>();

            if (modeButton_img.sprite == learnMode_img)
            {
                audioManager.Stop();
                modeButton_img.sprite = testMode_img;
                chapterModeData = 1;
                gameManager.UpdateChapterMode(chapterModeData);
                Learnimg.SetActive(false);
                Testing.SetActive(true);
                switchUI.ShowTestLevel();

            }
            else if (modeButton_img.sprite == testMode_img)
            {
                audioManager.Stop();
                modeButton_img.sprite = learnMode_img;
                chapterModeData = 0;
                gameManager.UpdateChapterMode(chapterModeData);
                Learnimg.SetActive(true);
                Testing.SetActive(false);
                RefreshScene();
            }
        }
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
        int checkUid = gameManager.GetUid();
        if (checkUid == 3)
        {
            gameManager.UpdateChapterMode(0);
        }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void TempChapterMode()
    {
        Image modeButton_img = chapterMode_btn.GetComponent<Image>();

        if (modeButton_img.sprite == learnMode_img)
        {
            audioManager.Stop();
            modeButton_img.sprite = testMode_img;
            chapterModeData = 1;
            gameManager.UpdateChapterMode(chapterModeData);

        }
        else if (modeButton_img.sprite == testMode_img)
        {
            audioManager.Stop();
            modeButton_img.sprite = learnMode_img;
            chapterModeData = 0;
            gameManager.UpdateChapterMode(chapterModeData);
        }
    }
}
