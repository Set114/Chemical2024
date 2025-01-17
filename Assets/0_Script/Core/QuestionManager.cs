using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//  負責管理題目介面
public class QuestionManager : MonoBehaviour
{
    [Header("章節起始UI")]
    [Tooltip("章節起始介面")] [SerializeField] GameObject mainCanvas;
    [Tooltip("內容文字")] [SerializeField] DialogData dialogContent;
    [Tooltip("文字介面")] [SerializeField] TextMeshProUGUI UI_M_Title, UI_M_Question, UI_M_Warning;
    [Tooltip("警告訊息圖片")] [SerializeField] GameObject Image_Warning;
    [Header("提問UI")]
    [Tooltip("提問介面")] [SerializeField] GameObject questionCanvas;
    [Tooltip("回答介面")] [SerializeField] GameObject ExamCanvas;
    [Tooltip("答案介面")] [SerializeField] GameObject AnswerCanvas;
    [Tooltip("正確標記")] [SerializeField] GameObject MarkCorrect;
    [Tooltip("錯誤標記")] [SerializeField] GameObject MarkWrong;
    [Tooltip("內容文字")] [SerializeField] QuestionData questionContent;
    [Tooltip("文字介面")] [SerializeField] TextMeshProUGUI UI_Q_Question, UI_Q_Answer, UI_Q_Button01, UI_Q_Button02;
    [Tooltip("頭像圖片")][SerializeField] Sprite[] userIcons;
    [Tooltip("答案提出者頭像")][SerializeField] GameObject userIcon0, userIcon1;
    [Tooltip("答案提出者頭像圖片")][SerializeField] Image userIconImage0, userIconImage1;
    [Header("結束UI")]
    [Tooltip("教學結束介面")] [SerializeField] GameObject finishLearnCanvas;
    [Tooltip("測驗結束介面")] [SerializeField] GameObject finishExamCanvas;

    int currentIndex;   //紀錄目前的編號
    bool isAnswer = false;  //紀錄是否已經答題，防呆用
    GameObject sender;  //用來記錄誰呼叫他的

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
        mainCanvas.SetActive(false);
        questionCanvas.SetActive(false);
        finishLearnCanvas.SetActive(false);
        finishExamCanvas.SetActive(false);
    }
    //  顯示章節提問
    public void ShowQuestion(int index)
    {
        currentIndex = index;
        mainCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        mainCanvas.SetActive(true);
        Image_Warning.SetActive(false);

        UI_M_Title.text = dialogContent.dialogContent[currentIndex].title;
        UI_M_Question.text = dialogContent.dialogContent[currentIndex].question;
        UI_M_Warning.text = dialogContent.dialogContent[currentIndex].warning;
        audioManager.PlayVoice(dialogContent.dialogContent[currentIndex].voiceName);
        Image_Warning.SetActive(UI_M_Warning.text != "");        
    }

    public void ShowExam(int index, GameObject Sender)
    {
        currentIndex = index;
        sender = Sender;
        questionCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
        questionCanvas.SetActive(true);

        ExamCanvas.SetActive(true);
        AnswerCanvas.SetActive(false);
        MarkCorrect.SetActive(false);
        MarkWrong.SetActive(false);
        UI_Q_Question.text = questionContent.questionContent[currentIndex].question;
        UI_Q_Button01.text = questionContent.questionContent[currentIndex].answer0Text;
        UI_Q_Button02.text = questionContent.questionContent[currentIndex].answer1Text;
        UI_Q_Answer.text = questionContent.questionContent[currentIndex].answer;

        //設定答案提出者頭像
        userIcon0.SetActive(false);
        userIcon1.SetActive(false);
        if (questionContent.questionContent[currentIndex].user0Name != "")
        {
            foreach (Sprite userIcon in userIcons)
            {
                if (userIcon.name == questionContent.questionContent[currentIndex].user0Name)
                {
                    userIconImage0.sprite = userIcon;
                    userIcon0.SetActive(true);
                }
            }
        }

        if (questionContent.questionContent[currentIndex].user1Name != "")
        {
            foreach (Sprite userIcon in userIcons)
            {
                if (userIcon.name == questionContent.questionContent[currentIndex].user1Name)
                {
                    userIconImage1.sprite = userIcon;
                    userIcon1.SetActive(true);
                }
            }
        }

        audioManager.PlayVoice(questionContent.questionContent[currentIndex].voiceQuestionName);
        Image_Warning.SetActive(UI_Q_Button02.text != "");

        isAnswer = false;
    }
    //當使用者點選回答按鈕時
    public void AnswerSelect(int answerNumber)
    {
        if (!isAnswer)  //防呆
        {
            isAnswer = true;
            //答對先撥音效
            if (questionContent.questionContent[currentIndex].answerNumber == answerNumber)
            {
                audioManager.Stop();
                audioManager.PlaySound(new SoundMessage(2, null, this.gameObject));
            }
            else //答錯直接講解
            {
                ExamCanvas.SetActive(false);
                AnswerCanvas.SetActive(true);
                MarkWrong.SetActive(true);
                audioManager.PlayVoice(questionContent.questionContent[currentIndex].voiceAnswerName);
            }
        }
    }

    //答對後等待語音
    void FinishAudio()
    {
        ExamCanvas.SetActive(false);
        AnswerCanvas.SetActive(true);
        MarkCorrect.SetActive(true);
        audioManager.PlayVoice(questionContent.questionContent[currentIndex].voiceAnswerName);
    }

    //結束該問題
    public void OnNextButtonClicked()
    {
        AnswerCanvas.SetActive(false);
        sender.SendMessage("FinishExam");
        audioManager.Stop();
    }

    //結束教學後，開啟選單
    public void ShowFinishLearnUI()
    {
        finishLearnCanvas.SetActive(true);
    }
    //結束測驗後，開啟選單
    public void ShowFinishExamUI()
    {
        finishExamCanvas.SetActive(true);
    }


    //  切換關卡結束介面
    public void SwitchLevelClearUI(bool isLearn)
    {
        finishLearnCanvas.SetActive(isLearn);
        finishExamCanvas.SetActive(!isLearn);
    }

    //  按下確認按鈕
    public void OnStartBtnClicked()
    {
        mainCanvas.SetActive(false);
        audioManager.Stop();
        audioManager.PlaySound(0);
        levelManager.LevelStart();
    }
    //  按下測試按鈕
    public void OnExamBtnClicked()
    {
        finishLearnCanvas.SetActive(false);
        gm.SwitchToExamLevel();
    }
    //  按下結束按鈕
    public void OnReturnBtnClicked()
    {
        gm.BackToMainMenu();
    }
    /*
    //  取得題目數量
    public int GetQuestionsLength()
    {
        return dialogContent.Length;
    }
    */
}
