using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class ShowBubbleChangeColor : MonoBehaviour
{
    public GameObject bubble;
    public Color startColor = Color.white;
    public Color endColor = Color.gray;
    public float duration = 2f;
    public string targetTag = "Object";
    public bool flag = false;

    private Material mat;
    
    public LevelEndSequence levelEndSequence;
    public FeAniAnimationController feAniAnimationController;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        mat = rend.material; // 獲取材質
    }

    // 檢測是否碰撞
    void OnTriggerEnter(Collider other)
    {
        if (flag == false)
        {
            if (other.CompareTag(targetTag))
            {
                // 激活 bubble GameObject 和 改變顏色
                bubble.SetActive(true);
                StartCoroutine(ChangeColorOverTime());
                feAniAnimationController.ResumeAnimation();
            }

        }
        flag = true;
    }

    IEnumerator ChangeColorOverTime()
    {
        float elapsedTime = 0f;
        Color startAlbedo = mat.GetColor("_Albedo");
        while (elapsedTime < duration)
        {
            // 設置 Albedo 改變顏色
            mat.SetColor("_Albedo", Color.Lerp(startAlbedo, endColor, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 確保時間結束後顏色正好是 endColor
        mat.SetColor("_Albedo", endColor);

        levelEndSequence.EndLevel(false,true, 1f, 6f, 1f, "1", () => { });
    }

}
