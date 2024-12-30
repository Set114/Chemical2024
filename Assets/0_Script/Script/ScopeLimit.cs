using UnityEngine;

public class ScopeLimit : MonoBehaviour
{
    public Vector3 minPosition; // 矩形左下角
    public Vector3 maxPosition; // 矩形右上角

    void Update()
    {
        // 获取物体当前位置
        Vector3 position = transform.position;

        // 限制物体的位置在矩形范围内
        position.x = Mathf.Clamp(position.x, minPosition.x, maxPosition.x);
        position.y = Mathf.Clamp(position.y, minPosition.y, maxPosition.y);
        position.z = Mathf.Clamp(position.z, minPosition.z, maxPosition.z);

        // 设置物体的新位置
        transform.position = position;
    }
}
