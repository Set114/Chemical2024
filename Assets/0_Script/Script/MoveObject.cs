using UnityEngine;

public class MoveObject : MonoBehaviour
{
    // 起始点（A 点）
    public Vector3 startPoint = Vector3.zero;

    // 目标点（B 点）
    public Vector3 endPoint = new Vector3(10, 0, 0);

    // 移动时间（秒）
    public float moveDuration = 5.0f;

    // 移动的时间计数器
    private float elapsedTime = 0.0f;

    // 更新函数在每一帧被调用
    void Update()
    {
        // 如果计数器小于移动时间
        if (elapsedTime < moveDuration)
        {
            // 增加计时器
            elapsedTime += Time.deltaTime;

            // 计算插值系数（0 到 1 之间）
            float t = elapsedTime / moveDuration;

            // 使用线性插值移动物体
            transform.position = Vector3.Lerp(startPoint, endPoint, t);
        }
    }
}
