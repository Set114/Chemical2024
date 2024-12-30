using UnityEngine;
using UnityEngine.UI;

public class PositionLockManager : MonoBehaviour
{
    public Transform[] targetPoints;
    public Transform CorrectPoint; // 正確地點的 Transform
    public Rigidbody rb; // 物體的 Rigidbody
    public MonoBehaviour otherScript; // 要停用的另一個腳本
    public CorrectPosition correctPositionScript;
    public bool isLocked = false;
    public Transform originalPosition; // 原始位置

    void Update()
    {
        foreach (Transform targetPoint in targetPoints)
        {
            // 如果物體與任意一個目標位置之間的距離小於某個閾值，表示到達了目標位置
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
            {
                if (CorrectPoint == targetPoint)
                {
                    // 將消息傳遞給 CorrectPosition.c 腳本
                    if (correctPositionScript != null)
                    {
                        correctPositionScript.CheckAllObjectsPlaced();
                    }
                }
                if (!IsOtherObjectAtTargetPoint(targetPoint) && !isLocked)
                {
                    LockRigidbody(targetPoint);
                    break;
                }
            }
            if(Vector3.Distance(transform.position, originalPosition.position) > 0.05f && IsOtherObjectAtTargetPoint(targetPoint))
            {
                UnlockRigidbody();
                break;
            }
        }
    }

    bool IsAnyTargetPoint(Vector3 position)
    {
        foreach (Transform targetPoint in targetPoints)
        {
            if (position == targetPoint.position)
            {
                //Debug.Log("IsAnyTargetPoint:T");
                return true;
            }
        }
        //Debug.Log("IsAnyTargetPoint:F");
        return false;
    }

    bool IsOtherObjectAtTargetPoint(Transform targetPoint)
    {
        if (transform.position == targetPoint.position)
        {
            Debug.Log("IsOtherObjectAtTargetPoint:T");
            return true;
        }
        else
        {
            Debug.Log("IsOtherObjectAtTargetPoint:F");
            return false;
        }

    }

    void LockRigidbody(Transform targetPoint)
    {
        Debug.Log(2);
        Debug.Log("targetPoint:" + targetPoint);
        //鎖定物體的位置為targetPoint的位置
        transform.position = targetPoint.position;
        //鎖定物體的選轉為targetPoint的旋轉
        transform.rotation = targetPoint.rotation;
        //鎖定物體的Rigidbody的位置與旋轉
        rb.constraints = RigidbodyConstraints.FreezeAll;
        isLocked = true;
    }

    // 取消鎖定物體的位置和旋轉
    public void UnlockRigidbody()
    {

        // 將物體移回原始位置
        transform.position = originalPosition.position;
        transform.rotation = originalPosition.rotation;
        //Debug.Log("UnlockRigidbody-originalPosition" + originalPosition + rb);
        rb.constraints = RigidbodyConstraints.None;
        isLocked = false;
    }
}