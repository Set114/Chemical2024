using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloon : MonoBehaviour
{
    public GameObject aballoon;

    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f); // 目标缩放值
    public float duration = 3.0f; // 缩放持续时间

    private Vector3 originalScale = new Vector3(0.4f, 0.4f, 0.4f); // 原始缩放值
    private float elapsedTime = 0f; // 已经过的时间
    private bool isScaling = false; // 是否正在缩放

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("bomb-a"))
        {
            aballoon.SetActive(true);
            aballoon.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            other.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.D))
        // {
        //     aballoon.SetActive(true);
        //     aballoon.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        // }

        // if(Input.GetKeyDown(KeyCode.A))
        // {
        //     StartScaling();
        // }
        // 如果正在缩放，则逐帧更新
        if (isScaling)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 线性插值缩放
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            // 缩放完成后停止
            if (t >= 1f)
            {
                isScaling = false;
                Debug.Log("缩放完成！");
            }
        }
    }

    // 启动缩放
    public void StartScaling()
    {
        if (!isScaling)
        {
            elapsedTime = 0f; // 重置时间
            isScaling = true; // 启动缩放
            Debug.Log("开始缩放物体！");
        }
    }
}
