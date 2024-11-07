using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera; // 你要控制的攝影機
    public float zoomSpeed = 1f; // 放大速度
    public float minZoom = 10f; // 最小視野值
    public float maxZoom = 60f; // 最大視野值
    public float targetDistance = 20f; // 目標距離

    void Update()
    {
        // 平滑過渡到目標距離
        float currentDistance = Vector3.Distance(mainCamera.transform.position, transform.position);
        float newDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSpeed);
        newDistance = Mathf.Clamp(newDistance, minZoom, maxZoom);

        // 設定新的攝影機位置
        mainCamera.transform.position = transform.position - mainCamera.transform.forward * newDistance;
    }
}
