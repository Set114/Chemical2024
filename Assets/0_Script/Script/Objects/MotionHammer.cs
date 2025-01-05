using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionHammer : MonoBehaviour
{
    public float rotationAngle = 45f; // 要旋轉的角度
    public float speed = 20f;        // 旋轉和回到原位的速度

    private Quaternion originalRotation; // 原始的旋轉角度
    private Quaternion targetRotation;   // 目標的旋轉角度

    int Status = 0;

    void Start()
    {
        // 儲存物件的原始旋轉
        originalRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles.x + rotationAngle, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    void Update()
    {
        if( Status == 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            // 旋轉到目標角度
            if(Quaternion.Angle(transform.rotation, targetRotation) < 0.01f )
            {
                // 矯正到精確目標角度
                transform.rotation = targetRotation;
                Status = 1;
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * speed);
            // 旋轉回原始角度
            if(Quaternion.Angle(transform.rotation, originalRotation) < 0.01f)
            {
                transform.rotation = originalRotation;
                Destroy(this.GetComponent<MotionHammer>());
            }           
        }
    }
}
