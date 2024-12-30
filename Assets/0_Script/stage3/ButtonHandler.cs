using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button p0Button;  // Button component for p0
    public GameObject part1; // GameObject to be shown when button is clicked
    public GameObject no2;   // GameObject to be hidden when button is clicked
    public Canvas canvas;    // Canvas to be hidden when button is clicked

    public GameObject Atomicdetection;
    public GameObject abc;
    private void Start()
    {
        // 确保 part1 初始状态为隐藏
        if (part1 != null)
        {
            part1.SetActive(false);
        }

        // 为按钮添加点击事件监听器
        if (p0Button != null)
        {
            p0Button.onClick.AddListener(HandleButtonClick);
        }
    }

    private void HandleButtonClick()
    {
        // 显示 part1
        if (part1 != null)
        {
            part1.SetActive(true);
        }
        if (abc != null)
        {
            abc.SetActive(true);
        }
        if (Atomicdetection != null)
        {
            Atomicdetection.SetActive(true);
        }

        // 隐藏 no2
        if (no2 != null)
        {
            no2.SetActive(false);
        }

        // 隐藏 Canvas
        if (canvas != null)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
