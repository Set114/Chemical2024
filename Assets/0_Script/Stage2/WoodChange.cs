using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodChange : MonoBehaviour
{
    public GameObject targetObject; // 当前需要缩小的物件
    public GameObject objectToActivate; // 缩小完成后需要激活的物件
    public Text uiText; // 用于显示文字的 UI 元素
    public string initialText = "20G"; // 初始文字
    public string finalText = "10G"; // 缩小完成后的文字
    public float scaleDuration = 3.0f; // 缩小时间

    private bool isScaling = false; // 是否正在缩小
    private Vector3 originalScale; // 原始缩放大小
    private float elapsedTime = 0f; // 经过时间

    void Start()
    {
        targetObject = this.gameObject;

        if (targetObject == null || objectToActivate == null || uiText == null)
        {
            Debug.LogError("请指定缩小物件、目标激活物件和 UI 文本组件！");
        }

        // 初始化文字
        if (uiText != null)
        {
            uiText.text = initialText;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //  修改了指定物件名稱
        if (!isScaling && targetObject != null && objectToActivate != null && other.gameObject.name.Equals("fire"))
        {
            StartScaling(); // 开始缩小
        }
    }

    void Update()
    {
        if (isScaling)
        {
            // 更新缩小过程
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / scaleDuration);

            // 线性插值计算缩放值
            targetObject.transform.localScale = Vector3.Lerp(originalScale, Vector3.one * 0.05f, t);

            // 缩小完成后执行关闭和激活操作
            if (t >= 1f)
            {
                CompleteScaling();
            }
        }
    }

    private void StartScaling()
    {
        // 初始化缩小参数
        originalScale = targetObject.transform.localScale;
        elapsedTime = 0f;
        isScaling = true;
        Debug.Log("开始缩小物件...");
    }

    private void CompleteScaling()
    {
        isScaling = false;
        Debug.Log("缩小完成！");

        // 更新 UI 文字
        if (uiText != null)
        {
            uiText.text = finalText;
            Debug.Log($"文字更新为：{finalText}");
        }

        // 关闭当前物件
        targetObject.SetActive(false);

        // 激活目标物件
        objectToActivate.SetActive(true);
    }
}
