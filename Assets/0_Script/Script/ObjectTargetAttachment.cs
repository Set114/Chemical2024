using UnityEngine;

public class ObjectTargetAttachment : MonoBehaviour
{
    public Transform[] targetPoints;
    public Transform correctPoint; // 正確地點的 Transform
    public Transform originalPosition;// 原地點的 Transform
    public Rigidbody rb; // 物體的 Rigidbody
    public MonoBehaviour otherScript; // 要停用的另一個腳本
    public CorrectPosition correctPositionScript;
    public string targetTag = "";
    public bool isLocked = false;

    void Start()
    {
        correctPositionScript = FindObjectOfType<CorrectPosition>();
        if (correctPositionScript == null)
        {
            Debug.LogError("CorrectPosition script not found!");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // 判斷碰撞到的是哪個目標點
            bool targetPointFound = false;
            for (int i = 0; i < targetPoints.Length; i++)
            {
                if (other.transform == targetPoints[i])
                {
                    // 將物體吸附到對應的目標點上
                    AttachToTargetPoint(targetPoints[i]);
                    targetPointFound = true;
                    break;
                }
            }
               // 將消息傳遞給 CorrectPosition.c 腳本
                if (targetPointFound)
                {
                    for (int j = 0; j < targetPoints.Length; j++)
                    {
                        if (correctPoint == targetPoints[j])
                        {
                            if (correctPositionScript != null)
                            {
                                correctPositionScript.CheckAllObjectsPlaced();
                            }
                            break;
                        }
                    }
                }
            
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, originalPosition.position) > 0.05f)
        {
            bool isAtTargetPoint = false;
            for (int i = 0; i < targetPoints.Length; i++)
            {
                if (Vector3.Distance(transform.position, targetPoints[i].position) < 0.05f)
                {
                    isAtTargetPoint = true;
                    break;
                }
            }

            if (!isAtTargetPoint)
            {
                // 如果不在目標點位置且與原始位置有一定距離，回到原始位置
                transform.position = originalPosition.position;
                transform.rotation = originalPosition.rotation;
                isLocked = false;
            }
        }
    }

    void AttachToTargetPoint(Transform targetPoint)
    {
        // 將物體吸附到指定的點上
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
        isLocked = true;
    }
}
