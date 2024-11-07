using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class LevelData2
{
    [Header("關卡名稱")]
    public string levelName; // 關卡名稱

    [Header("關卡物品")]
    public GameObject[] levelObjects; // 關卡物品
    [Header("Text")]
    [SerializeField] public TMP_Text showLevelText;

}

public class SwitchLevelObjects_5 : MonoBehaviour
{
    [Header("切換關卡按鈕")]
    [SerializeField] private Button[] changeLevelButtons; // 切換關卡按鈕陣列
    [SerializeField] private Button test_btn; // 切換關卡按鈕

    [Header("關卡數據")]
    [SerializeField] private LevelData2[] levelsData; // 每個關卡的數據陣列
    public int leveltestindex; // 初始化時設置為 -1，這樣可以在第一次切換時顯示第一關

    private int currentLevelIndex = -1; // 初始化時設置為 -1，這樣可以在第一次切換時顯示第一關
    private bool isFirstSwitch = true; // 標記是否是第一次切換
    //[Header("ModeTest")]
    //[SerializeField] public Button TestPlayButton;
    //[SerializeField] public Button level2_btn;

    public Stage5_UIManager stage5_UIManager;
    public CameraController cameraController;

    [SerializeField] public Button showTestitem;
    // public PlaySpeechAudio playSpeechAudio;
    // public FireInteraction fireInteraction;


    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        // 设置切换关卡按钮的监听器
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

        showTestitem.onClick.AddListener(showtestlevel);

        // 预设不显示任何关卡的物品
        for (int i = 0; i < levelsData.Length; i++)
        {
            SetGameObjectsActive(levelsData[i].levelObjects, false);
        }

        // 如果有关卡数据，则初始化第一个关卡的文本显示
        if (levelsData.Length > 0)
        {
            LevelData2 initialLevel = levelsData[0];

            // 如果 `showLevelText` 不为空，设置为关卡名称
            if (initialLevel.showLevelText != null)
            {
                initialLevel.showLevelText.text = initialLevel.levelName;
            }
        }
    }

    // 切换到下一关的物品
    public void ChangeLevel()
    {

        if (cameraController != null)
        {
           cameraController.ResetCameraSize();
        }

        // 打印当前关卡索引
        //Debug.Log($"Current Level Index: {currentLevelIndex}");

        // 关闭当前关卡的物品
        if (currentLevelIndex >= 0 && currentLevelIndex < levelsData.Length)
        {
            SetGameObjectsActive(levelsData[currentLevelIndex].levelObjects, false);
        }

        Debug.Log("currentLevelIndex" + currentLevelIndex);
        // 更新当前关卡索引
        currentLevelIndex = (currentLevelIndex + 1) % levelsData.Length;
        Debug.Log("now currentLevelIndex" + currentLevelIndex); 
        // 打开新关卡的物品
        SetGameObjectsActive(levelsData[currentLevelIndex].levelObjects, true);


        // 更新显示的关卡名称
        if (levelsData[currentLevelIndex].showLevelText != null)
        {
            levelsData[currentLevelIndex].showLevelText.text = levelsData[currentLevelIndex].levelName;
        }

        // 在首次切换关卡后，将标记设置为 false
        if (isFirstSwitch)
        {
            isFirstSwitch = false;
        }
    }

    // 設置物件的活動狀態
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

    public void showtestlevel()
    {
        if (cameraController != null)
        {
            cameraController.ResetCameraSize();
        }
        Debug.Log("showtestlevel" + leveltestindex);

        // 打印当前关卡索引
        //Debug.Log($"Current Level Index: {currentLevelIndex}");

        // 关闭当前关卡的物品
        if (currentLevelIndex >= 0 && currentLevelIndex < levelsData.Length)
        {
            SetGameObjectsActive(levelsData[currentLevelIndex].levelObjects, false);
        }

        // 更新当前关卡索引
        currentLevelIndex = leveltestindex % levelsData.Length;


        // 打开新关卡的物品
        SetGameObjectsActive(levelsData[currentLevelIndex].levelObjects, true);


        // 更新显示的关卡名称
        if (levelsData[currentLevelIndex].showLevelText != null)
        {
            levelsData[currentLevelIndex].showLevelText.text = levelsData[currentLevelIndex].levelName;
        }

        // 在首次切换关卡后，将标记设置为 false
        if (isFirstSwitch)
        {
            isFirstSwitch = false;
        }
    }

    public void SetCurrentLevel(int level)
    {
        currentLevelIndex = level;
    }


    public void ListSwitch(int index)
    {
        //playSpeechAudio.SetCurrentLevel(index);
        cameraController.SetCurrentLevel(index);
        stage5_UIManager.SetCurrentLevel(index);
        currentLevelIndex = index;
    }
}
