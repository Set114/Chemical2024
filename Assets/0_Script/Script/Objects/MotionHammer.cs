using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHammer : MonoBehaviour
{
    public float rotationAngle = 45f; // �n���઺����
    public float speed = 20f;        // ����M�^���쪺�t��

    private Quaternion originalRotation; // ��l�����ਤ��
    private Quaternion targetRotation;   // �ؼЪ����ਤ��

    int Status = 0;

    void Start()
    {
        // �x�s���󪺭�l����
        originalRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles.x + rotationAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void Update()
    {
        if( Status == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            // �����ؼШ���
            if(Quaternion.Angle(transform.rotation, targetRotation) < 0.01f )
            {
                // �B�����T�ؼШ���
                transform.rotation = targetRotation;
                Status = 1;
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * speed);
            // ����^��l����
            if(Quaternion.Angle(transform.rotation, originalRotation) < 0.01f)
            {
                transform.rotation = originalRotation;
                Destroy(this.GetComponent<MotionHammer>());
            }           
        }
    }
}
