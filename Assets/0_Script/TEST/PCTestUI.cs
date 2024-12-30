using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCTestUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 獲取按鈕並添加事件監聽器
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("Button component not found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 當按鈕被點擊時觸發
    void OnButtonClick()
    {
        Debug.Log("1");
    }
}
