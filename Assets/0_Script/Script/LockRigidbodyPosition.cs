using UnityEngine;

public class LockRigidbodyPosition : MonoBehaviour
{
    public Transform targetPoint; // �S�w�a�I�� Transform
    public Rigidbody rb; // ���骺 Rigidbody
    public MonoBehaviour otherScript; // �n���Ϊ��t�@�Ӹ}��

    void Update()
    {
        // ���]��e��m�P�ؼЦ�m�������Z���p��Y���H�ȡA��ܨ�F�F�ؼЦ�m
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            LockRigidbody();

            // ���Ψ�L�}��
            if (otherScript != null)
            {
                otherScript.enabled = false;
            }
        }
    }

    void LockRigidbody()
    {
        // �p�G Rigidbody ���s�b�A�h��^
        if (rb == null)
        {
            Debug.LogError("Rigidbody is not assigned!");
            return;
        }
        //��w���骺��m��targetPoint����m
        transform.position = targetPoint.position;
        //��w���骺���ରtargetPoint������
        transform.rotation = targetPoint.rotation;
        //��w���骺Rigidbody����m�P����
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
