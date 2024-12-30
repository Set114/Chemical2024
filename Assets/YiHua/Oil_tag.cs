using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Oil_tag : MonoBehaviour
{
    public LevelEndSequence levelEndSequence;
    public GameObject Oil;
    public GameObject UI03;
    public GameObject next_levelTV;
    public GameObject next_level202;
    public GameObject next_level201;
    public GameObject next_levelItem;
    public string ragwaterTag = "rag_water";
    public string ragTag = "rag";

    // 引用 SwitchItem
    public SwitchItem switchItem;
    private LevelObjManager levelObjManager;

    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        // 如果碰到標籤為 rag_water 的物體
        if (other.CompareTag(ragwaterTag))
        {
            Debug.Log("碰到rag_water");
            StartCoroutine(DisableOilWithDelay()); // 啟動協程來延遲禁用 Oil
        }
        // 如果碰到標籤為 rag 的物體
        if (other.CompareTag(ragTag))
        {
            Debug.Log("碰到rag");
            StartCoroutine(ShowAndHideUI()); // 啟動協程來顯示和隱藏 UI
        }
    }

    // 協程：延遲 3 秒後禁用 Oil
    private IEnumerator DisableOilWithDelay()
    {
        yield return new WaitForSeconds(3f); // 等待 3 秒
        if (Oil != null)
        {
            Debug.Log("等待三秒");
            Oil.SetActive(false); // 禁用 Oil
        }

        // 切換到下一關
        if (switchItem != null)
        {
            switchItem.ChangeLevel();  // 呼叫 SwitchItem 的 ChangeLevel 來顯示下一關的物品
        }

        if (levelEndSequence != null)
        {
           // Debug.Log("場景切換");
           /*
            levelEndSequence.EndLevel(false, false, 1f, 1f, 1f, "1", () =>
            {
                next_levelItem.SetActive(true);//Item
                next_level201.SetActive(false);  //2-1
                next_levelTV.SetActive(true); //TV
                next_level202.SetActive(true);  //2-2
            });*/
            levelObjManager.LevelClear("1", "");
        }
    }

    // 協程：顯示 UI，等待 3 秒後隱藏 UI
    private IEnumerator ShowAndHideUI()
    {
        UI03.SetActive(true); // 顯示 UI
        yield return new WaitForSeconds(3f); // 等待 3 秒
        UI03.SetActive(false); // 隱藏 UI
    }
}
