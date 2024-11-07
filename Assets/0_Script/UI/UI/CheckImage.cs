using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckImage : MonoBehaviour
{
    [Header("Images")]
    public Image[] imagesToSwitch; // 把單一圖片變成陣列
    
    [Header("Sprites")]
    public Sprite sprite1; // 第一張圖片
    public Sprite sprite2; // 第二張圖片

    [Header("Buttons")]
    public Button[] buttonsToDisable; // 按鈕陣列
    

    void Start()
    {
        
    }

    // 使用索引切換到第二張圖片 (sprite2) 並禁用相應的按鈕
    public void SwitchImage(int index)
    {
        if (index >= 0 && index < imagesToSwitch.Length)
        {
            imagesToSwitch[index].sprite = sprite2;
            if (index < buttonsToDisable.Length)
            {
                buttonsToDisable[index].interactable = false;
            }
        }
    }

    // 使用索引將圖片切換回第一張圖片 (sprite1) 並啟用相應的按鈕
    public void SwitchToFirstImage(int index)
    {
        if (index >= 0 && index < imagesToSwitch.Length)
        {
            imagesToSwitch[index].sprite = sprite1;
            if (index < buttonsToDisable.Length)
            {
                buttonsToDisable[index].interactable = true;
            }
        }
    }
}
