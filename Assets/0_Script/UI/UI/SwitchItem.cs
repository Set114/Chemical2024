using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class LevData
{
    [Header("關卡名稱")]
    public string levelName; // 關卡名稱

    [Header("關卡物品")]
    public GameObject[] levelObjects; // 關卡物品
}

public class SwitchItem : MonoBehaviour
{
    [Header("切換關卡按鈕")]
    [SerializeField] private Button[] changeLevelButtons; // 切換關卡按鈕陣列

    [Header("關卡數據")]
    [SerializeField] private LevData[] levelsData; // 每個關卡的數據陣列

    
    [Header("測試用")]
    [SerializeField] private Button test_btn; // 切換關卡按鈕
    
    private int currentLevelIndex = -1;
    
    [Header("關卡文字顯示")]
    [SerializeField] public TMP_Text showLevelText;
    [SerializeField] public GameObject item;

    void Start()
    {
        if (changeLevelButtons != null)
        {
            foreach (Button button in changeLevelButtons)
            {
                if (button != null)
                {
                    button.onClick.AddListener(ChangeLevel);
                }
            }
        }

        if (test_btn != null)
        {
            test_btn.onClick.AddListener(ChangeLevel);
        }

        CloseAllItem();

        // 如果有關卡數據，則初始化第一個關卡的文字顯示
        if (levelsData.Length > 0)
        {
            LevData initialLevel = levelsData[0];

            if (showLevelText != null)
            {
                showLevelText.text = initialLevel.levelName;
            }
        }
    }

    // 切換到下一關的物品
    public void ChangeLevel()
    {
        CloseAllItem();
        item.SetActive(true);

        // 打開新關卡的的物品
        SetGameObjectsActive(levelsData[currentLevelIndex].levelObjects, true);


        // 更新顯示關卡名稱
        // if (showLevelText != null)
        // {
        //     showLevelText.text = levelsData[currentLevelIndex].levelName;
        // }
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

    // 關閉所有物件
    void CloseAllItem()
    {
        for (int i = 0; i < levelsData.Length; i++)
        {
            SetGameObjectsActive(levelsData[i].levelObjects, false);
        }
    }

    // 獲取目前在第幾關
    public void SetCurrentLevel(int levelindex)
    {
        currentLevelIndex = levelindex;
    }

    // 獲取目前在第幾關
    public void UpdateLevelName(int levelindex)
    {
        if (showLevelText != null)
        {
            showLevelText.text = levelsData[levelindex].levelName;
        }
    }
}
