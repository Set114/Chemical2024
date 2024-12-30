using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceBlockCollisionStage1UI : MonoBehaviour
{
    [Header("UI")]
    public GameObject LearnUI;
    public GameObject TestUI;
    public GameObject TestShowUI;
    [Header("Questions UI")]
    public GameObject[] questionUIs; // 使用陣列來存放 Q1, Q2, Q3 的 UI
    public GameObject T213UI;
    [Header("Item")]
    public GameObject item;

    [Header("Button")]
    public Button playButton;


    void Start()
    {
        // 檢查陣列是否有正確設置
        if (questionUIs == null || questionUIs.Length < 3)
        {
            Debug.Log("請確保 questionUIs 陣列包含至少 3 個 UI 元素。");
        }
    }

    public void Q1MarkUIShow()
    {
        LearnUI.SetActive(false);
        TestUI.SetActive(true);
    }

    public void ShowQuestionUI(int questionIndex)
    {
        TestUI.SetActive(true);
        TestUIHide();
        
        if (questionIndex >= 0 && questionIndex < questionUIs.Length)
        {
            questionUIs[questionIndex].SetActive(true); // 顯示對應的問題 UI
        }
        else
        {
            Debug.Log("問題索引超出範圍。");
        }
    }

    public void T213ShowUI()
    {
        TestUIShow();
        item.SetActive(false);

        TestUI.SetActive(true);
        T213UI.SetActive(true);
        playButton.gameObject.SetActive(false);
    }
    public void TestUIHide()
    {
        TestShowUI.SetActive(false);
    }
    public void TestUIShow()
    {
        TestShowUI.SetActive(true);
    }
}
