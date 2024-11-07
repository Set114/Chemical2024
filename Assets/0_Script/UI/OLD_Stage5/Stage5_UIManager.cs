using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Stage5_UIManager : MonoBehaviour
{
    [Header("UIMode")]
    [SerializeField] GameObject LearnUI;
    [SerializeField] GameObject TestUI;
    [Header("TeachEnd")]
    [SerializeField] GameObject EndUI;

    [Header("Buttons")]
    [SerializeField] Button UI_Close;
    [SerializeField] Button playButton; 

    [Header("UI Panels")]
    [SerializeField] GameObject[] levelUIs; 
    [SerializeField] GameObject items; 
    [SerializeField] GameObject BG; 

    [Header("TestIndex")]
    [SerializeField] int TestIndex;
    private int levelCount = -1;
    private int chapterMode;


    public CameraController cameraController;
    public PlaySpeechAudio playSpeechAudio;
    public SwitchLevelObjects_5 switchLevelObjects_5;


    public GameManager gameManager;
    //public SwitchLevelObjects switchLevelObjects;

    void Start()
    {
        gameManager = GameManager.Instance;
        chapterMode = gameManager.GetChapterMode();

        if (UI_Close != null)
        {
            UI_Close.onClick.AddListener(CloseCurrentUI);
        }
        else
        {
            Debug.LogError("UI_Close button is not assigned!"); 
        }


        if (chapterMode == 0)
        {
            ShowNextUI();
            Debug.Log("***Modecheck***" + levelCount);
        }
        else if (chapterMode == 1)
        {
            ShowTestMode(TestIndex);
        }

    }

    //��������UI
    public void CloseCurrentUI()
    {
        foreach (GameObject ui in levelUIs)
        {
            if (ui.activeSelf) 
            {
                ui.SetActive(false);
                LearnUI.SetActive(false);
                TestUI.SetActive(false);
                items.SetActive(true);
                break;
            }
        }
    }

    //LearnNextUI
    public void ShowNextUI()
    {
        if (levelCount < levelUIs.Length)
        {
            items.SetActive(false);
            Debug.Log("***LevCount1***" + levelCount);
            levelCount++;
            Debug.Log("***LevCount2***" + levelCount);
            LearnUI.SetActive(true);
            playButton.gameObject.SetActive(true);
            BG.SetActive(true);
            levelUIs[levelCount].SetActive(true);
            Debug.Log("***LevCount3***" + levelCount);
            Debug.Log("finish");

        }
        else
        {
            Debug.Log("No more UI panels to show."); 
        }
    }

    //LearnEndUI
    public void ShowEndUI()
    {
        LearnUI.SetActive(true);
        EndUI.SetActive(true);
    }

    //���ռҦ�(�������d�Ĥ@��)
    public void ShowTestMode(int index)
    {
        if (levelCount < levelUIs.Length)
        {
            items.SetActive(false);
            levelCount = index;
            Debug.Log("Current Level: " + levelCount); 
            LearnUI.SetActive(false);
            TestUI.SetActive(true);
            levelUIs[levelCount].SetActive(true); 
        }
        else
        {
            Debug.Log("No more UI panels to show.");
        }
    }
    
    //���ռҦ�(�U�@��)
    public void ShowTestNextUI()
    {
        if (levelCount < levelUIs.Length)
        {
            items.SetActive(false);
            levelCount++;
            Debug.Log("Current Level: " + levelCount); 
            LearnUI.SetActive(false);
            TestUI.SetActive(true);
            levelUIs[levelCount].SetActive(true); 
        }
        else
        {
            Debug.Log("No more UI panels to show.");
        }
    }

    public void SetCurrentLevel(int level)
    {
        levelCount = level;
    }


    public void ListChangeLevel(int levelindex)
    {
        CloseCurrentUI();
        levelCount = levelindex;
        if (levelindex < TestIndex)
        {
            //TEACH MODE
            LearnUI.SetActive(true);
            TestUI.SetActive(false);
            BG.SetActive(true);
            playButton.gameObject.SetActive(true);

            items.SetActive(false);

            levelUIs[levelCount].SetActive(true); 
            switchLevelObjects_5.ListSwitch(levelindex-1);

            // lvl1tutorialGM.GetsId(levelCount);
            // lvl1tutorialGM.StartLevel();
            // LearnUI.SetActive(true); 
            //以上是學歷登記

        }
        else
        {
            //TEST MODE
            LearnUI.SetActive(false);
            TestUI.SetActive(true);

            items.SetActive(false);

            levelUIs[levelCount].SetActive(true); 
            switchLevelObjects_5.ListSwitch(levelindex-1);
            // switchLevelObjects.DisableAllLevelObjects();
            // if (levelindex ==  6)
            // {
            //     ShowTestMode();
            // }else if (levelindex ==  7)
            // {
            //     ShowTestMode2();
            // }
        }
    }
}