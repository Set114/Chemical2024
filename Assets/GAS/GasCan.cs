using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GasCan : MonoBehaviour
{
    public GameObject cap;
    public GameObject cap1;
    public float targetScaleMultiplier = 1.5f; // 目标缩放倍数
    public float scaleSpeed = 0.3f; // 缩放速度

    private Renderer renderer1;
    private Renderer renderer2;
    private bool shouldScale = false;
    private bool shouldPlayAnimation = true;
    private Vector3 initialScaleCap;
    private Vector3 initialScaleCap1;

    //public LevelEndSequence levelEndSequence;
    public FeAniAnimationController feAniAnimationController;

    private LevelObjManager levelObjManager;

    void Start()
    {
        levelObjManager = FindObjectOfType<LevelObjManager>();
        renderer1 = cap.GetComponent<Renderer>();
        renderer2 = cap1.GetComponent<Renderer>();

        if (renderer1 != null)
            renderer1.enabled = false;

        if (renderer2 != null)
            renderer2.enabled = false;

        // 记录初始缩放
        initialScaleCap = cap.transform.localScale;
        initialScaleCap1 = cap1.transform.localScale;
    }

    void Update()
    {
        if (shouldScale)
        {
            // 计算目标缩放
            Vector3 targetScaleCap = initialScaleCap * targetScaleMultiplier;
            Vector3 targetScaleCap1 = initialScaleCap1 * targetScaleMultiplier;

            // 平滑缩放
            if (cap != null && cap.transform.localScale != targetScaleCap)
            {
                cap.transform.localScale = Vector3.Lerp(cap.transform.localScale, targetScaleCap, scaleSpeed * Time.deltaTime);
            }

            if (cap1 != null && cap1.transform.localScale != targetScaleCap1)
            {
                cap1.transform.localScale = Vector3.Lerp(cap1.transform.localScale, targetScaleCap1, scaleSpeed * Time.deltaTime);
            }

            // 检查是否达到目标缩放
            if (cap.transform.localScale == targetScaleCap && cap1.transform.localScale == targetScaleCap1)
            {
                shouldScale = false; // 停止缩放
            }

            
        }
    }
    

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered with: " + other.gameObject.name);
        if(shouldPlayAnimation == true)
        {
            if (other.gameObject.CompareTag("IronType1"))
            {
                feAniAnimationController.ResumeAnimation();
                if (renderer1 != null)
                    renderer1.enabled = true;

                if (renderer2 != null)
                    renderer2.enabled = true;

                // 开始放大
                shouldScale = true;

                shouldPlayAnimation = false;

                //levelEndSequence.EndLevel(false,true, 1f, 6f, 1f, "1", () => { });
                levelObjManager.LevelClear("1", "");
            }
        }
    }
}