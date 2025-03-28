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
    [Tooltip("縮小按鈕")] [SerializeField] GameObject minimizeButton;
    [Tooltip("結束按鈕")] [SerializeField] GameObject nextButton;

    [Tooltip("目前步驟")]
    [SerializeField] private int currStep = 1;

    AudioManager audioManager;          //音樂管理
    GameObject Sender;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();

        hintPanel.SetActive(false);
        minimizeButton.SetActive(false);
        nextButton.SetActive(false);
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
    }

    /* 感覺完全沒用到的功能
    //按下確認按鈕
    public void OnConfirmBtnClicked()
    {
        CloseHintPanel();
        gm.LevelStart();
    }
    */

    //  按下關閉按鈕
    public void OnCloseBtnClicked()
    {
        hintPanel.SetActive(false);
    }

    //  取得目前步驟
    public int GetCurrStep()
    {
        return currStep;
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
        Sender.SendMessage("CloseHint");
    }
}
