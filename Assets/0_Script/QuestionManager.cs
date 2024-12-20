using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  負責管理題目介面
public class QuestionManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject[] questionUIs;
    [Tooltip("教學結束介面")][SerializeField] private GameObject learnEnd;
    [Tooltip("測驗結束介面")][SerializeField] private GameObject testEnd;

    private GameManager gm;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    //  切換介面
    public void SwitchUI(int index)
    {
        learnEnd.SetActive(false);
        testEnd.SetActive(false);

        foreach (GameObject ui in questionUIs)
        {
            if (ui != null)
            {
                ui.SetActive(false);
            }
        }
        if (questionUIs[index] != null)
            questionUIs[index].SetActive(true);

        if (gm == null)
            gm = FindObjectOfType<GameManager>();

        //  播放該提示音效
        gm.PlaySound("Question_" + (gm.GetCurrLevel() + 1) + "-1",gm.GetDefaultDelay());
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
        gameObject.SetActive(false);
        gm.LevelStart();
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
        return questionUIs.Length;
    }
}
