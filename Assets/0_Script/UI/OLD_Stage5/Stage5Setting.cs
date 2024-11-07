using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Stage5Setting : MonoBehaviour
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
    [SerializeField] TMP_Text studentname_txt;

    [Header("UI")]
    [SerializeField] GameObject Account;
    [SerializeField] GameObject Lesson_List;
    [SerializeField] GameObject Setting;
    [SerializeField] GameObject Learnimg;
    [SerializeField] GameObject Testing;

    [Header("Slider")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider UIeffectSlider;

    public int testIndex;

    private string studentData;
    public int chapterModeData = 0;
    public Stage5_UIManager stage5_UIManager;
    public AudioManager audioManager;
    public GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.Instance;
        // �T�O gameManager ��Ҥw��l��
        if (gameManager != null)
        {
            studentData = gameManager.GetCurrentPlayerName();

            if (!string.IsNullOrEmpty(studentData))
            {
                studentname_txt.text = studentData;
                chapterModeData = gameManager.GetChapterMode();
            }
            else
            {
                Debug.Log("GameManager ��Ҫ� studentData ���šC");
                studentname_txt.text = "���p��";
            }
        }
        else
        {
            Debug.Log("GameManager ��Ҭ��šC");
            studentname_txt.text = "���p��";
            // Optionally handle the case where gameManager is null
        }

        // �ǲ߼Ҧ�����
        if (chapterMode_btn != null)
        {
            Image modeButton_img = chapterMode_btn.GetComponent<Image>();
            if (modeButton_img != null)
            {
                UpdateChapterModeImage(modeButton_img);
            }
            else
            {
                Debug.LogError("chapterMode_btn �S�� Image �ե�C");
            }
        }
        else
        {
            Debug.LogError("chapterMode_btn �b�˵����O�������w�C");
        }

        BindButtonEvents();

    }

    private void BindButtonEvents()
    {
        lessonList_btn.onClick.AddListener(LessonList_Open);
        lessonBack_btn.onClick.AddListener(LessonList_Close);
        setting_btn.onClick.AddListener(Setting_Open);
        settingBack_btn.onClick.AddListener(Setting_Close);

        // settingUI�����s
        logout_btn.onClick.AddListener(Logout);
        levlMenu_btn.onClick.AddListener(LevlMenu);
        refresh_btn.onClick.AddListener(RefreshScene);
        chapterMode_btn.onClick.AddListener(ChapterMode);
    }

    //�P�_�i�J�Ҧ�
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

    //�����Ҧ�
    public void ChapterMode()
    {
        Image modeButton_img = chapterMode_btn.GetComponent<Image>();

        if (modeButton_img.sprite == learnMode_img)
        {
            audioManager.Stop();
            modeButton_img.sprite = testMode_img;
            chapterModeData = 1;
            Learnimg.SetActive(false);
            Testing.SetActive(true);
            stage5_UIManager.ShowTestMode(testIndex);

        }
        else if (modeButton_img.sprite == testMode_img)
        {
            audioManager.Stop();
            modeButton_img.sprite = learnMode_img;
            chapterModeData = 0;
            Learnimg.SetActive(true);
            Testing.SetActive(false);
            RefreshScene();
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

    void RefreshScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
