using UnityEngine;
using UnityEngine.UI;

public class PositionLockManager : MonoBehaviour
{
    public Transform[] targetPoints;
    public Transform CorrectPoint; // ���T�a�I�� Transform
    public Rigidbody rb; // ���骺 Rigidbody
    public MonoBehaviour otherScript; // �n���Ϊ��t�@�Ӹ}��
    public CorrectPosition correctPositionScript;
    public bool isLocked = false;
    public Transform originalPosition; // ��l��m

    void Update()
    {
        foreach (Transform targetPoint in targetPoints)
        {
            // �p�G����P���N�@�ӥؼЦ�m�������Z���p��Y���H�ȡA��ܨ�F�F�ؼЦ�m
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.05f)
            {
                if (CorrectPoint == targetPoint)
                {
                    // �N�����ǻ��� CorrectPosition.c �}��
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
        //��w���骺��m��targetPoint����m
        transform.position = targetPoint.position;
        //��w���骺���ରtargetPoint������
        transform.rotation = targetPoint.rotation;
        //��w���骺Rigidbody����m�P����
        rb.constraints = RigidbodyConstraints.FreezeAll;
        isLocked = true;
    }

    // ������w���骺��m�M����
    public void UnlockRigidbody()
    {

        // �N���鲾�^��l��m
        transform.position = originalPosition.position;
        transform.rotation = originalPosition.rotation;
        //Debug.Log("UnlockRigidbody-originalPosition" + originalPosition + rb);
        rb.constraints = RigidbodyConstraints.None;
        isLocked = false;
    }
}