using UnityEngine;
using System.Collections;

public class ChangeColor : MonoBehaviour
{
    public Color startColor = Color.white;
    public Color endColor = Color.gray;
    public float duration = 2f;
    public GameObject bubble; 

    private Material mat;

    void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        mat = rend.material; // 獲取材質
        mat.SetColor("_Color", startColor); // 設置初始顏色
    }

    private void Update()
    {
        if (bubble.activeSelf)
        {
            StartCoroutine(ChangeCoroutine());
        }
    }

    IEnumerator ChangeCoroutine()
    {
        float elapsedTime = 0f;
        Color startAlbedo = mat.GetColor("_Color");
        while (elapsedTime < duration)
        {
            // 設置 Color 改變顏色
            mat.SetColor("_Color", Color.Lerp(startAlbedo, endColor, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 確保時間結束後顏色正好是 endColor
        mat.SetColor("_Color", endColor);
    }
}//有改過程式碼