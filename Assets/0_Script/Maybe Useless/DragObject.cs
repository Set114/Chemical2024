using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 offset;
    private float zDistance;

    void Start()
    {
        // 獲取主相機
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        // 記錄物件與鼠標之間的偏移量
        zDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = zDistance;
        offset = transform.position - mainCamera.ScreenToWorldPoint(mousePosition);

        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            // 計算鼠標在世界中的新位置
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = zDistance;
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(mousePosition) + offset;

            // 更新物件位置
            transform.position = newPosition;
        }
    }

    void OnMouseUp()
    {
        // 停止拖動
        isDragging = false;
    }
}
