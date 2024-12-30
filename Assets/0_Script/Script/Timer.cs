using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{
    public Button button;
    public TMP_Text timerText; // 引用 Text 组件
    float timer_f;
    public bool isCounting = false;

    void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
        timerText.text = "0.0"; // 初始时显示计时器为0
    }

    void Update()
    {
        if (isCounting)
        {
            timer_f += Time.deltaTime; // 累加时间
            timerText.text = timer_f.ToString("F1"); // 更新显示的文本内容
        }
    }

    public void OnButtonClicked()
    {
        isCounting = true;
        timer_f = 0f; // 重置计时器值
    }
}
