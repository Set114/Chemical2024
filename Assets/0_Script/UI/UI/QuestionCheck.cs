using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuestionCheck : MonoBehaviour
{
    [Header("Question UIs")]
    public GameObject[] questionUIs;  // 題目UI面板陣列
    public GameObject[] hintUIs;      // 提示UI面板陣列

    [Header("Result Objects")]
    public GameObject correctObject;  // 正確的物件
    public GameObject wrongObject;    // 錯誤的物件

    [Header("Buttons")]
    public Button[] correctButtons;   // 正確答案按鈕陣列
    public Button[] wrongButtons;     // 錯誤答案按鈕陣列
    public Button closeHintButton;    // 提示UI的關閉按鈕

    private int uid = 1;   
    private int currentQuestionIndex; // 當前題目索引
    //public Le1CheckAnswer le1CheckAnswer;
    public AudioManager audioManager;
    public CheckAnswer checkAnswer;
    public ControllerHaptics controllerHaptics;

    void Start()
    {
        // 為所有問題添加事件監聽器
        for (int i = 0; i < correctButtons.Length; i++)
        {
            int index = i; // 捕獲迴圈變數

            // 初始化顯示題目UI
            ShowQuestionUI(index);

            // 為按鈕添加點擊事件監聽器
            correctButtons[i].onClick.AddListener(() => OnCorrectButtonClicked(index));
            wrongButtons[i].onClick.AddListener(() => OnWrongButtonClicked(index));
        }

        // 為提示UI的關閉按鈕添加點擊事件監聽器
        closeHintButton.onClick.AddListener(OnCloseHintButtonClicked);
    }

    void ShowQuestionUI(int index)
    {
        // 設定當前題目索引
        currentQuestionIndex = index;

        // 隱藏所有UI，僅顯示當前題目UI
        HideAllUIs();
        questionUIs[index].SetActive(true);
    }

    void ShowHintUI(int index)
    {
        // 隱藏所有UI，僅顯示當前提示UI和關閉按鈕
        HideAllUIs();
        hintUIs[index].SetActive(true);
        closeHintButton.gameObject.SetActive(true);
        // Debug.Log("ShowHintUI: index = " + index);
    }

    void OnCorrectButtonClicked(int index)
    {
        // 設定當前題目索引
        currentQuestionIndex = index;

        if (uid == 1)
        {
            checkAnswer.CorrectAnswerData(currentQuestionIndex);
        }

        // 播放成功音效，並在音效播放後等待3秒
        StartCoroutine(PlaySuccessAndContinue());
        
        // Debug.Log("Correct");
    }

    IEnumerator PlaySuccessAndContinue()
    {
        // 播放音效
        audioManager.Play("Success");

        // 等待3秒
        yield return new WaitForSeconds(2f);

        // 顯示正確的物件，隱藏錯誤的物件
        correctObject.SetActive(true);
        wrongObject.SetActive(false);

        // 顯示提示UI
        ShowHintUI(currentQuestionIndex);
    }
    void OnWrongButtonClicked(int index)
    {
        controllerHaptics.TriggerHapticFeedback(true);
        // 設定當前題目索引
        currentQuestionIndex = index;

        if (uid == 1)
        {
            checkAnswer.WrongAnswerData(currentQuestionIndex);
        }

        // 顯示錯誤的物件，隱藏正確的物件
        wrongObject.SetActive(true);
        correctObject.SetActive(false);

        // 顯示提示UI
        ShowHintUI(index);
        // Debug.Log("Wrong");
    }

    void OnCloseHintButtonClicked()
    {
        HideAllUIs();
        closeHintButton.gameObject.SetActive(false);

        // 隱藏正確和錯誤的物件
        correctObject.SetActive(false);
        wrongObject.SetActive(false);
    }

    void HideAllUIs()
    {
        // 隱藏所有題目、提示和結果UI
        foreach (var ui in questionUIs)
        {
            ui.SetActive(false);
        }

        foreach (var ui in hintUIs)
        {
            ui.SetActive(false);
        }
    }
}
