using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonArrayStopAudio : MonoBehaviour
{
    public List<Button> buttons; // 按鈕的陣列
    public AudioManager audioManager;

    void Start()
    {
        // 遍歷每個按鈕，並為它們添加點擊事件
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClicked);
            }
            else
            {
                Debug.LogWarning("按鈕陣列中有空元素，請檢查陣列配置。");
            }
        }
    }

    // 當按鈕按下時調用此方法
    private void OnButtonClicked()
    {
        audioManager.Stop();
    }
}
