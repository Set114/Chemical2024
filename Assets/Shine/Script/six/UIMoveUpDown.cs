using UnityEngine;

public class UIMoveUpDown : MonoBehaviour
{
    public float moveDistance = 30f;  // 移動距離（上下各一半）
    public float moveSpeed = 2f;      // 移動速度

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);
    }
}
