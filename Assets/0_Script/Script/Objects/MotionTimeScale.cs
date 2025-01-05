using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTimeScale : MonoBehaviour
{
    public Vector3 startScale = Vector3.one;    // ��l���
    public Vector3 endScale = Vector3.zero;    // �������
    public float duration = 20.0f;              // �Y��һݮɶ�

    float elapsedTime = 0f;

    private void OnEnable()
    {
        startScale = this.gameObject.transform.localScale;
        endScale = this.gameObject.transform.localScale * 0.5f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < duration)
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
        else
        {        // �T�O�̫��Һ�ǹF��ؼФ��
            transform.localScale = endScale;
            Destroy(this.GetComponent<MotionTimeScale>());
        }
    }
}
