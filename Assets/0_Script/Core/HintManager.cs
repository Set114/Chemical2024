using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//  管理提示板、角色
public class HintManager : MonoBehaviour
{
    [Tooltip("提示板")] [SerializeField] GameObject hintPanel;
    [Tooltip("提示訊息")] [SerializeField] TipsData tipsContent;
    [SerializeField] Text hintText;
    [SerializeField] Text hintStepText;
    [Tooltip("縮小按鈕")] [SerializeField] GameObject minimizeButton;
    [Tooltip("結束按鈕")] [SerializeField] GameObject nextButton;

    private int currStep = 0;
    private int totalStep = 0;

    AudioManager audioManager;          //音樂管理
    GameObject Sender;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();

        hintPanel.SetActive(false);
        minimizeButton.SetActive(false);
        nextButton.SetActive(false);
        if (hintStepText != null)
            hintStepText.text = "";
    }

    //  切換步驟
    public void SwitchStep(string chapterName)
    {
        hintPanel.SetActive(true);

        TextMapping t = Array.Find(tipsContent.tipsContent, tip => tip.chapter == chapterName);
        if (t.chapter == null)
        {
            Debug.LogWarning("沒找到這提示編號： " + chapterName);
            return;
        }

        hintText.text = t.content;
        audioManager.PlayVoice(chapterName);
        if (totalStep > 0)
        {
            currStep++;
            SetStepText(currStep);
        }
    }

    //  按下關閉按鈕
    public void OnCloseBtnClicked()
    {
        hintPanel.SetActive(false);
        nextButton.SetActive(false);
        if (hintStepText != null)
            hintStepText.text = "";
    }

    //設定步驟文字
    public void SetStepText(int step)
    {
        if (hintStepText == null)
            return;
        currStep = step;
        hintStepText.text = currStep + " / " + totalStep;
    }
    //設定步驟文字
    public void SetTotalStep(int step)
    {
        currStep = 0;
        totalStep = step;
    }

    public void ShowNextButton(GameObject sender)
    {
        Sender = sender;
        nextButton.SetActive(true);
    }

    public void NextButtonClick()
    {
        nextButton.SetActive(false);
        hintPanel.SetActive(false);
        Sender.SendMessage("CloseHint", SendMessageOptions.DontRequireReceiver);
    }
}
