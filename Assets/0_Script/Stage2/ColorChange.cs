using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour
{
    public Material targetMaterial; // 指定的材质
    public Color startColor = Color.grey; // 初始颜色（银色）
    public Color orangeColor = new Color(1f, 0.5f, 0f); // 目标颜色（橙色）
    public Color blackColor = Color.black; // 目标颜色（黑色）
    public float duration = 5.0f; // 每个阶段的过渡时间（秒）

    public Text uiText; // 指定的 UI 文本（UGUI Text）
    public string initialText = "20G"; // 初始文字
    public string finalText = "30G"; // 目标文字

    private float elapsedTime = 0f; // 已经过的时间
    private bool isTransitioning = false; // 是否正在进行过渡
    private int transitionStage = 0; // 当前过渡阶段：0 - 未启动，1 - 银色到橙色，2 - 橙色到黑色

    void Start()
    {
        if (targetMaterial != null)
        {
            targetMaterial.color = startColor; // 初始化为起始颜色
        }
        else
        {
            Debug.LogError("请指定目标材质！");
        }

        if (uiText != null)
        {
            uiText.text = initialText; // 初始化文字
        }
        else
        {
            Debug.LogError("请指定 UI 文本组件！");
        }
    }

    void Update()
    {
        if (!isTransitioning) return; // 如果未启动过渡，直接返回

        // 计算进度
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        // 根据阶段调整颜色
        if (transitionStage == 1)
        {
            // 从银色变到橙色
            targetMaterial.color = Color.Lerp(startColor, orangeColor, t);

            if (t >= 1f) // 第一阶段完成
            {
                StartNextTransition(orangeColor, blackColor, 2);
            }
        }
        else if (transitionStage == 2)
        {
            // 从橙色变到黑色
            targetMaterial.color = Color.Lerp(orangeColor, blackColor, t);

            if (t >= 1f) // 第二阶段完成
            {
                isTransitioning = false;
                Debug.Log("所有颜色过渡完成！");
                UpdateUIText(); // 修改文字内容
            }
        }
    }

    // 触发事件，启动颜色过渡
    private void OnTriggerEnter(Collider other)
    {
        if (!isTransitioning && other.gameObject.name.Equals("GAS"))
        {
            StartNextTransition(startColor, orangeColor, 1);
        }
    }

    // 启动下一个过渡阶段
    private void StartNextTransition(Color fromColor, Color toColor, int nextStage)
    {
        elapsedTime = 0f; // 重置时间
        transitionStage = nextStage; // 设置当前阶段
        isTransitioning = true; // 启动过渡
    }

    // 更新 UI 文本
    private void UpdateUIText()
    {
        if (uiText != null)
        {
            uiText.text = finalText; // 修改为目标文字
            Debug.Log($"文字更新为：{finalText}");
        }
    }
}
