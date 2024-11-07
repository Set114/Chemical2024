using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CTrigger : MonoBehaviour
{
    public Transform[] targetPoints; // 目標點的 Transform
    public Transform correctPoint; // 正确地點的 Transform
    public Transform originalPosition; // 原地點的 Transform
    public GameObject warning;
    public CorrectPosition correctPositionScript;
    public MonoBehaviour otherScript; // 要停用的另一個腳本
    public bool botPlaced = false;
    public bool isLocked = false;
    //public string correctTag; // 正确位置的標籤
    //public string incorrectTag; // 錯誤位置的標籤

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectExited.AddListener(OnPutDown);
        warning.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (botPlaced)
        {
            bool targetPointFound = false;
            foreach (var targetPoint in targetPoints)
            {
                if (Vector3.Distance(other.transform.position, targetPoint.position) < 0.05f)
                {
                    if (other.transform.parent != null)
                    {
                        AttachToTargetPoint(targetPoint);
                        targetPointFound = true;
                        break;
                    }
                }
            }
            if (targetPointFound)
            {
                correctPositionScript?.CheckAllObjectsPlaced();
            }
        }
    }

    void AttachToTargetPoint(Transform targetPoint)
    {
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
        isLocked = true;
    }

    public void OnPutDown(SelectExitEventArgs args)
    {
        // 檢查物體地點
        bool isAtTargetPoint = false;
        foreach (var targetPoint in targetPoints)
        {
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
            {
                isAtTargetPoint = true;
                break;
            }
        }

        if (!isAtTargetPoint && Vector3.Distance(transform.position, originalPosition.position) > 0.05f)
        {
            transform.position = originalPosition.position;
            transform.rotation = originalPosition.rotation;
            isLocked = false;
            warning.SetActive(true); // 如果不在原點或目標點，則警告
        }
        else
        {
            warning.SetActive(false);
        }
    }
}
