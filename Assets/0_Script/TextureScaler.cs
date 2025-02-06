using UnityEngine;

public class TextureScaler : MonoBehaviour
{
    private Material material;  // 貼圖的材質
    [SerializeField] private float min, max; 
    [SerializeField] private float ratio = 0.1f; // 貼圖縮放比例
    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        // 確保材質不為空
        if (material == null) return;

        //轉換比例範圍
        float mappedValue = Mathf.Lerp(min, max, ratio);

        // 設定材質的 Tiling (縮放)
        material.SetTextureScale("_MainTex", new Vector2(mappedValue, mappedValue));

        // 讓貼圖從中心放大
        float offset = (1 - mappedValue) / 2;
        material.SetTextureOffset("_MainTex", new Vector2(offset, offset));
    }

    // 可透過外部呼叫來改變貼圖大小
    public void SetRatio(float newRatio)
    {
        ratio = newRatio;
    }
}
