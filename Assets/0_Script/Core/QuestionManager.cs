using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//  負責管理題目介面
public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public struct UIDailog  //對話框架構
    {
        public string title;    //標題文字
        [TextArea(3, 5)]
        public string question; //內容文字
        [TextArea(3, 5)]
        public string warning;  //警告訊息
        public string voiceName;  //對應語音

        public UIDailog(string Title, string Question, string Warning, string VoiceName)
        {
            title = Title;
            question = Question;
            warning = Warning;
            voiceName = VoiceName;
        }
    }

    [Header("UI")]
    [Tooltip("介面主體")] [SerializeField] GameObject mainCanvas;
    [Tooltip("內容文字")] [SerializeField] UIDailog[] dialogContent;     
    [Tooltip("文字介面")] [SerializeField] TextMeshProUGUI UI_Title, UI_Question, UI_Warning;
    [Tooltip("警告訊息圖片")] [SerializeField] GameObject Image_Warning;
    [Tooltip("教學結束介面")][SerializeField] GameObject learnEnd;
    [Tooltip("測驗結束介面")][SerializeField] GameObject testEnd;
    //--System
    GameManager gm;
    LevelObjManager levelManager;
    AudioManager audioManager;  //音樂管理
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        mainCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }
    //  切換介面
    public void ShowQuestion(int index)
    {
        mainCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        mainCanvas.SetActive(true);

        learnEnd.SetActive(false);
        testEnd.SetActive(false);

        UI_Title.text = dialogContent[index].title;
        UI_Question.text = dialogContent[index].question;
        UI_Warning.text = dialogContent[index].warning;
        audioManager.PlayVoice(dialogContent[index].voiceName);
        Image_Warning.SetActive(UI_Warning.text != "");        
    }

    //  切換關卡結束介面
    public void SwitchLevelClearUI(bool isLearn)
    {
        learnEnd.SetActive(isLearn);
        testEnd.SetActive(!isLearn);
    }

    //  按下確認按鈕
    public void OnStartBtnClicked()
    {
        mainCanvas.SetActive(false);
        audioManager.PlaySound(0);  
        
        levelManager.LevelStart();
    }
    //  按下測試按鈕
    public void OnTestBtnClicked()
    {
        gm.SwitchToTestLevel();
    }
    //  按下結束按鈕
    public void OnQuitBtnClicked()
    {
        gm.BackToMainMenu();
    }
    //  取得題目數量
    public int GetQuestionsLength()
    {
        return dialogContent.Length;
    }
}
