using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSnap : MonoBehaviour
{
    public float snapDistance = 0.5f; // 吸附距离
    private Vector3 originalPosition; // 原始位置
    [SerializeField] GameObject point;
    private bool isBeingHeld = false; // 是否正在被拿起
    private bool isDragging = false; // 是否正在被鼠标拖拽

    void Start()
    {
        // 记录物件的原始位置
        originalPosition = point.transform.position;
    }

    void Update()
    {
        if (!isBeingHeld && !isDragging)
        {
            // 如果未被拿起或拖拽，检测吸附逻辑
            float distance = Vector3.Distance(transform.position, point.transform.position);
            if (distance <= snapDistance)
            {
                SnapToOriginalPosition();
            }
        }

        if (isDragging)
        {
            // 鼠标拖拽物件
            DragObject();
        }
    }

    private void SnapToOriginalPosition()
    {
        transform.position = point.transform.position;
        // Debug.Log("物件已吸附回原始位置！");
    }

    private void DragObject()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // 确保深度不变
        transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // 鼠标按下时调用
    private void OnMouseDown()
    {
        isDragging = true;
        OnPickup(); // 调用物件被拿起逻辑
    }

    // 鼠标释放时调用
    private void OnMouseUp()
    {
        isDragging = false;
        OnDrop(); // 调用物件被放下逻辑
    }

    // 当物件被拿起时调用
    public void OnPickup()
    {
        isBeingHeld = true;
        Debug.Log("物件被拿起！");
    }

    // 当物件被放下时调用
    public void OnDrop()
    {
        isBeingHeld = false;
        Debug.Log("物件被放下！");
    }
}
