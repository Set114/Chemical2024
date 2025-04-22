using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Drawing;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;
using static Cinemachine.DocumentationSortingAttribute;

//  負責管理題目介面
public class QuestionManager : MonoBehaviour
{
    [Header("章節起始UI")]
    [Tooltip("章節起始介面")] [SerializeField] GameObject mainCanvas;
    [Tooltip("內容文字")] public DialogData dialogContent;
    [Tooltip("文字介面")] [SerializeField] TextMeshProUGUI UI_M_Title, UI_M_Question, UI_M_Warning;
    [Tooltip("警告訊息圖片")] [SerializeField] GameObject Image_Warning;
    [Header("提問UI")]
    [Tooltip("提問介面")] [SerializeField] GameObject questionCanvas;
    [Tooltip("回答介面")] [SerializeField] GameObject ExamCanvas;
    [Tooltip("答案介面")] [SerializeField] GameObject AnswerCanvas;
    [Tooltip("正確標記")] [SerializeField] GameObject MarkCorrect;
    [Tooltip("錯誤標記")] [SerializeField] GameObject MarkWrong;
    [Tooltip("內容文字")] [SerializeField] QuestionData questionContent;
    [Tooltip("按鈕3")][SerializeField] GameObject button3;
    [Tooltip("文字介面")] [SerializeField] TextMeshProUGUI UI_Q_Question, UI_Q_Answer, UI_Q_Button01, UI_Q_Button02, UI_Q_Button03;
    [Tooltip("答案圖片")][SerializeField] Image UI_Q_ButtonImage01, UI_Q_ButtonImage02 , UI_Q_ButtonImage03;
    [Tooltip("答案提出者頭像")][SerializeField] GameObject userIcon0, userIcon1;
    [Tooltip("答案提出者頭像圖片")][SerializeField] Image userIconImage0, userIconImage1;
    [Header("結束UI")]
    [Tooltip("教學結束介面")] [SerializeField] GameObject finishLearnCanvas;
    [Tooltip("測驗結束介面")] [SerializeField] GameObject finishExamCanvas;
    [Tooltip("分數文字")][SerializeField] Text scoreText;
    [Tooltip("答案文字")][SerializeField] Text answerText;
    [Tooltip("已提交答案")][SerializeField] List<string> submittedAnswers;
    [Tooltip("已提交答案是否正確")][SerializeField] List<bool> submittedCorrectly;

    int currentIndex;   //紀錄目前的編號
    bool isAnswer = false;  //紀錄是否已經答題，防呆用

