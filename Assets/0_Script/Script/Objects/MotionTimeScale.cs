using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTimeScale : MonoBehaviour
{
    public Vector3 startScale = Vector3.one;    // 初始比例
    public Vector3 endScale = Vector3.zero;    // 結束比例
    public float duration = 20.0f;              // 縮放所需時間

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
        {        // 確保最後比例精準達到目標比例
            transform.localScale = endScale;
            Destroy(this.GetComponent<MotionTimeScale>());
        }
    }
}
