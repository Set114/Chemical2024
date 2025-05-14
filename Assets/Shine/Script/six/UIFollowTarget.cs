using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    public RectTransform targetUI;  // BUI
    public float moveSpeed = 5f;    // 移動速度

    private RectTransform selfRect;

    void Start()
    {
        selfRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (targetUI == null) return;

        // 計算從 AUI 位置移動到 BUI 的差距（anchoredPosition）
        Vector2 currentPos = selfRect.anchoredPosition;
        Vector2 targetPos = targetUI.anchoredPosition;

        // 平滑移動
        selfRect.anchoredPosition = Vector2.Lerp(currentPos, targetPos, Time.deltaTime * moveSpeed);
    }
}
