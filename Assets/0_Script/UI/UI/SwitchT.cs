using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LevTData
{
    [Header("提示名稱")]
    public string levelName; 

    [Header("提示文字")]
    public GameObject[] levelObjects; 
}

public class SwitchT : MonoBehaviour
{
    [Header("Canvas_ElF_T")]
    [SerializeField] GameObject TUI;

    [Header("提示數據")]
    [SerializeField] private LevTData[] levelsData;

    void Start()
    {
    }

    // 切換到指定提示
    public void ChangeLevel(int levelIndex)
    {
        TUI.SetActive(true);
        // 先隱藏所有提示
        for (int i = 0; i < levelsData.Length; i++)
        {
            SetGameObjectsActive(levelsData[i].levelObjects, false);
        }

        if (levelIndex >= 0 && levelIndex < levelsData.Length && levelsData[levelIndex].levelObjects != null && levelsData[levelIndex].levelObjects.Length > 0)
        {
            SetGameObjectsActive(levelsData[levelIndex].levelObjects, true);
        }
        else
        {
            //Debug.Log("關卡索引超出範圍或該關卡沒有可顯示的物品");
        }
    }
    
    // 設置物件活動狀態
    void SetGameObjectsActive(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }
    public void CloseT()
    {
        TUI.SetActive(false);
        for (int i = 0; i < levelsData.Length; i++)
        {
            SetGameObjectsActive(levelsData[i].levelObjects, false);
        }
    }
}
