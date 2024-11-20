using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationStateChecker : MonoBehaviour
{
    public GameObject DefultLiquid;
    public GameObject DefultLiquid2;
    public GameObject AfterLiquid;
    public GameObject image;
    public GameObject ball;
    public enum RotationState
    {
        None,       // 无状态
        Positive90, // 正向超出90度
        Negative90  // 负向超出90度
    }

    public RotationState currentState = RotationState.None; // 当前状态
    private bool hasTriggered = false; // 是否已经触发状态

    void Update()
    {
        if (hasTriggered) return; // 如果已经触发过状态，跳过检测

        // 获取物体的当前 Z 轴旋转角度
        float zRotation = NormalizeAngle(transform.eulerAngles.z);

        // 检查是否超出正负90度
        if (zRotation > 90f)
        {
            SetState(RotationState.Positive90);
        }
        else if (zRotation < -90f)
        {
            SetState(RotationState.Negative90);
        }
    }

    private void SetState(RotationState newState)
    {
        if (ball != null)
        {
            ball.GetComponent<balloon>().StartScaling();
        }
        DefultLiquid.SetActive(false);
        DefultLiquid2.SetActive(false);
        AfterLiquid.SetActive(true);
        image.SetActive(true);
        currentState = newState;
        hasTriggered = true; // 标记为已触发，避免重复
        Debug.Log($"状态更新为: {currentState}");
        GetComponentInChildren<PourSolution>().audioSource4.Play();
        GetComponentInChildren<PourSolution>().text.text = GetComponentInChildren<PourSolution>().Text[3];
    }

    private float NormalizeAngle(float angle)
    {
        // 将角度规范化到 -180 到 180 范围
        if (angle > 180f)
        {
            angle -= 360f;
        }
        return angle;
    }
}
