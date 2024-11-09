using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CheckImage : MonoBehaviour
{
    [Header("需要改變的清單圖項")]
    public Image[] imagesToSwitch; 

    [Header("切換關卡的清單案紐")]
    public Button[] buttonsToDisable; // 按鈕陣列
    
    [Header("Sprites")]
    public Sprite sprite1; // 未完成
    public Sprite sprite2; // 完成
    
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

    // 不會用到但保留
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
