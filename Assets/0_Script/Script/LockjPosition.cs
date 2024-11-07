using UnityEngine;

public class LockPosition : MonoBehaviour
{
    public Transform lockPoint; // 要鎖定的點
    public Rigidbody rb; // 物體的 Rigidbody
    private bool isLocked = false; // 是否鎖定

    void Update()
    {
        if (!isLocked)
        {
            // 计算物体到锁定点的距离
            float distanceToLockPoint = Vector3.Distance(transform.position, lockPoint.position);

            // 如果距离小于某个阈值，就锁定物体
            if (distanceToLockPoint < 0.1f)
            {
                LockObject();
            }
        }
    }

    void LockObject()
    {
        isLocked = true;
        // 锁定物体的位置为锁定点的位置
        transform.position = lockPoint.position;
        // 锁定物体的旋转为锁定点的旋转
        transform.rotation = lockPoint.rotation;
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }
}
