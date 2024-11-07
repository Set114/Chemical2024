using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class CubeTransformDataList
{
    public Transform cubeTransform; // 方塊的Transform
    public Vector3 initialScale = Vector3.one; // 初始方塊縮放
    public Vector3 targetScale = new Vector3(2f, 2f, 2f); // 目標方塊縮放
    public Vector3 targetPosition; // 目標方塊位置
}

[Serializable]
public class CameraControllerList
{
    public string name; // 名稱
    public Animator[] animators; // 動畫控制器數組
}

public class CameraController : MonoBehaviour
{
    private int currentLevel = 1; // 當前關卡
    [Header("Camera")]
    [SerializeField] float targetSize = 13f; // 目標相機大小
    [SerializeField] float initialSize = 2.5f; // 初始相機大小
    [SerializeField] float duration = 1f; // 平滑過渡的持續時間
    [SerializeField] TMP_Text targetshowLevelText; // 顯示級別的文字
    private Camera cam; // 相機引用
    public Vector3 camPosition;
    [Header("Animator List")]
    public List<CameraControllerList> animatorsList; // 動畫控制器列表

    [Header("Screen")]
    [SerializeField] CubeTransformDataList[] cubeTransformDataArray; // 方塊變換數據數組
    private Vector3[] initialCubePositions; // 初始方塊位置數組
    public SwitchUI switchUI;

    private string animationTrigger; // 動畫觸發器

    void Start()
    {
        // cam = GetComponent<Camera>();
        // if (cam == null)
        // {
        //     Debug.LogError("Camera component not found on the GameObject.");
        //     return;
        // }

        // cam.orthographic = true; // 設置相機為正交模式
        // cam.orthographicSize = initialSize; // 設置初始相機大小


        // 設置動畫觸發器
        // SetAnimationTrigger(targetshowLevelText.text);

        // // 初始化方塊位置數組
        // if (cubeTransformDataArray != null)
        // {
        //     initialCubePositions = new Vector3[cubeTransformDataArray.Length];
        //     for (int i = 0; i < cubeTransformDataArray.Length; i++)
        //     {
        //         initialCubePositions[i] = cubeTransformDataArray[i].cubeTransform.localPosition;
        //     }
        // }

        // cam.transform.position = camPosition;
    }

    // 根據顯示級別文本設置動畫觸發器
    void SetAnimationTrigger(int levelindex)
    {
        foreach (var controller in animatorsList)
        {
            if (controller.name == levelindex.ToString())
            {
                animationTrigger = controller.name;
                Debug.Log("GetNAEM" + animationTrigger);
                return;
            }
        }
    }

    // 觸發動畫
    void TriggerAnimation()
    {
        foreach (var controller in animatorsList)
        {
            if (controller.name == animationTrigger)
            {
                foreach (var animator in controller.animators)
                {
                    if (animator.gameObject.activeSelf)
                    {
                        animator.SetTrigger(animationTrigger);
                    }
                }
                return;
            }
        }
    }

    // 放大相機和方塊
    public void ZoomIn()
    {
        cam.transform.position = camPosition;
        // cam.orthographicSize = initialSize; // 重置相機大小
        SetAnimationTrigger(int.Parse(targetshowLevelText.text));// 設置動畫觸發器
        //print(targetshowLevelText.text);
        // StartCoroutine(SmoothZoom(initialSize, targetSize, duration)); // 平滑過渡相機大小

        Vector3[] targetPositions = new Vector3[cubeTransformDataArray.Length];
        // for (int i = 0; i < targetPositions.Length; i++)
        // {
        //     targetPositions[i] = cubeTransformDataArray[i].targetPosition; // 將目標位置賦值給每個方塊
        // }
        // StartCoroutine(HandleCubesSmoothTransform(cubeTransformDataArray, duration, true)); // 平滑過渡方塊的縮放和位置
        Invoke(nameof(TriggerAnimation), 0.2f); // 延遲觸發動畫
    }

    public void ZoomInAni()
    {
        int index = switchUI.GetLevelCount() +1;
        SetAnimationTrigger(index); // 設置動畫觸發器
        Invoke(nameof(TriggerAnimation), 0.5f); // 延遲觸發動畫
    }

    public void ZoomOut()
    {
        cam.transform.position = camPosition;
        StartCoroutine(SmoothZoom(cam.orthographicSize, initialSize, duration)); // 平滑過渡相機大小

        StartCoroutine(HandleCubesSmoothTransform(cubeTransformDataArray, duration, false)); // 平滑過渡方塊的縮放和位置
    }

    // 平滑過渡相機大小的協程
    IEnumerator SmoothZoom(float startSize, float endSize, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            cam.orthographicSize = Mathf.Lerp(startSize, endSize, elapsed / duration);

            // 調整相機的位置，使螢幕從中心向外放大
            Vector3 centerPosition = CalculateCenterPosition(); // 新增計算中心位置

            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.orthographicSize = endSize; // 確保最終大小
    }

    // 計算方塊的中心位置
    Vector3 CalculateCenterPosition() // 新增方法計算方塊的中心位置
    {
        Vector3 center = Vector3.zero;
        foreach (var cubeData in cubeTransformDataArray)
        {
            center += cubeData.cubeTransform.position;
        }
        center /= cubeTransformDataArray.Length;

        return center;
    }

    // 平滑過渡方塊的縮放和位置的協程
    IEnumerator HandleCubesSmoothTransform(CubeTransformDataList[] cubeDataArray, float duration, bool zoomIn)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            for (int i = 0; i < cubeDataArray.Length; i++)
            {
                var cubeData = cubeDataArray[i];
                cubeData.cubeTransform.localScale = Vector3.Lerp(zoomIn ? cubeData.initialScale : cubeData.targetScale, zoomIn ? cubeData.targetScale : cubeData.initialScale, elapsed / duration);
                cubeData.cubeTransform.localPosition = Vector3.Lerp(zoomIn ? initialCubePositions[i] : cubeData.targetPosition, zoomIn ? cubeData.targetPosition : initialCubePositions[i], elapsed / duration);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < cubeDataArray.Length; i++)
        {
            var cubeData = cubeDataArray[i];
            cubeData.cubeTransform.localScale = zoomIn ? cubeData.targetScale : cubeData.initialScale;
            cubeData.cubeTransform.localPosition = zoomIn ? cubeData.targetPosition : initialCubePositions[i];
        }
    }

    // 重置相機大小和方塊的縮放及位置
    public void ResetCameraSize()
    {
        cam.orthographicSize = initialSize;
        if (cubeTransformDataArray != null)
        {
            for (int i = 0; i < cubeTransformDataArray.Length; i++)
            {
                var cubeData = cubeTransformDataArray[i];
                cubeData.cubeTransform.localScale = cubeData.initialScale;
                cubeData.cubeTransform.localPosition = initialCubePositions[i];
            }
        }
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }
}
