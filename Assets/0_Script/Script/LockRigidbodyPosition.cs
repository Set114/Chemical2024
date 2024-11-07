using UnityEngine;

public class LockRigidbodyPosition : MonoBehaviour
{
    public Transform targetPoint; // 特定地點的 Transform
    public Rigidbody rb; // 物體的 Rigidbody
    public MonoBehaviour otherScript; // 要停用的另一個腳本

    void Update()
    {
        // 假設當前位置與目標位置之間的距離小於某個閾值，表示到達了目標位置
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            LockRigidbody();

            // 停用其他腳本
            if (otherScript != null)
            {
                otherScript.enabled = false;
            }
        }
    }

    void LockRigidbody()
    {
        // 如果 Rigidbody 不存在，則返回
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned!");
            return;
        }
        //鎖定物體的位置為targetPoint的位置
        transform.position = targetPoint.position;
        //鎖定物體的選轉為targetPoint的旋轉
        transform.rotation = targetPoint.rotation;
        //鎖定物體的Rigidbody的位置與旋轉
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
