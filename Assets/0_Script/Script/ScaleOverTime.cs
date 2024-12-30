using UnityEngine;

public class ScaleOverTime : MonoBehaviour
{
    // 开始缩放的初始大小
    public Vector3 initialScale;

    // 最终缩放的目标大小
    public Vector3 targetScale;

    // 缩放持续的时间
    public float duration = 1.0f;

    // 记录已经过去的时间
    private float timer = 0.0f;

    private void Start()
    {
        // 设置初始缩放大小
        transform.localScale = initialScale;
    }

    private void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;

        // 计算当前时间点的插值因子
        float t = Mathf.Clamp01(timer / duration);

        // 根据插值因子计算当前缩放大小
        Vector3 currentScale = Vector3.Lerp(initialScale, targetScale, t);

        // 应用当前缩放大小
        transform.localScale = currentScale;

        // 如果已经达到目标缩放大小，停止更新
        if (t >= 1.0f)
        {
            enabled = false;
        }
    }
}