    private GameObject sender;  //用來記錄誰呼叫他的
    //--System
    private GameManager gm;
    private LevelObjManager levelManager;
    private AudioManager audioManager;                      //音樂管理
    private ControllerHaptics controllerHaptics;            //處理手把震動
    private bool isPC;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelObjManager>();
        audioManager = FindObjectOfType<AudioManager>();
        //判斷平台
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
        isPC = true;
#else
        isPC = false;
        controllerHaptics = FindObjectOfType<ControllerHaptics>();
#endif
    }

    private void OnEnable()
    {
        mainCanvas.SetActive(false);
        questionCanvas.SetActive(false);
        finishLearnCanvas.SetActive(false);
        finishExamCanvas.SetActive(false);
        submittedAnswers = new List<string>();
        submittedCorrectly = new List<bool>();

        foreach(QuestionMapping question in questionContent.questionContent)
        {
            submittedAnswers.Add("未回答");
            submittedCorrectly.Add(false);
        }
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
        if (button3 != null)
            button3.SetActive(false);
        UI_Q_Question.text = questionContent.questionContent[currentIndex].question;
        UI_Q_Button01.text = questionContent.questionContent[currentIndex].answer0Text;
        UI_Q_Button02.text = questionContent.questionContent[currentIndex].answer1Text;
        UI_Q_Answer.text = questionContent.questionContent[currentIndex].answer;

        //設定答案圖片
        if (UI_Q_ButtonImage01)
        {
            UI_Q_ButtonImage01.gameObject.SetActive(false);
            Sprite image01 = questionContent.questionContent[currentIndex].answer0Image;
            if (image01)
            {
                UI_Q_ButtonImage01.sprite = image01;
                UI_Q_ButtonImage01.gameObject.SetActive(true);
            }
        }
        if (UI_Q_ButtonImage02)
        {
            UI_Q_ButtonImage02.gameObject.SetActive(false);
            Sprite image02 = questionContent.questionContent[currentIndex].answer1Image;
            if (image02)
            {
                UI_Q_ButtonImage02.sprite = image02;
                UI_Q_ButtonImage02.gameObject.SetActive(true);
            }
        }

        if (button3 != null)
        {
            UI_Q_ButtonImage03.gameObject.SetActive(false);
            Sprite image03 = questionContent.questionContent[currentIndex].answer2Image;
            if (image03)
            {
                UI_Q_ButtonImage03.sprite = image03;
                UI_Q_ButtonImage03.gameObject.SetActive(true);
                button3.SetActive(true);
            }
        }

        //設定答案提出者頭像
        if (userIcon0)
        {
            userIcon0.SetActive(false);
            Sprite icon0 = questionContent.questionContent[currentIndex].user0Icon;
            if (icon0)
            {
                userIconImage0.sprite = icon0;
                userIcon0.SetActive(true);
            }

        }
        if (userIcon1)
        {
            userIcon1.SetActive(false);
            Sprite icon1 = questionContent.questionContent[currentIndex].user1Icon;
            if (icon1)
            {
                userIconImage1.sprite = icon1;
                userIcon1.SetActive(true);
            }
        }

        audioManager.PlayVoice(questionContent.questionContent[currentIndex].voiceQuestionName);
        Image_Warning.SetActive(UI_Q_Button02.text != "");

        isAnswer = false;
    }

    //延遲顯示題目
    public void ShowExamWithDelay(int index,float delay, GameObject Sender)
    {
        levelManager.loading_sign.SetActive(true);
        StartCoroutine(DelayedFunction(index, delay, Sender));
    }

    IEnumerator DelayedFunction(int index, float delay, GameObject Sender)
    {
        // 等待指定的秒數
        yield return new WaitForSeconds(delay);
        ShowExam(index, Sender);
        levelManager.loading_sign.SetActive(false);
    }

    //當使用者點選回答按鈕時
    public void AnswerSelect(int answerNumber)
    {
        if (!isAnswer)  //防呆
        {
            isAnswer = true;
            float score = (1 / (float)questionContent.questionContent.Length) * 100f;
            //答對先撥音效
            if (questionContent.questionContent[currentIndex].answerNumber == answerNumber)
            {
                audioManager.Stop();
                audioManager.PlaySound(new SoundMessage(2, null, this.gameObject));
                submittedCorrectly[currentIndex] = true;
            }
            else //答錯直接講解
            {
                ExamCanvas.SetActive(false);
                AnswerCanvas.SetActive(true);
                MarkWrong.SetActive(true);
                audioManager.PlayVoice(questionContent.questionContent[currentIndex].voiceAnswerName);
                submittedCorrectly[currentIndex] = false;
                if (!isPC)
                    TriggerHapticFeedback();
            }
            switch (answerNumber)
            {
                case 0:
                    submittedAnswers[currentIndex] = questionContent.questionContent[currentIndex].answer0Text;
                    break;
                case 1:
                    submittedAnswers[currentIndex] = questionContent.questionContent[currentIndex].answer1Text;
                    break;
                case 2:
                    submittedAnswers[currentIndex] = questionContent.questionContent[currentIndex].answer2Text;
                    break;
            }
        }
    }
    //回答錯誤時VR手把的震動回饋
    public void TriggerHapticFeedback()
    {
        if (controllerHaptics)
        {
            controllerHaptics.TriggerHapticFeedback();
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
        audioManager.Stop();
        AnswerCanvas.SetActive(false);
        sender.SendMessage("FinishExam");
    }

    //結束教學後，開啟選單
    public void ShowFinishLearnUI()
    {
        finishLearnCanvas.SetActive(true);
    }
    //結束測驗後，開啟選單
    public void ShowFinishExamUI()
    {
        int correct = 0;
        string answer = "";
        int stage = ((gm.currStage - 1) * 2) + gm.gameMode;
        DataInDevice dataInDevice = FindObjectOfType<DataInDevice>();
        for (int i = 0; i < questionContent.questionContent.Length; i++)
        {

            int offset = 2; // 學號＋姓名佔 0,1
            int answerIndex = offset + i * 2;
            int scoreIndex = answerIndex + 1;

            dataInDevice.SaveData[stage][answerIndex] = submittedAnswers[i];
            if (submittedCorrectly[i] == true)
            {
                correct++;
                answer += (i + 1) + "." + submittedAnswers[i] + "\n";
                dataInDevice.SaveData[stage][scoreIndex] =
                   ((1 / (float)questionContent.questionContent.Length) * 100f).ToString("0");
            }
            else
            {
                answer += "<color=red>" + (i + 1) + "." + submittedAnswers[i] + "</color>\n";
                dataInDevice.SaveData[stage][scoreIndex] = "0";
            }
        }

        float score = ((float)correct / (float)questionContent.questionContent.Length) * 100f;
        if (scoreText)
            scoreText.text = score.ToString("0")+"分";
        if (answerText)
            answerText.text = answer;
        finishExamCanvas.SetActive(true);
        dataInDevice.AddDataExcel(stage);
        gm.isSaved = true;
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
    
    //  取得題目數量
    public int GetQuestionsLength()
    {
        return questionContent.questionContent.Length;
    }
}
