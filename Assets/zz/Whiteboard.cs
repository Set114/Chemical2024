using UnityEngine;
using TMPro;

public class Whiteboard : MonoBehaviour
{
    public int value = 0;
    public TMP_Text text;

    public void ChangeValue(int change)
    {
        value += change;
        // 更新顯示邏輯
        text.text = value.ToString();
    }

    public void ResetValue()
    {
        value = 0;
        text.text = value.ToString();
    }
}
